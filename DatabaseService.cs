using Microsoft.Data.Sqlite;
using System.Data;


namespace PhotoApp
{
    public class DatabaseService
    {
        private string _databasePath;

        public DatabaseService(string databasePath)
        {
            _databasePath = databasePath;

            using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Photos (Id INTEGER PRIMARY KEY, FilePath TEXT)";
                command.ExecuteNonQuery();
            }
        }

        public List<Photo> GetPhotos()
        {
            using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Photos";

                using (var reader = command.ExecuteReader())
                {
                    return reader.Cast<IDataRecord>()
                                 .Select(r => new Photo
                                 {
                                     Id = r.GetInt32(0),
                                     FilePath = r.GetString(1)
                                 })
                                 .ToList();
                }
            }
        }

        public void InsertPhoto(Photo photo)
        {
            using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Photos (FilePath) VALUES (@filePath)";
                command.Parameters.AddWithValue("@filePath", photo.FilePath);

                command.ExecuteNonQuery();
            }
        }
    }
}
