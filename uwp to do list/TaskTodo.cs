using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Windows.Storage;
using System.Diagnostics;

namespace uwp_to_do_list
{
    public class TaskTodo : INotifyPropertyChanged
    {
        public TaskTodo()
        {
            
            NameTask = default;
            Reminder = String.Empty;
            Date     = String.Empty;
            Priority = String.Empty;
            NameList = String.Empty;
            Description = String.Empty;
        }

        public int TaskId { get  ; set; }
        public string NameTask { get; set; }
        public string Reminder { get; set; }
        public string NameList { get; set; }
        public string Date { get; set; }
        public string Priority { get; set; }

        public string Description { get; set; }
         
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            updateTask(TaskId, NameTask, Date, Reminder, Priority, NameList, Description);
        }


        public ObservableCollection<TaskTodo> GetTasks()
        {
            var Tasks = new ObservableCollection<TaskTodo>();
            //get data from sqlite3

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from Task", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    try
                    {
                        TaskTodo taskTodo = new TaskTodo();
                        taskTodo.TaskId   =  query.GetInt32(0);
                        taskTodo.NameTask =  query.GetString(1);
                        taskTodo.Date    =  query.GetString(2);
                        taskTodo.Reminder =  query.GetString(3);
                        taskTodo.Priority =  query.GetString(4);
                        taskTodo.NameList =  query.GetString(5);
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

     
       public void AddTask(TaskTodo task)
        {
            sqliteControler sqliteControler = new sqliteControler();
            sqliteControler.InitializeDatabase();
            sqliteControler.AddData(task);
                               
        }

        public void updateTask(int Id, string NameTask, string Date,string Reminder, string Priority, string NameList, string Description)
        {

            sqliteControler sqliteControler = new sqliteControler();
            sqliteControler.UpdateData(TaskId, NameTask, Date, Reminder, Priority, NameList, Description);
        }

       
        
    }
    
}


