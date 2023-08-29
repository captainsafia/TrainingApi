using TrainingApi.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using 
Microsoft.Data.Sqlite;

public class TrainingService
{
    public Results<Ok<Client>, NotFound> GetClientById(int id)
    {
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM clients
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    var weight = reader.GetInt32(4);
                    var height = reader.GetInt32(5);
                    var birthDate = reader.GetDateTime(6);
                    var client = new Client(id, firstName, lastName, email, weight, height, birthDate);
                    return TypedResults.Ok(client);
                }
            }
        }
        return TypedResults.NotFound();
    }

    public Created<Client> UpdateClientById(int id, Client updatedClient)
    {
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE clients
                SET firstName = $firstName,
                lastName = $lastName,
                email = $email,
                weight = $weight,
                height = $height,
                birthDate = $birthDate
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", updatedClient.Id);
            command.Parameters.AddWithValue("$firstName", updatedClient.FirstName);
            command.Parameters.AddWithValue("$lastName", updatedClient.LastName);
            command.Parameters.AddWithValue("$email", updatedClient.Email);
            command.Parameters.AddWithValue("$weight", updatedClient.Weight);
            command.Parameters.AddWithValue("$height", updatedClient.Height);
            command.Parameters.AddWithValue("$birthDate", updatedClient.BirthDate);

            command.ExecuteNonQuery();

            return TypedResults.Created($"/clients/{updatedClient.Id}", updatedClient);
        }
    }

    public Created<Trainer> UpdateTrainerById(int id, Trainer updatedTrainer)
    {
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE trainers
                SET firstName = $firstName,
                lastName = $lastName,
                email = $email,
                level = $level,
                isCertificationActive = $isCertificationActive
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", updatedTrainer.Id);
            command.Parameters.AddWithValue("$firstName", updatedTrainer.FirstName);
            command.Parameters.AddWithValue("$lastName", updatedTrainer.LastName);
            command.Parameters.AddWithValue("$email", updatedTrainer.Email);
            command.Parameters.AddWithValue("$level", updatedTrainer.Level);
            command.Parameters.AddWithValue("$isCertificationActive", updatedTrainer.IsCertificationActive);

            command.ExecuteNonQuery();

            return TypedResults.Created($"/trainers/{updatedTrainer.Id}", updatedTrainer);
        }
    }

    public Results<Created<Trainer>, NotFound> CreateTrainer(Trainer trainer)
    {
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO trainers
                VALUES ($id, $firstName, $lastName, $email, $level, $isCertificationActive)
            ";
            command.Parameters.AddWithValue("$id", trainer.Id);
            command.Parameters.AddWithValue("$firstName", trainer.FirstName);
            command.Parameters.AddWithValue("$lastName", trainer.LastName);
            command.Parameters.AddWithValue("$email", trainer.Email);
            command.Parameters.AddWithValue("$level", trainer.Level);
            command.Parameters.AddWithValue("$isCertificationActive", trainer.IsCertificationActive);

            command.ExecuteNonQuery();

            return TypedResults.Created($"/trainers/{trainer.Id}", trainer);
        }
    }

    public Ok<List<Trainer>> GetTrainers()
    {
        List<Trainer> trainers = new();
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM trainers
            ";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    Enum.TryParse(reader.GetString(4), true, out Level level);
                    var isCertificationActive = reader.GetBoolean(5);
                    trainers.Add(new Trainer(id, firstName, lastName, email, level, isCertificationActive));
                }
            }
        }
        return TypedResults.Ok(trainers);
    }

    public NoContent DeleteTrainerById(int id)
    {
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM trainers
                WHERE id = $id
            ";

            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();

            return TypedResults.NoContent();
        }
    }

    public Results<Ok<Trainer>, NotFound> GetTrainerById(int id)
    {
        using (var connection = new SqliteConnection("Data Source=trainingapi.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM trainers
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    Enum.TryParse(reader.GetString(4), true, out Level level);
                    var isCertificationActive = reader.GetBoolean(5);
                    var trainer = new Trainer(id, firstName, lastName, email, level, isCertificationActive);
                    return TypedResults.Ok(trainer);
                }
            }
        }
        return TypedResults.NotFound();
    }
}