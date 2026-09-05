using HabitLogger.Console;
using Microsoft.Data.Sqlite;
var habitsRepo = new HabitsRepo();
bool finished = false;
while (!finished)
{
    MainMenu();
    Console.Clear();
}


void MainMenu()
{
    Console.WriteLine("1. Add Habit");
    Console.WriteLine("2. Add Occurrence");
    Console.WriteLine("3. View Habits");
    Console.WriteLine("4. View Occurrences of a Habit");
    Console.WriteLine("5. Exit");
    Console.Write("Choose an option: ");
    
    switch (Console.ReadLine())
    {
        case "1":
            AddHabit();
            break;
        case "2":
            AddOccurrence();
            break;
        case "3":
            ViewHabits();
            break;
        case "4":
            ViewOccurrences();
            break;
        case "5":
            Exit();
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
    if (!finished)
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(intercept: true);
    }
}

void AddHabit()
{
    Console.Write("Enter habit name: ");
    string name = Console.ReadLine();
    int habitId = habitsRepo.AddHabit(name);
    Console.WriteLine($"Habit '{name}' added with ID {habitId}.");
}
void AddOccurrence()
{
    // display all habits with their ids
    habitsRepo.DisplayHabitsWithOccurrences();
    Console.Write("Enter habit id: ");
    int habitId = int.Parse(Console.ReadLine());
    Console.Write("Enter date (yyyy-MM-dd): ");
    string dateInput = Console.ReadLine();
    DateTime date;
    if (!DateTime.TryParse(dateInput, out date))
    {
        Console.WriteLine("Invalid date format.");
        return;
    }
    Console.Write("Enter quantity: ");
    int quantity;
    if (!int.TryParse(Console.ReadLine(), out quantity))
    {
        Console.WriteLine("Invalid quantity.");
        return;
    }
    habitsRepo.AddOccurrence(habitId, date, quantity);
    Console.WriteLine($"Occurrence added for habit with ID {habitId} on {date.ToShortDateString()} with quantity {quantity}.");
}
void ViewHabits()
{
    habitsRepo.DisplayHabitsWithOccurrences();
}
void ViewOccurrences()
{
    // display all habits with their ids
    habitsRepo.DisplayHabitsWithOccurrences();
    Console.Write("Enter habit id to view occurrences: ");
    int habitId = int.Parse(Console.ReadLine());
    habitsRepo.DisplayHabitOccurrences(habitId);
}
void Exit()
{
    finished = true;
    Console.WriteLine("Exiting...");
}
