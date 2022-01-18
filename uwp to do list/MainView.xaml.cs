using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Windows.UI.Xaml.Media;

namespace uwp_to_do_list
{
    public sealed partial class MainView : Page
    {
        ObservableCollection<TaskTodo> Tasks = new ObservableCollection<TaskTodo>();
        ObservableCollection<TaskTodo> SubTasks = new ObservableCollection<TaskTodo>();
        TaskTodo task = new TaskTodo();
        Func<DateTimeOffset, string> SetDate = (date) => string.Format("{0}-{1}-{2}", date.Day, date.Month, date.Year);
      
        
        public MainView()
        {
            this.InitializeComponent();

            Tasks = task.GetTasks();
            task_list.ItemsSource = Tasks;
            subtask_list.ItemsSource = SubTasks;
            number_repeat.MaxLength = 3;  

       
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

                task_list.SelectedItem = task;
            }

        }
        private void add_Task_textbox_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e) => add_Task_textbox.Text = string.Empty;

        private void task_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            task = task_list.SelectedItem as TaskTodo;
            TaskForm.Visibility = Windows.UI.Xaml.Visibility.Visible;
            NameTask_form.Text = task.NameTask;
        }

        private void calendar_button(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            calendar_popup.IsOpen = true;
            calendar_date.SelectedDates.Add(DateTimeOffset.Now);
        }
          
        private void quit_TaskForm_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => TaskForm.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        private void cancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            calendar_popup.IsOpen = false;
            calendar_date.SelectedDates.Clear();
        }

        private void save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            calendar_popup.IsOpen = false;
            task.Date = SetDate(calendar_date.SelectedDates[0]);

            task.UpdateTask();
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
            task.UpdateTask();
            //clear values
            reminder_calendar.SelectedDates.Clear();
            reminder_time_picker.SelectedTime = null;
        }
        private void priority_checkbox_click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            task.Priority = radioButton.Name;
            task.UpdateTask();

        }
        private void descriptions_textbox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                task.Description = descriptions_textbox.Text;
                task.UpdateTask();
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

        private void list_name_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                task.NameList = list_name.Text;
                task.UpdateTask();

                this.Focus(Windows.UI.Xaml.FocusState.Pointer);
            }
        }

        private void repeat_button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (repeat_options_popup.IsOpen == true)
                repeat_options_popup.IsOpen = false;

            else
                repeat_options_popup.IsOpen = true;

        }

        private void number_repeat_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            // only numbers in textbox
            args.Cancel = args.NewText.Any(c => !char.IsNumber(c));         
        }

        private void cancel_repeat_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => custom_repeat_popup.IsOpen = false;

        private void save_repeat_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            custom_repeat_popup.IsOpen = false; 
              
           
        }

        private void custom_repeat_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {  
            ComboBox ComboBox = sender as ComboBox;
            ComboBoxItem ComboBoxSelected = ComboBox.SelectedItem as ComboBoxItem;
           
            if (ComboBoxSelected != null)
            {
                custom_month_calendar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                week_calendar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                switch (ComboBoxSelected.Name)
                {
                    case "MonthsComboBox":
                       
                        custom_month_calendar.Visibility = Windows.UI.Xaml.Visibility.Visible;

                        break;

                    case "DaysComboBox":
                        
                        break ;

                    case "WeeksComboBox":
            
                        week_calendar.Visibility = Windows.UI.Xaml.Visibility.Visible;

                        break;
                }
            }
        }
        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void SubTask_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
              TaskTodo SubTask = new TaskTodo();
              SubTask.NameTask = SubTask_TexBox.Text;
              Debug.WriteLine(task.NameTask);
              Debug.WriteLine("sub");
              SubTask.ParentTask = task.TaskId; //the value of the task selected
              SubTasks.Add(SubTask);
              SubTask.AddTask(SubTask);
            }
        }

        public bool CheckTaskRepeat(DateTimeOffset date) =>  date==DateTime.Today;

        
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock TextBlockSelected = Repeat_options.SelectedItem  as TextBlock;
    
            if (TextBlockSelected != null)
            {
                switch (TextBlockSelected.Name)
                {
                    case "Custom":

                        repeat_options_popup.IsOpen = false;
                        custom_repeat_popup.IsOpen = true;

                        break;

                    case "Daily":

                        task.NextRep = DateTime.Today.AddDays(1);
                        task.UpdateTask();
                        break;


                    case "Weekly":

                        task.NextRep = DateTime.Today.AddDays(7);
                        task.UpdateTask();
                        break;


                    case "Montly":

                        task.NextRep = DateTime.Today.AddMonths(1);
                        task.UpdateTask();
                       
                        break;

                    case "Yearly":

                        task.NextRep = DateTime.Today.AddYears(1);
                        task.UpdateTask();

                        break;


                }
            }
        }
    }

}