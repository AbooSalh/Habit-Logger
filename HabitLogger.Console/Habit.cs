using System;

public record Habit(int Id, string Name);
public record Occurrence(int Id, int HabitId, DateTime Date, int Quantity);
