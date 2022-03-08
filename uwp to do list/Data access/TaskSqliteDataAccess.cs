using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace uwp_to_do_list
{
    public class TaskSqliteDataAccess
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
                    "Name_task NVARCHAR(100) ,due_date NVARCHAR(10), reminder datetime, priority NVARCHAR(10), list NVARCHAR(50),description text, parent_task int, Next_rep datetime)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            
            }

        }

        public  void AddTaskDB(TaskTodo Task)
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
                    insertCommand.CommandText = "INSERT INTO Task VALUES (NULL, @NameTask,@DueDate,@Reminder,@Priority,@List,@description,@ParentTask,@NextRep);";
                    insertCommand.Parameters.AddWithValue("@NameTask", Task.NameTask);
                    insertCommand.Parameters.AddWithValue("@DueDate", Task.Date);
                    insertCommand.Parameters.AddWithValue("@Reminder", Task.Reminder);
                    insertCommand.Parameters.AddWithValue("@Priority", Task.Priority);
                    insertCommand.Parameters.AddWithValue("@List", Task.ListName);
                    insertCommand.Parameters.AddWithValue("@description", Task.Description);
                    insertCommand.Parameters.AddWithValue("@ParentTask", Task.ParentTask);
                    insertCommand.Parameters.AddWithValue("@NextRep", Task.NextRep);
                    insertCommand.ExecuteReader();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        public void UpdateData(int Id, string NameTask, DateTimeOffset Date, DateTimeOffset Reminder, string Priority, string NameList, string Description,DateTimeOffset NextRep)
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

        public ObservableCollection<TaskTodo> GetTaskDB(string ListName )
        {
            var Tasks = new ObservableCollection<TaskTodo>();
            //get data from sqlite3
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from Task where list  = @ListName", db);

                selectCommand.Parameters.AddWithValue("ListName", ListName);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    try
                    {
                        TaskTodo taskTodo = new TaskTodo();
                        taskTodo.TaskId =   query.GetInt32(0);
                        taskTodo.NameTask = query.GetString(1);
                        taskTodo.Date =     query.GetDateTimeOffset(2);
                        taskTodo.Reminder = query.GetDateTimeOffset(3);
                        taskTodo.Priority = query.GetString(4);
                        taskTodo.ListName = query.GetString(5);
                        taskTodo.Description = query.GetString(6);

                        Tasks.Add(taskTodo);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                db.Close();
            }
            return Tasks;

        }


        public ObservableCollection<TaskTodo> GetSubtasks(TaskTodo taskTodo)
        {

            {
                var SubTasks = new ObservableCollection<TaskTodo>();
  
                //get data from sqlite3

                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
                using (SqliteConnection db =
                   new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand
                        ("SELECT * from Task where parent_task = @Id", db);

                    selectCommand.Parameters.AddWithValue("@Id", taskTodo.TaskId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        try
                        {
                       
                            taskTodo.TaskId   = query.GetInt32(0);
                            taskTodo.NameTask = query.GetString(1);
                            taskTodo.Date     = query.GetDateTimeOffset(2);
                            taskTodo.Reminder = query.GetDateTimeOffset(3);
                            taskTodo.Priority = query.GetString(4);
                            taskTodo.ListName = query.GetString(5);
                            taskTodo.Description = query.GetString(6);

                            SubTasks.Add(taskTodo);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    db.Close();
                }
                return SubTasks;
            }
        }



    } }
    




