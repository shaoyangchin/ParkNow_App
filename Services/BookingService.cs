using Microsoft.EntityFrameworkCore;
using ParkNow.Data;
using ParkNow.Models;

namespace ParkNow.Services;

public class BookingService : IBookingService
{
    // Get Database Context and Constructor to include context
    private readonly ILogger<BookingService> _logger;
    private readonly AppDbContext _context;
    public BookingService(AppDbContext context, ILogger<BookingService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Booking>> GetUserBookings(string username) {
        List<Booking> User_Bookings = await _context.Bookings
        .Include(b => b.Vehicle)
        .Include(b => b.Carpark)
        .Include(b => b.Payment)
            .ThenInclude(p => p.Voucher)
        .Where(v => v.User.Username == username).ToListAsync();
        foreach (Booking book in User_Bookings) {
            // Completed
            if (DateTime.Now > book.EndTime) {
                book.Status = Booking.Statuses.Completed;
            }
            // Active
            else if (DateTime.Now > book.StartTime){
                book.Status = Booking.Statuses.Active;
            }
        }
        await _context.SaveChangesAsync();
        return User_Bookings;
    }
    public async Task<Booking> GetBooking(int bookingId) {
        return await _context.Bookings.Where(b => b.BookingId == bookingId).FirstOrDefaultAsync();
    }

    public async Task<bool> CreateBooking(Booking booking) {
       try {
            // Add Booking and Payment
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            Payment temp_pay = new Payment{
                Booking = booking,
                Timestamp = DateTime.Now,
                Amount = booking.Cost,
                Status = Payment.Statuses.Success
            };
            await _context.Payments.AddAsync(temp_pay);
            booking.Payment = temp_pay;
            await _context.SaveChangesAsync();
            return true;
       }
       catch (Exception e){
            _logger.LogInformation(e.Message);
            _logger.LogInformation(e.InnerException.Message);
            return false;
       }
    }

    public async Task<bool> CreateBooking(Booking booking, Voucher voucher) {
       try {
            // Add Booking and Payment
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            Payment temp_pay = new Payment{
                Voucher = voucher,
                Discount = voucher.Amount,
                Booking = booking,
                Timestamp = DateTime.Now,
                Amount = Math.Max(0, booking.Cost - voucher.Amount),
                Status = Payment.Statuses.Success
            };
            await _context.Payments.AddAsync(temp_pay);
            booking.Payment = temp_pay;
            await _context.SaveChangesAsync();
            return true;
       }
       catch (Exception e){
            _logger.LogInformation(e.Message);
            _logger.LogInformation(e.InnerException.Message);
            return false;
       }
    }

    public async Task<bool> UpdateBooking(Booking booking) {
       try {
            // Update Booking and Payment
            Booking? db_Booking = await _context.Bookings.Where(b => b.BookingId == booking.BookingId).FirstOrDefaultAsync();
            Payment? db_Payment = await _context.Payments.Where(p => p.Booking.BookingId == booking.BookingId).FirstOrDefaultAsync();
            if (db_Booking == null || db_Payment == null) {
                return false;
            }

            db_Booking.StartTime = booking.StartTime;
            db_Booking.EndTime= booking.EndTime;
            db_Booking.Cost = booking.Cost;
            db_Booking.Status = booking.Status;

            if (db_Payment.Discount != null) {
                db_Payment.Amount = Math.Max(0,booking.Cost - db_Payment.Discount.Value);
            }
            else {
                db_Payment.Amount = booking.Cost;
            }
            await _context.SaveChangesAsync();
       }
       catch (Exception ex) {
            _logger.LogError(ex.Message);
            return false;
       }
        return true;
    }
    public async Task<bool> DeleteBooking(int bookingId) {
        try {
            // Delete Booking and Payment
            Booking? db_Booking = await _context.Bookings.Where(b => b.BookingId == bookingId).FirstOrDefaultAsync();
            Payment? db_Payment = await _context.Payments.Where(p => p.Booking.BookingId == bookingId).FirstOrDefaultAsync();
            if (db_Booking == null || db_Payment == null) {
                return false;
            }
            _context.Bookings.Remove(db_Booking);
            await _context.SaveChangesAsync();
       }
       catch (Exception ex) {
            _logger.LogError(ex.Message);
            return false;
       }
        return true;
    }

    // Price Calculation
    public class TimeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public Decimal CalculatePrice(DateTime start, DateTime end, Carpark carpark) {
        List<TimeSlot> paid_slots = new List<TimeSlot>();

        List<TimeSlot> day_slots = new List<TimeSlot>();
        List<TimeSlot> night_slots = new List<TimeSlot>();
        List<TimeSlot> central_slots = new List<TimeSlot>();

        Decimal TotalPrice = 0M;

        // Remove Free Parking Timings
        if (carpark.FreeParking == "NO") {
            paid_slots.Add(new TimeSlot 
            { 
                Start = start,
                End = end
            });
        }
        else {
            // SUN & PH FR 1PM-10.30PM and SUN & PH FR 7AM-10.30PM
            var times = carpark.FreeParking.Split(' ').Last().Split('-');
            var startTime = DateTime.ParseExact(times[0].Replace("AM", " AM").Replace("PM", " PM"), "h tt", null).TimeOfDay;
            var endTime = DateTime.ParseExact(times[1].Replace(".", ":").Replace("PM", " PM").Replace("AM", " AM"), "h:mm tt", null).TimeOfDay;
            // Get Non Free Time Slots
            paid_slots = RemoveFreeTimeSlots(start,end,startTime,endTime);
            
        }

        // Ensure Short Term Parking Available
        if (carpark.ShortTermParkingType == "NO") {
            // Not Allowed
            return -1.00M;
        }
        // If requires parsing
        else if (carpark.ShortTermParkingType != "WHOLE DAY") {
            // 7AM-10.30PM 7AM-7PM
            var times = carpark.ShortTermParkingType.Split('-');
            var startTime = DateTime.ParseExact(times[0].Replace("AM", " AM"), "h tt", null).TimeOfDay;
            var endStr = times[1].Replace(".", ":").Replace("PM", " PM");
            var endFormat = endStr.Contains(":") ? "h:mm tt" : "h tt";
            var endTime = DateTime.ParseExact(endStr, endFormat, null).TimeOfDay;

            // Check If Short Term Parking Allowed
            foreach (TimeSlot ts in paid_slots) {
                var currentDate = ts.Start.Date;
                var endDate = ts.End.Date;

                while (currentDate <= endDate)
                {
                    var dayStart = currentDate == ts.Start.Date ? ts.Start : currentDate;
                    var dayEnd = currentDate == ts.End.Date ? ts.End : currentDate.AddDays(1);

                    // For each day, the timeslot must be fully contained within allowed hours
                    if (dayStart.TimeOfDay < startTime || 
                        (dayEnd.TimeOfDay > endTime && dayEnd.Date == currentDate) ||  // Same day end
                        (dayEnd.Date > currentDate))  // Spans to next day
                    {
                        return -1.00M;
                    }

                    currentDate = currentDate.AddDays(1);
                }
                day_slots.Add(ts);
            }
        }
        // Whole Day means night parking available
        else {
            // Split Day and Night Parking
            foreach (TimeSlot ts in paid_slots) {
                var result = SplitOvernightSlots(ts.Start, ts.End);
                day_slots.AddRange(result.day);
                night_slots.AddRange(result.night);
            }
        }

        // Calculate Central Charges on Day Slots
        if (carpark.CentralCharge == true) {
            // If true, split out central charge timings
            List<TimeSlot> temp_day_slots = new List<TimeSlot>();
            foreach (TimeSlot ts in day_slots) {
                var result = SplitCentralSlots(ts.Start, ts.End);
                temp_day_slots.AddRange(result.normal);
                central_slots.AddRange(result.central);
            }
            day_slots = temp_day_slots;
        }
        // Add Day Slot Charges
        foreach (TimeSlot ts in day_slots) {
            Decimal price = Convert.ToDecimal((ts.End - ts.Start).TotalHours * 0.6);
            if (price > 12.0M) {
                TotalPrice += 12.0M;
            }
            else {
                TotalPrice += price;
            }
        }
        // Add Night Slot Charges
        foreach (TimeSlot ts in day_slots) {
            Decimal price = Convert.ToDecimal((ts.End - ts.Start).TotalHours * 0.6);
            if (price > 5.0M) {
                TotalPrice += 5.0M;
            }
            else {
                TotalPrice += price;
            }
        }
        // Add Central Slot Charges
        foreach (TimeSlot ts in central_slots) {
            Decimal price = Convert.ToDecimal((ts.End - ts.Start).TotalHours * 1.2);
            if (price > 20.0M) {
                TotalPrice += 20.0M;
            }
            else {
                TotalPrice += price;
            }
        }
        return decimal.Round(TotalPrice, 2);;
    }
    public static List<TimeSlot> RemoveFreeTimeSlots(DateTime start, DateTime end, TimeSpan freeStartTime, TimeSpan freeEndTime)
    {
        var result = new List<TimeSlot>();
        var currentStart = start;
        
        while (currentStart < end)
        {
            if (currentStart.DayOfWeek == DayOfWeek.Sunday)
            {
                var sundayFreeStart = currentStart.Date + freeStartTime;
                var sundayFreeEnd = currentStart.Date + freeEndTime;
                
                // If we're before free time on Sunday
                if (currentStart < sundayFreeStart)
                {
                    result.Add(new TimeSlot 
                    { 
                        Start = currentStart,
                        End = sundayFreeStart < end ? sundayFreeStart : end
                    });
                    currentStart = sundayFreeEnd;
                }
                // If we're during free time
                else if (currentStart >= sundayFreeStart && currentStart < sundayFreeEnd)
                {
                    currentStart = sundayFreeEnd;
                }
                
                // If we're after free time or moved past it
                if (currentStart < end && currentStart >= sundayFreeEnd)
                {
                    var nextDay = currentStart.Date.AddDays(1);
                    result.Add(new TimeSlot 
                    { 
                        Start = currentStart,
                        End = nextDay < end ? nextDay : end
                    });
                    currentStart = nextDay;
                }
            }
            else
            {
                // For non-Sunday, go until next day or end
                var nextDay = currentStart.Date.AddDays(1);
                var slotEnd = nextDay < end ? nextDay : end;
                
                result.Add(new TimeSlot 
                { 
                    Start = currentStart,
                    End = slotEnd
                });
                currentStart = nextDay;
            }
        }
		// Merge continuous time slots
		if (result.Count > 1)
		{
			var mergedSlots = new List<TimeSlot>();
			var currentSlot = result[0];

			for (int i = 1; i < result.Count; i++)
			{
				if (currentSlot.End == result[i].Start)
				{
					// Merge the slots
					currentSlot.End = result[i].End;
				}
				else
				{
					// Add the completed slot and start a new one
					mergedSlots.Add(currentSlot);
					currentSlot = result[i];
				}
			}
			// Add the last slot
			mergedSlots.Add(currentSlot);

			return mergedSlots;
		}
        
        return result;
    }

    public static (List<TimeSlot> day, List<TimeSlot> night) SplitOvernightSlots(DateTime start, DateTime end)
	{
		var day = new List<TimeSlot>();
		var night = new List<TimeSlot>();

		var currentDate = start.Date;
		var currentTime = start;

		// Helper function to check if a time is in night period
		bool IsInNightPeriod(DateTime time)
		{
			var timeOfDay = time.TimeOfDay;
			var nightStart = new TimeSpan(22, 30, 0);  // 10:30 PM
			var morningEnd = new TimeSpan(7, 0, 0);    // 7:00 AM
			return timeOfDay >= nightStart || timeOfDay < morningEnd;
		}

		while (currentTime < end)
		{
			// If starting during night period
			if (IsInNightPeriod(currentTime))
			{
				// Find the end of current restriction period
				var currentNightEnd = currentTime.TimeOfDay < new TimeSpan(7, 0, 0) 
					? currentDate.AddHours(7) 
					: currentDate.AddDays(1).AddHours(7);

				night.Add(new TimeSlot {
					Start = currentTime,
					End = end < currentNightEnd ? end : currentNightEnd
				});

				currentTime = currentNightEnd;
				if (currentTime >= end) break;
			}

			// Handle day period until next restriction
			var nextNight = currentTime.Date.AddHours(22).AddMinutes(30);
			if (currentTime < nextNight)
			{
				day.Add(new TimeSlot {
					Start = currentTime,
					End = end < nextNight ? end : nextNight
				});
				currentTime = nextNight;
			}

			// Handle overnight restriction if we haven't reached the end
			if (currentTime < end)
			{
				var nextDay = currentTime.Date.AddDays(1).AddHours(7);
				night.Add(new TimeSlot {
					Start = currentTime,
					End = end < nextDay ? end : nextDay
				});
				currentTime = nextDay;
			}

			currentDate = currentDate.AddDays(1);
		}

		return (day, night);
	}

    public static (List<TimeSlot> normal, List<TimeSlot> central) SplitCentralSlots(DateTime start, DateTime end)
	{
		var normal = new List<TimeSlot>();
		var special = new List<TimeSlot>();

		var currentDate = start.Date;
		var currentTime = start;

		while (currentTime < end)
		{
			var centralStart = currentDate.AddHours(7);     // 7 AM
			var centralEnd = currentDate.AddHours(17);      // 5 PM
			var nextDate = currentDate.AddDays(1);

			if (currentDate.DayOfWeek != DayOfWeek.Sunday)
			{
				// If we start before central hours
				if (currentTime < centralStart)
				{
					normal.Add(new TimeSlot
					{
						Start = currentTime,
						End = end < centralStart ? end : centralStart
					});
					currentTime = centralStart;
				}

				// Handle central hours
				if (currentTime >= centralStart && currentTime < centralEnd && currentTime < end)
				{
					special.Add(new TimeSlot
					{
						Start = currentTime,
						End = end < centralEnd ? end : centralEnd
					});
					currentTime = centralEnd;
				}

				// Handle after central hours
				if (currentTime >= centralEnd && currentTime < nextDate && currentTime < end)
				{
					normal.Add(new TimeSlot
					{
						Start = currentTime,
						End = end < nextDate ? end : nextDate
					});
					currentTime = nextDate;
				}
			}
			else
			{
				// Handle full non-central day
				var dayEnd = currentTime.Date.AddDays(1);
				normal.Add(new TimeSlot
				{
					Start = currentTime,
					End = end < dayEnd ? end : dayEnd
				});
				currentTime = dayEnd;
			}

			currentDate = nextDate;
		}

		return (normal, special);
	}
}
