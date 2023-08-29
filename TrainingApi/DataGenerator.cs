using System.Globalization;
using Microsoft.Data.Sqlite;
using TrainingApi.Shared;

public static class DataGenerator
{
    public static void InitializeDatabase()
    {
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS clients (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    firstName TEXT NOT NULL,
                    lastName TEXT NOT NULL,
                    email TEXT NOT NULL,
                    weight INTEGER NOT NULL,
                    height INTEGER NOT NULL,
                    birthDate DATETIME NOT NULL
                );

                CREATE TABLE IF NOT EXISTS trainers (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    firstName TEXT NOT NULL,
                    lastName TEXT NOT NULL,
                    email TEXT NOT NULL,
                    level TEXT NOT NULL,
                    isCertificationActive BOOLEAN NOT NULL
                );

                DELETE FROM clients;
                INSERT INTO clients
                VALUES (1, 'Vonnie', 'Mawer', 'vmawer0@go.com', 149, 66, '4-24-2000'),
                        (2, 'Langston', 'Feldberg', 'lfeldberg1@hc360.com', 329, 73, '10-20-1982'),
                        (3, 'Olwen', 'Maeer', 'omaeer3@purevolume.com', 261, 70, '8-22-1993');

                DELETE FROM trainers;
                INSERT INTO trainers
                VALUES (1, 'Inna', 'Spedroni', 'ispedroni0@studiopress.com', 'junior', true),
                        (2, 'Nikoletta', 'Orrell', 'norrell1@nydailynews.com', 'senior', true),
                        (3, 'Briana', 'Diprose', 'bdiprose0@t.co', 'senior', true),
                        (4, 'Zerk', 'Riepl', 'svanshin5@google.com', 'elite', true);
            ";
            command.ExecuteNonQuery();
        }
    }
}