using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace uwp_to_do_list
{
    public class sqliteControler
    {
        public async void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("TasksSqlite.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS Task (Primary_Key INTEGER PRIMARY KEY, " +
                    "Name_task NVARCHAR(50) NULL,due_date date, reminder datetime, priority NVARCHAR(10), list NVARCHAR(50),description text)";


                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
                
            }
        }

        public static void AddData(TaskTodo Task)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Task VALUES (NULL, @NameTask,@DueDate,@Reminder,@Priority,@List,@description);";
                insertCommand.Parameters.AddWithValue("@NameTask", Task.NameTask);
                insertCommand.Parameters.AddWithValue("@DueDate", Task.Date);
                insertCommand.Parameters.AddWithValue("@Reminder", Task.Reminder);
                insertCommand.Parameters.AddWithValue("@Priority", Task.Priority);
                insertCommand.Parameters.AddWithValue("@List", Task.NameList);
                insertCommand.Parameters.AddWithValue("@description", Task.Description);
                insertCommand.ExecuteReader();
                db.Close();
            }

        }


        public void UpdateData(int Id, string NameTask, string Date, string Reminder, string Priority, string NameList, string Description)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand updateCommand = new SqliteCommand();
                updateCommand.Connection = db;
                try
                {                   
                    updateCommand.CommandText = "update Task set Name_task = @NameTask,  due_date = @DueDate, reminder = @Reminder, priority = @Priority, list = @List, description = @Description" +
                        "where Primary_Key = @ID ";
                    updateCommand.Parameters.AddWithValue("@DueDate", Date);
                    updateCommand.Parameters.AddWithValue("@ID", Id);
                    updateCommand.Parameters.AddWithValue("@NameTask", NameTask);
                    updateCommand.Parameters.AddWithValue("@Reminder", Reminder);
                    updateCommand.Parameters.AddWithValue("@Priority", Priority);
                    updateCommand.Parameters.AddWithValue("@List", NameList);
                    updateCommand.Parameters.AddWithValue("@Description",Description);
                    updateCommand.ExecuteReader();
                    db.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
               


            }

        }

    }
}


