using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Windows.Storage;

namespace uwp_to_do_list
{
    public class  sqliteControler
    {
        public async  void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("TasksSqlite.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS Task (Primary_Key INTEGER PRIMARY KEY, " +
                    "Name_task NVARCHAR(50) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Task VALUES (NULL, @NameTask);";
                insertCommand.Parameters.AddWithValue("@NameTask", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }



    }
}


