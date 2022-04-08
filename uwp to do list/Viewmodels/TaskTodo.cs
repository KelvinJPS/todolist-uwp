using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Windows.Storage;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

namespace uwp_to_do_list
{
    public class TaskTodo : INotifyPropertyChanged
    {
        private string _NameTask, _Priority, _NameList, _Description, _Done;
        private int _Id, _ParentTask;

        private DateTimeOffset _NextRep, _Date, _Reminder;
        public TaskTodo()
        {
            _NameTask = String.Empty;
            _Reminder = default;
            _Date = default;
            _Priority = String.Empty;
            _NameList = String.Empty;
            _Description = String.Empty;
            _ParentTask = -1;
            _Id = 1;
            _Done = string.Empty;
        }

        Resource_Manager rm = new Resource_Manager();
        public event PropertyChangedEventHandler PropertyChanged;
        TaskSqliteDataAccess TaskSqlite = new TaskSqliteDataAccess();

        private void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int TaskId { get { return _Id; } set { _Id = value; NotifyPropertyChanged(TaskId.ToString()); } }
        public string NameTask { get { return _NameTask; } set { _NameTask = value; NotifyPropertyChanged(nameof(NameTask)); } }
        public DateTimeOffset Reminder
        {
            get { return _Reminder; }
            set
            {
                _Reminder = value;
                NotifyPropertyChanged(nameof(FormatDateReminder));
                NotifyPropertyChanged(nameof(FormatTimeReminder));

            }
        }
        public string ListName
        {
            get
            {
                if (_NameList == string.Empty || _NameList == null)
                {
                    return rm.GetDefaultNamelist;
                }
                return _NameList;

            }
            set { _NameList = value; NotifyPropertyChanged(nameof(ListName)); }
        }


        public DateTimeOffset Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                NotifyPropertyChanged(nameof(FormatDate));
            }
        }
        public string FormatDate
        {
            get
            {
                if (Date == default)
                    return rm.GetDefaultDate;

                return String.Format(" {0},{1} {2}", Date.DayOfWeek, Date.Day, Date.ToString("MMM"));
            }



        }
        public string Priority { get { return _Priority; } set { _Priority = value; NotifyPropertyChanged(nameof(Priority)); } }
        public string Description { get { return _Description; } set { _Description = value; NotifyPropertyChanged(nameof(Description)); } }
        public DateTimeOffset NextRep { get { return _NextRep; } set { _NextRep = value; NotifyPropertyChanged(nameof(NextRep)); } }
        public int ParentTask { get { return _ParentTask; } set { _ParentTask = value; NotifyPropertyChanged(nameof(TaskId)); } }
        public string FormatTimeReminder
        {
            get
            {
                if (Reminder == default)
                    return rm.GetDefaultReminder;

                return string.Format(" Remind me at {0}:{1}", Reminder.Hour, Reminder.Minute);
                ;

            }
        }
        public string FormatDateReminder
        {
            get => string.Format(" {0},{1} {2}", Date.DayOfWeek, Date.Day, Date.ToString("MMM"));

        }
        public string Done
        {
            get { return _Done; }
            set
            {
                _Done = value;
                NotifyPropertyChanged(nameof(Done));
            }
        }
        public ObservableCollection<TaskTodo> TasksDone
        {
            get { return TaskSqlite.GetTasksDone(); }

            set { NotifyPropertyChanged(nameof(TasksDone)); }

        }

        public ObservableCollection<TaskTodo> GetTasks(string ListSelected) => TaskSqlite.GetTasksDB(ListSelected);



        public void AddTask(TaskTodo task) => TaskSqlite.AddTaskDB(task);

        public void UpdateTask() => TaskSqlite.UpdateData(TaskId, NameTask, Date, Reminder, Priority, ListName, Description, NextRep, Done);

        public ObservableCollection<TaskTodo> GetSubtasks(TaskTodo taskTodo) => TaskSqlite.GetSubtasks(taskTodo);

        public ObservableCollection<TaskTodo> GetTodayTasks()
        {
          ObservableCollection<TaskTodo> Tasks =   TaskSqlite.GetAllTasksDB();
          ObservableCollection<TaskTodo> tasks = new ObservableCollection<TaskTodo>();
          
            if(Tasks.Count > 0)
            {
                foreach (TaskTodo task in Tasks)
                {
                    if (task.Date.ToString("d") == DateTimeOffset.Now.ToString("d"))
                    {
                        tasks.Add(task);                      
                    }
                }
            }
            return tasks;
        }

        public ObservableCollection<TaskTodo> GetTomorrowTasks()
        {
            ObservableCollection<TaskTodo> Tasks = TaskSqlite.GetAllTasksDB();
            ObservableCollection<TaskTodo> TomorrowTasks = new ObservableCollection<TaskTodo>();
            
            if(Tasks.Count > 0)
            {
                foreach (TaskTodo task in Tasks)
                {
                    if (task.Date.ToString("d") == DateTimeOffset.Now.AddDays(1).ToString("d"))
                    {
                        TomorrowTasks.Add(task);
                    }

                }
            }
           
            return TomorrowTasks;
        }
    }
    }   


