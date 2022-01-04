using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library

namespace uwp_to_do_list
{
    public sealed partial class MainView : Page
    {
        ObservableCollection<TaskTodo> Tasks = new ObservableCollection<TaskTodo>();
        TaskTodo task = new TaskTodo();
        Func<DateTimeOffset, string> SetDate = (date) => string.Format("{0}-{1}-{2}", date.Day, date.Month, date.Year);

        public MainView()
        {
            this.InitializeComponent();

            Tasks = task.GetTasks();
            task_list.ItemsSource = Tasks;


        }
        private void add_Task_textbox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                //add task to de database and the observable collection
                task.NameTask = add_Task_textbox.Text;
                task.AddTask(task);
                Tasks.Add(task);

                //show the task data 
                TaskForm.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //clean the texbox and focus
                add_Task_textbox.Text = String.Empty;
                add_Task_textbox.Focus(Windows.UI.Xaml.FocusState.Keyboard);

            }

        }
        private void add_Task_textbox_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e) => add_Task_textbox.Text = string.Empty;

        private void task_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            task = task_list.SelectedItem as TaskTodo;
            TaskForm.Visibility = Windows.UI.Xaml.Visibility.Visible;
            NameTask_form.Text = task.NameTask;

        }

        private void calendar_button(object sender, Windows.UI.Xaml.RoutedEventArgs e) => calendar_popup.IsOpen = true;

        private void quit_TaskForm_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => TaskForm.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        private void cancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            calendar_popup.IsOpen = false;
            calendar_date.SelectedDates.Clear();
        }

        private void save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            calendar_popup.IsOpen = false;
            //if the date is not selected set the actual date
            if (calendar_date.SelectedDates.Count > 0) task.Date = SetDate(calendar_date.SelectedDates[0]);
            else task.Date = SetDate(DateTimeOffset.Now);

            task.UpdateTask(task.TaskId, task.NameTask, task.Date, task.Reminder, task.Priority, task.NameList, task.Description);
            calendar_date.SelectedDates.Clear();
        }
        private void reminder_cancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => reminder_popup.IsOpen = false;

        private void reminder_save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        { 
            reminder_popup.IsOpen = false;
            DateTimeOffset Date = reminder_calendar.SelectedDates[0];

            //get the date and time from the control 
            if (reminder_time_picker.SelectedTime.HasValue == true)
            {
                var Time = reminder_time_picker.SelectedTime;
                Date = Date - ((TimeSpan)Time) + ((TimeSpan)Time);      
            }
            
            SheduleNotification(Date);
            task.Reminder = string.Format("{0}-{1}-{2} {3}:{4}:{5}", Date.Day, Date.Month, Date.Year, Date.Hour, Date.Minute, Date.Second);
            //update in DB
            task.UpdateTask(task.TaskId, task.NameTask, task.Date, task.Reminder, task.Priority, task.NameList, task.Description);
            //clear values
            reminder_calendar.SelectedDates.Clear();
            reminder_time_picker.SelectedTime = null;
        }
        private void priority_checkbox_click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            task.Priority = radioButton.Name;
            task.UpdateTask(task.TaskId, task.NameTask, task.Date, task.Reminder, task.Priority, task.NameList, task.Description);

        }
        private void descriptions_textbox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                task.Description = descriptions_textbox.Text;
                task.UpdateTask(task.TaskId, task.NameTask, task.Date, task.Reminder, task.Priority, task.NameList, task.Description);
                descriptions_textbox.Text = string.Empty;
            }
        }
        private void reminder_button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            reminder_popup.IsOpen = true;
            reminder_calendar.SelectedDates.Add(DateTimeOffset.Now);
        }


        private void SheduleNotification(DateTimeOffset Date)
        {
            // sheduled notification 
            try
            {
                ToastContentBuilder N = new ToastContentBuilder();
                N.AddText(String.Format("you have to do {0}", task.NameTask));
                N.AddText(String.Format("{0}", task.NameTask));
                N.Schedule(Date.DateTime.AddSeconds(10));
            }
            catch (Exception ex) { ex.Message.ToString(); }


        }


    }

}