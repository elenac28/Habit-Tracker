using HabitTracker.Services;

var habitService = new HabitService();

bool running = true;

while (running)
{
    //Console.Clear();
    Console.WriteLine("1. Add Habit");
    Console.WriteLine("2. View Habits");
    Console.WriteLine("3. Mark Habit Complete");
    Console.WriteLine("4. Edit Habit");
    Console.WriteLine("5. Delete Habit");
    Console.WriteLine("6. Exit");
    Console.Write("Choose an option: ");


    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            habitService.AddHabit();
            break;
        case "2":
            habitService.ViewHabits();
            break;
        case "3":
            habitService.MarkHabitComplete();
            break;
        case "4":
            habitService.EditHabitName();
            break;
        case "5":
            habitService.DeleteHabit();
            break;
        case "6":
            running = false;
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }

    if (running)
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}