using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using uwp_to_do_list.model;

namespace uwp_to_do_list.view_model
{
    public class Task_Todo : INotifyPropertyChanged
    {
        public string Name_Task { get; set; }
        public DateTime Reminder { get; set; }
        public string name_list { get; set; }
        public DateTime date { get; set; }
        public string subtask { get; set; }
        public string priority { get; set; }

        public ObservableCollection<Task_Todo> Tasks = new ObservableCollection<Task_Todo>();
        public MainModel model = new MainModel(); 
        
      
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}


