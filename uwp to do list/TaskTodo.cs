using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Windows.Storage;

namespace uwp_to_do_list
{
    public class TaskTodo : INotifyPropertyChanged
    {
        public string NameTask { get; set; }
        public DateTime Reminder { get; set; }
        public string NameList { get; set; }
        public DateTime Date { get; set; }
        public string SubTask { get; set; }
        public string Priority { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    ("SELECT Name_task from Task", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                   TaskTodo taskTodo = new TaskTodo();
                    taskTodo.NameTask = query.GetString(0);
                    Tasks.Add(taskTodo);
                }

                db.Close();
            }


            return Tasks;
        }

       public void AddTask(string NameTask)
        {
            sqliteControler sqliteControler = new sqliteControler();
            sqliteControler.InitializeDatabase();
            sqliteControler.AddData(NameTask);
            
        }
        
    }
    
}


