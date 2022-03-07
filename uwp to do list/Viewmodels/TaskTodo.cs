using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Windows.Storage;
using System.Diagnostics;
using System.Globalization;


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
          _Date =       String.Empty;
          _Priority  =  String.Empty;
          _NameList  =  String.Empty;
          _Description = String.Empty;
         _ParentTask =  -1;
         _Id = 1;
        }
  
        Resource_Manager rm = new Resource_Manager();
        public event PropertyChangedEventHandler PropertyChanged;
        TaskSqliteDataAccess TaskSqlite = new TaskSqliteDataAccess();

        private void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int TaskId { get { return _Id; }  set { _Id = value; NotifyPropertyChanged(TaskId.ToString()); } }
        public string NameTask { get { return _NameTask; } set { _NameTask = value; NotifyPropertyChanged(nameof(NameTask)); } }
        public string Reminder { get { return _Reminder; } set { _Reminder = value; 
                NotifyPropertyChanged(nameof(FormatDateReminder));
                NotifyPropertyChanged(nameof(FormatTimeReminder));
            
            } }
        public string ListName {  
            get 
            {
                if ( _NameList == string.Empty || _NameList == null)
                {
                    return rm.GetDefaultNamelist;
                }
                return _NameList;

                }
            set { _NameList = value; NotifyPropertyChanged(nameof(ListName)); }
        }
   
           
        public string Date { get { return _Date ; } set { _Date = value; NotifyPropertyChanged(nameof(FormatDate)); } }
        public string FormatDate
        {
            get
            {
                var cultureInfo = new CultureInfo("en-US");
                DateTimeOffset DueDate;
                
                if (Date != string.Empty && Date != null)
                {
                    DueDate = DateTimeOffset.Parse(Date, cultureInfo);
                    return String.Format(" {0},{1} {2}", DueDate.DayOfWeek, DueDate.Day, DueDate.ToString("MMM"));
                }
                else                   
                    return rm.GetDefaultDate;             
            }
             
    }
        public string Priority { get { return _Priority; } set { _Priority = value; NotifyPropertyChanged(nameof(Priority)); } }
        public string Description { get { return _Description;  } set { _Description = value; NotifyPropertyChanged(nameof(Description)); } } 
        public DateTimeOffset NextRep { get { return _NextRep; } set { _NextRep= value; NotifyPropertyChanged(nameof(NextRep)); } }      
        public int  ParentTask { get { return _ParentTask; } set { _ParentTask = value; NotifyPropertyChanged(nameof(TaskId)); } }      
        public string FormatTimeReminder
        {
            get {
                DateTime Time;
                var cultureInfo = new CultureInfo("en-US");
                if ( Reminder != string.Empty && Reminder !=  null )
                {
                    Time = DateTime.Parse (Reminder, cultureInfo);
                    return string.Format(" Remind me at {0}:{1}", Time.Hour, Time.Minute);
                }
                              
                return rm.GetDefaultReminder;
                
            } 
        }
        public string FormatDateReminder
        {
            get
            {
                DateTimeOffset Date;

                if(DateTimeOffset.TryParse(_Reminder, out Date) == true)
                {
                    return string.Format(" {0},{1} {2}", Date.DayOfWeek, Date.Day, Date.ToString("MMM"));

                }

                return Reminder;

            }
        }
        public ObservableCollection<TaskTodo> GetTasks( string ListSelected) => TaskSqlite.GetTaskDB(ListSelected);
     
        public void AddTask(TaskTodo task) => TaskSqlite.AddTaskDB(task); 
   
        public void UpdateTask() => TaskSqlite.UpdateData(TaskId, NameTask, Date, Reminder, Priority, ListName, Description, NextRep);
      
        public ObservableCollection<TaskTodo> GetSubtasks(TaskTodo taskTodo) => TaskSqlite.GetSubtasks(taskTodo);
    }   
}


