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
        private string _NameTask, _Reminder, _Date, _Priority, _NameList, _Description;
        private int _Id, _ParentTask;
        
        private DateTimeOffset _NextRep;
        public TaskTodo()
        {        
          _NameTask =   String.Empty;
          _Reminder  =  String.Empty;
          _Date      =  String.Empty;
          _Priority  =  String.Empty;
          _NameList  =  "Tasks";
            _Description = String.Empty;
         _ParentTask =  -1;
            _Id = 1;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int TaskId { get { return _Id; }  set { _Id = value; NotifyPropertyChanged(TaskId.ToString()); } }
        public string NameTask { get { return _NameTask; } set { _NameTask = value; NotifyPropertyChanged(NameTask); } }
        public string Reminder { get { return _Reminder; } set { _Reminder = value; NotifyPropertyChanged(FormatDateReminder); } }
        public string NameList {  get { return _NameList; } set  { _NameList = value; NotifyPropertyChanged(NameList); } }
        public string Date { get { return _Date ; } set { _Date = value; NotifyPropertyChanged(FormatDate); } }
        public string FormatDate
        {
            get
            {
                DateTimeOffset DueDate;
                if(DateTimeOffset.TryParse(_Date, out DueDate)==true) 
                {
                    return String.Format(" {0},{1} {2}", DueDate.DayOfWeek, DueDate.Day, DueDate.ToString("MMM"));
                }
                return Date;
            }
        }
        public string Priority { get { return _Priority; } set { _Priority = value; NotifyPropertyChanged(Priority); } }
        public string Description { get { return _Description;  } set { _Description = value; NotifyPropertyChanged(Description); } } 
        public DateTimeOffset NextRep { get { return _NextRep; } set { _NextRep= value; NotifyPropertyChanged(NextRep.ToString()); } }      
        public int  ParentTask { get { return _ParentTask; } set { _ParentTask = value; NotifyPropertyChanged(TaskId.ToString()); } }      
        public string FormatTimeReminder
        {
            get { DateTime Time;
                 if( DateTime.TryParse(_Reminder, out Time) == true)
                {
                    return string.Format(" Remind me at {0}:{1}", Time.Hour, Time.Minute);
                }

                return " Reminder";
            } 
        }

        public string FormatDateReminder
        {
            get
            {
                DateTimeOffset Date;

                DateTimeOffset.TryParse(_Reminder, out Date);
                
                return string.Format(" {0},{1} {2}", Date.DayOfWeek, Date.Day, Date.ToString("MMM"));
              
              
            }
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
                    ("SELECT * from Task where parent_task = -1", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    try
                    {
                        TaskTodo taskTodo = new TaskTodo();
                        taskTodo.TaskId   =  query.GetInt32(0);
                        taskTodo.NameTask =  query.GetString(1);
                        taskTodo.Date    =   query.GetString(2);
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
        public void UpdateTask()
        {
           
            sqliteControler sqliteControler = new sqliteControler();
            sqliteControler.UpdateData(TaskId, NameTask, Date, Reminder, Priority, NameList, Description, NextRep);
        }
  

        public ObservableCollection<TaskTodo> GetSubtasks()
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
                     
                    selectCommand.Parameters.AddWithValue("@Id",TaskId);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        try
                        {
                            TaskTodo taskTodo = new TaskTodo();
                            taskTodo.TaskId = query.GetInt32(0);
                            taskTodo.NameTask = query.GetString(1);
                            taskTodo.Date = query.GetString(2);
                            taskTodo.Reminder = query.GetString(3);
                            taskTodo.Priority = query.GetString(4);
                            taskTodo.NameList = query.GetString(5);
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
    }   
}


