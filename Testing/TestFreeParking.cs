namespace Parknow.Testing;
public class TestFreeParking
{
    public class TimeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        public override string ToString()
        {
            return $"Start: {Start:yyyy-MM-dd HH:mm}, End: {End:yyyy-MM-dd HH:mm}";
        }
    }
    

    public List<TimeSlot> GetTimeSlots(DateTime start, DateTime end, TimeSpan freeStartTime, TimeSpan freeEndTime)
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
    
    public static void PrintTestCase(string testName, List<TimeSlot> slots)
    {
        Console.WriteLine($"\n=== {testName} ===");
        Console.WriteLine($"Number of slots: {slots.Count}");
        for (int i = 0; i < slots.Count; i++)
        {
            Console.WriteLine($"Slot {i + 1}: {slots[i]}");
        }
        Console.WriteLine("================");
    }
    
	public static void Main()
	{
    	var program = new TestFreeParking();
		var times = "SUN & PH FR 1PM-10.30PM".Split(' ').Last().Split('-');
		var freeStart = DateTime.ParseExact(times[0].Replace("AM", " AM").Replace("PM", " PM"), "h tt", null).TimeOfDay;
		var freeEnd = DateTime.ParseExact(times[1].Replace(".", ":").Replace("PM", " PM").Replace("AM", " AM"), "h:mm tt", null).TimeOfDay;
    
		// Test Case 1: No Sundays (Tuesday to Thursday)
		{
			var start = new DateTime(2024, 1, 2, 8, 0, 0);  // Tuesday
			var end = new DateTime(2024, 1, 4, 17, 0, 0);   // Thursday

			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 1: No Sundays (Tuesday to Thursday)", result);
		}

		// Test Case 2: One Sunday (Friday to Monday)
		{
			var start = new DateTime(2024, 1, 5, 8, 0, 0);  // Friday
			var end = new DateTime(2024, 1, 8, 17, 0, 0);   // Monday

			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 2: One Sunday (Friday to Monday)", result);
		}

		// Test Case 3: Starts on Sunday
		{
			var start = new DateTime(2024, 1, 7, 8, 0, 0);  // Sunday
			var end = new DateTime(2024, 1, 9, 17, 0, 0);   // Tuesday
			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 3: Starts on Sunday", result);
		}

		// Test Case 4: Multiple Sundays
		{
			var start = new DateTime(2024, 1, 5, 8, 0, 0);  // Friday
			var end = new DateTime(2024, 1, 15, 17, 0, 0);  // Monday

			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 4: Multiple Sundays (Two Week Period)", result);
		}

		// Test Case 5: Sunday Morning Only (Ends Before Free Time)
		{
			var start = new DateTime(2024, 1, 7, 8, 0, 0);  // Sunday
			var end = new DateTime(2024, 1, 7, 11, 0, 0);   // Sunday before free time

			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 5: Sunday Morning Only (Ends Before Free Time)", result);
		}

		// Test Case 6: Sunday Afternoon Only (Starts After Free Time)
		{
			var start = new DateTime(2024, 1, 7, 15, 0, 0);  // Sunday after free time
			var end = new DateTime(2024, 1, 7, 17, 0, 0);    // Sunday
			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 6: Sunday Afternoon Only (Starts After Free Time)", result);
		}

		// Test Case 7: Completely Within Free Time on Sunday
		{
			var start = new DateTime(2024, 1, 7, 12, 30, 0);  // Sunday during free time
			var end = new DateTime(2024, 1, 7, 13, 30, 0);    // Sunday during free time

			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 7: Completely Within Free Time on Sunday", result);
		}

		// Test Case 8: Starting Within Free Time
		{
			var start = new DateTime(2024, 1, 7, 13, 0, 0);  // Sunday during free time
			var end = new DateTime(2024, 1, 7, 15, 0, 0);    // Sunday after free time

			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 8: Starting Within Free Time", result);
		}

		// Test Case 9: Ending Within Free Time
		{
			var start = new DateTime(2024, 1, 7, 11, 0, 0);  // Sunday before free time
			var end = new DateTime(2024, 1, 7, 13, 0, 0);    // Sunday during free time

			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 9: Ending Within Free Time", result);
		}

		// Test Case 10: Single Minute Within Free Time
		{
			var start = new DateTime(2024, 1, 7, 12, 30, 0);  // Sunday during free time
			var end = new DateTime(2024, 1, 7, 12, 31, 0);    // Sunday during free time
			var result = program.GetTimeSlots(start, end, freeStart, freeEnd);
			PrintTestCase("Test Case 10: Single Minute Within Free Time", result);
		}
	}
}