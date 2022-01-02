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
        private string _NameTask, _Reminder, _Date, _Priority,_NameList,_Description; 
        private int _Id;
        public TaskTodo()
        {        
          _NameTask = String.Empty;
          _Reminder  = String.Empty;
          _Date      = String.Empty;
          _Priority  = String.Empty;
          _NameList  = String.Empty;
         _Description = String.Empty;
        }
        public int TaskId { get { return _Id; }  set { _Id = value; NotifyPropertyChanged(TaskId.ToString()); } }
        public string NameTask { get { return _NameTask; } set { _NameTask = value; NotifyPropertyChanged(NameTask);  } }
        public string Reminder { get { return _Reminder; } set { _Reminder = value; NotifyPropertyChanged(Reminder); } }
        public string NameList {  get { return _NameList; } set  { _NameList = value; NotifyPropertyChanged(NameList); } }
        public string Date { get { return _Date; } set { _Date = value; NotifyPropertyChanged(Date); }  }
        public string Priority { get { return _Priority; } set { _Priority = value; NotifyPropertyChanged(Priority); } }
        public string Description { get { return _Description;  } set { _Description = value; NotifyPropertyChanged(Description); } } 

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));                     
        

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
        public void UpdateTask(int Id, string NameTask, string Date,string Reminder, string Priority, string NameList, string Description)
        {
           
            sqliteControler sqliteControler = new sqliteControler();
            sqliteControler.UpdateData(Id, NameTask, Date, Reminder, Priority, NameList, Description);
        }      
    }   
}


