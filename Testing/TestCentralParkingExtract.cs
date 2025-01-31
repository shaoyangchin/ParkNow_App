namespace ParkNow.Testing;

public class TestCentralParkingExtract
{
    public class TimeSlot
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public override string ToString() => $"{Start:yyyy-MM-dd HH:mm} to {End:yyyy-MM-dd HH:mm}";
	}

	public static (List<TimeSlot> normal, List<TimeSlot> special) SplitCentralSlots(DateTime start, DateTime end)
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

	static void PrintTestCase(string testName, (List<TimeSlot> normal, List<TimeSlot> special) result)
	{
		Console.WriteLine($"\n{testName}");
		Console.WriteLine("Normal hours:");
		if (!result.normal.Any()) Console.WriteLine("None");
		else result.normal.ForEach(t => Console.WriteLine(t));

		Console.WriteLine("Central hours (7 AM - 5 PM):");
		if (!result.special.Any()) Console.WriteLine("None");
		else result.special.ForEach(t => Console.WriteLine(t));
	}
	
	public static void Main()
	{
		// Test Case 1: Regular central day (Monday 6 AM to 8 PM)
		{
			var start = new DateTime(2024, 1, 1, 6, 0, 0);   // Monday 6 AM
			var end = new DateTime(2024, 1, 1, 20, 0, 0);    // Monday 8 PM

			var result = SplitCentralSlots(start, end);
			PrintTestCase("Test Case 1: Regular central day (Monday 6 AM to 8 PM)", result);
		}

		// Test Case 2: Across multiple central days (Monday to Wednesday)
		{
			var start = new DateTime(2024, 1, 1, 6, 0, 0);   // Monday 6 AM
			var end = new DateTime(2024, 1, 3, 20, 0, 0);    // Wednesday 8 PM

			var result = SplitCentralSlots(start, end);
			PrintTestCase("Test Case 2: Across multiple central days", result);
		}

		// Test Case 3: Including Sunday
		{
			var start = new DateTime(2024, 1, 6, 20, 0, 0);  // Saturday 8 PM
			var end = new DateTime(2024, 1, 8, 8, 0, 0);     // Monday 8 AM

			var result = SplitCentralSlots(start, end);
			PrintTestCase("Test Case 3: Including Sunday", result);
		}

		// Test Case 4: During central hours only
		{
			var start = new DateTime(2024, 1, 1, 8, 0, 0);   // Monday 8 AM
			var end = new DateTime(2024, 1, 1, 16, 0, 0);    // Monday 4 PM

			var result = SplitCentralSlots(start, end);
			PrintTestCase("Test Case 4: During central hours only", result);
		}

		// Test Case 5: Outside central hours only
		{
			var start = new DateTime(2024, 1, 1, 18, 0, 0);  // Monday 6 PM
			var end = new DateTime(2024, 1, 2, 6, 0, 0);     // Tuesday 6 AM

			var result = SplitCentralSlots(start, end);
			PrintTestCase("Test Case 5: Outside central hours only", result);
		}
	}
}
