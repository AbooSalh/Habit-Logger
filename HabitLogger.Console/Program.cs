using HabitLogger.Console;
using Microsoft.Data.Sqlite;
var habitsRepo = new HabitsRepo();
//var exerciseHabitId = habitsRepo.AddHabit("Exercise");
//habitsRepo.AddOccurrence(exerciseHabitId, DateTime.Now, 1);
habitsRepo.DisplayHabitsWithOccurrences();
//bool finished = false;
//while (!finished)
//{
//    MainMenu();
//}


//void MainMenu()
//{
//    Console.WriteLine("1. Add Habit");
//    Console.WriteLine("2. Add Occurrence");
//    Console.WriteLine("3. View Habits");
//    Console.WriteLine("4. View Occurrences of a Habit");
//    Console.WriteLine("5. Exit");

//}

