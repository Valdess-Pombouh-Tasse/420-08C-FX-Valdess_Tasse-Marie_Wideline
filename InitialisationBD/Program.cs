using System;
using System.IO;
using MySql.Data.MySqlClient;

class Program
{
    static void Main(string[] args)
    {
        string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        try
        {
            string sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gestioncomptesutilisateurs.sql");
            string script = File.ReadAllText(sqlFilePath);

            string connectionString = "server=localhost;user=root;password='';";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            File.AppendAllText(logPath, $"[{DateTime.Now}] Base de données créée avec succès.\n");
        }
        catch (Exception ex)
        {
            File.AppendAllText(logPath, $"[{DateTime.Now}] ERREUR : {ex.Message}\n{ex.StackTrace}\n");
        }
    }
}