namespace HabitTracker.Models;

public class Habit
{
    public string Name { get; set; } = "";
    public int Streak {get; set;}
    public List<DateTime> CompletedDates { get; set;} = new();
}