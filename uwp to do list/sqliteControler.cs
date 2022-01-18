using Microsoft.Data.Sqlite;
using System;
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
                    "Name_task NVARCHAR(50) NULL,due_date NVARCHAR(10), reminder datetime, priority NVARCHAR(10), list NVARCHAR(50),description text)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();

            }
        }

        public static void AddData(TaskTodo Task)
        {
            try
            {

                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO Task VALUES (NULL, @NameTask,@DueDate,@Reminder,@Priority,@List,@description,@NextRep,@ParentTask);";
                    insertCommand.Parameters.AddWithValue("@NameTask", Task.NameTask);
                    insertCommand.Parameters.AddWithValue("@DueDate", Task.Date);
                    insertCommand.Parameters.AddWithValue("@Reminder", Task.Reminder);
                    insertCommand.Parameters.AddWithValue("@Priority", Task.Priority);
                    insertCommand.Parameters.AddWithValue("@List", Task.NameList);
                    insertCommand.Parameters.AddWithValue("@description", Task.Description);
                    insertCommand.Parameters.AddWithValue("@NextRep", Task.NextRep);
                    insertCommand.Parameters.AddWithValue("@ParentTask", Task.ParentTask);
                    insertCommand.ExecuteReader();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        public void UpdateData(int Id, string NameTask, string Date, string Reminder, string Priority, string NameList, string Description,DateTimeOffset NextRep)
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
                    updateCommand.CommandText = "update Task set Name_task = @NameTask, due_date = @DueDate, reminder = @Reminder, priority = @Priority, list = @List, description = @Description, next_rep = @NextRep" +
                        " where Primary_Key = @ID";
                    updateCommand.Parameters.AddWithValue("@DueDate", Date);
                    updateCommand.Parameters.AddWithValue("@ID", Id);
                    updateCommand.Parameters.AddWithValue("@NameTask", NameTask);
                    updateCommand.Parameters.AddWithValue("@Reminder", Reminder);
                    updateCommand.Parameters.AddWithValue("@Priority", Priority);
                    updateCommand.Parameters.AddWithValue("@List", NameList);
                    updateCommand.Parameters.AddWithValue("@Description", Description);
                    updateCommand.Parameters.AddWithValue("@NextRep", NextRep);
                    updateCommand.ExecuteReader();
                    db.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }



            }

        }

    }
}



