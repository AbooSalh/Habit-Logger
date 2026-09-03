using Microsoft.Data.Sqlite;

namespace HabitLogger.Console
{
    internal class HabitsRepo
    {
        const string connectionString = "Data Source=Habits.db";
        public HabitsRepo()
        {
            InitializeDatabase();
        }
        private static void InitializeDatabase()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            // if there is no table called Habits, create it
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Habit (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
                );";
            command.ExecuteNonQuery();
            // if there is no table called Occurrences, create it
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Occurrence (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                HabitId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                Quantity INTEGER NOT NULL,
                FOREIGN KEY (HabitId) REFERENCES Habit(Id)
                );";
            command.ExecuteNonQuery();
        }

        public int AddHabit(string name)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Habit (Name) VALUES (@name);";
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
            // return habit id of the newly added habit
            command.CommandText = "SELECT last_insert_rowid();";
            return Convert.ToInt32(command.ExecuteScalar());
        }
        public void RemoveHabit(string name)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            // 1. Add the parameter ONCE. It can be reused in multiple queries on this command object.
            command.Parameters.AddWithValue("@name", name);

            // 2. ALWAYS delete child rows (Occurrence) BEFORE parent rows (Habit)
            command.CommandText = "DELETE FROM Occurrence WHERE HabitId = (SELECT Id FROM Habit WHERE Name = @name);";

            // ExecuteNonQuery returns the number of rows changed/deleted
            int occurrencesDeleted = command.ExecuteNonQuery();

            if (occurrencesDeleted == 0)
            {
                System.Console.WriteLine($"[INFO]: No occurrences found to delete for habit '{name}'.");
            }
            else
            {
                System.Console.WriteLine($"[SUCCESS]: Deleted {occurrencesDeleted} occurrence(s) for habit '{name}'.");
            }

            // 3. Now it is safe to delete the parent habit
            command.CommandText = "DELETE FROM Habit WHERE Name = @name;";
            int habitsDeleted = command.ExecuteNonQuery();

            if (habitsDeleted == 0)
            {
                System.Console.WriteLine($"[WARNING]: Habit '{name}' did not exist in the database.");
            }
            else
            {
                System.Console.WriteLine($"[SUCCESS]: Habit '{name}' successfully removed.");
            }
        }

        public void AddOccurrence(int habitId, DateTime date, int quantity)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Occurrence (HabitId, Date, Quantity) VALUES (@habitId, @date, @quantity);";
            command.Parameters.AddWithValue("@habitId", habitId);
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@quantity", quantity);
            command.ExecuteNonQuery();
        }

        public void UpdateHabit (int habitId , string name)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Habit SET Name = @name WHERE Id = @habitId;";
            command.Parameters.AddWithValue("@habitId", habitId);
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();
        }

        public void DisplayHabitsWithOccurrences()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT h.Id, h.Name, COUNT(o.Id) AS OccurrenceCount
                FROM Habit h
                LEFT JOIN Occurrence o ON h.Id = o.HabitId
                GROUP BY h.Id, h.Name;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int habitId = reader.GetInt32(0);
                string habitName = reader.GetString(1);
                int occurrenceCount = reader.GetInt32(2);
                System.Console.WriteLine($"Habit ID: {habitId}, Name: {habitName}, Occurrences: {occurrenceCount}");
            }
        }
    }
}
