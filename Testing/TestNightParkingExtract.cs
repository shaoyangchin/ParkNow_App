namespace ParkNow.Testing;

public class TestNightParkingExtract
{
    public class TimeSlot
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public override string ToString() => $"{Start:yyyy-MM-dd HH:mm} to {End:yyyy-MM-dd HH:mm}";
	}

	public static (List<TimeSlot> allowed, List<TimeSlot> restricted) SplitOvernight(DateTime start, DateTime end)
	{
		var allowed = new List<TimeSlot>();
		var restricted = new List<TimeSlot>();

		var currentDate = start.Date;
		var currentTime = start;

		// Helper function to check if a time is in restricted period
		bool IsInRestrictedPeriod(DateTime time)
		{
			var timeOfDay = time.TimeOfDay;
			var nightStart = new TimeSpan(22, 30, 0);  // 10:30 PM
			var morningEnd = new TimeSpan(7, 0, 0);    // 7:00 AM
			return timeOfDay >= nightStart || timeOfDay < morningEnd;
		}

		while (currentTime < end)
		{
			var restrictStart = currentDate.AddHours(22).AddMinutes(30); // 10:30 PM
			var restrictEnd = currentDate.AddDays(1).AddHours(7);        // 7 AM next day

			// If starting during restricted period
			if (IsInRestrictedPeriod(currentTime))
			{
				// Find the end of current restriction period
				var currentRestrictEnd = currentTime.TimeOfDay < new TimeSpan(7, 0, 0) 
					? currentDate.AddHours(7) 
					: currentDate.AddDays(1).AddHours(7);

				restricted.Add(new TimeSlot {
					Start = currentTime,
					End = end < currentRestrictEnd ? end : currentRestrictEnd
				});

				currentTime = currentRestrictEnd;
				if (currentTime >= end) break;
			}

			// Handle allowed period until next restriction
			var nextRestrict = currentTime.Date.AddHours(22).AddMinutes(30);
			if (currentTime < nextRestrict)
			{
				allowed.Add(new TimeSlot {
					Start = currentTime,
					End = end < nextRestrict ? end : nextRestrict
				});
				currentTime = nextRestrict;
			}

			// Handle overnight restriction if we haven't reached the end
			if (currentTime < end)
			{
				var nextAllowed = currentTime.Date.AddDays(1).AddHours(7);
				restricted.Add(new TimeSlot {
					Start = currentTime,
					End = end < nextAllowed ? end : nextAllowed
				});
				currentTime = nextAllowed;
			}

			currentDate = currentDate.AddDays(1);
		}

		return (allowed, restricted);
	}
	public static void PrintTestCase(string testName, (List<TimeSlot> allowed, List<TimeSlot> restricted) result)
	{
		Console.WriteLine($"\n{testName}");
		Console.WriteLine("Allowed periods:");
		if (!result.allowed.Any()) Console.WriteLine("None");
		else result.allowed.ForEach(t => Console.WriteLine(t));

		Console.WriteLine("Restricted periods:");
		if (!result.restricted.Any()) Console.WriteLine("None");
		else result.restricted.ForEach(t => Console.WriteLine(t));
	}
	
	public static void Main()
	{
		// Test Case 1: Regular overnight (8 PM - 8 AM)
		{
			var start = new DateTime(2024, 1, 1, 20, 0, 0);  // 8 PM
			var end = new DateTime(2024, 1, 2, 8, 0, 0);     // 8 AM next day

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 1: Regular overnight (8 PM - 8 AM)", result);
		}

		// Test Case 2: Multi-day (8 PM - 8 AM two days later)
		{
			var start = new DateTime(2024, 1, 1, 20, 0, 0);  // 8 PM
			var end = new DateTime(2024, 1, 3, 8, 0, 0);     // 8 AM two days later

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 2: Multi-day (8 PM - 8 AM two days later)", result);
		}

		// Test Case 3: Ends during restricted time (8 PM - 2 AM)
		{
			var start = new DateTime(2024, 1, 1, 20, 0, 0);  // 8 PM
			var end = new DateTime(2024, 1, 2, 2, 0, 0);     // 2 AM next day

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 3: Ends during restricted time (8 PM - 2 AM)", result);
		}

		// Test Case 4: Starts during restricted time (2 AM - 8 AM)
		{
			var start = new DateTime(2024, 1, 1, 2, 0, 0);   // 2 AM
			var end = new DateTime(2024, 1, 1, 8, 0, 0);     // 8 AM same day

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 4: Starts during restricted time (2 AM - 8 AM)", result);
		}

		// Test Case 5: Entirely within restricted time (11 PM - 6 AM)
		{
			var start = new DateTime(2024, 1, 1, 23, 0, 0);  // 11 PM
			var end = new DateTime(2024, 1, 2, 6, 0, 0);     // 6 AM next day

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 5: Entirely within restricted time (11 PM - 6 AM)", result);
		}

		// Test Case 6: Entirely within allowed time (2 PM - 8 PM)
		{
			var start = new DateTime(2024, 1, 1, 14, 0, 0);  // 2 PM
			var end = new DateTime(2024, 1, 1, 20, 0, 0);    // 8 PM same day

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 6: Entirely within allowed time (2 PM - 8 PM)", result);
		}

		// Test Case 7: Multiple days ending in restricted time (2 PM - 2 AM two days later)
		{
			var start = new DateTime(2024, 1, 1, 14, 0, 0);  // 2 PM
			var end = new DateTime(2024, 1, 3, 2, 0, 0);     // 2 AM two days later

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 7: Multiple days ending in restricted time", result);
		}

		// Test Case 8: Multiple days starting in restricted time (2 AM - 8 PM next day)
		{
			var start = new DateTime(2024, 1, 1, 2, 0, 0);   // 2 AM
			var end = new DateTime(2024, 1, 2, 20, 0, 0);    // 8 PM next day

			var result = SplitOvernight(start, end);
			PrintTestCase("Test Case 8: Multiple days starting in restricted time", result);
		}
	}
}
