using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using System.Linq;
using Windows.UI.Xaml;

namespace uwp_to_do_list
{
    public sealed partial class MainView : Page
    {
        ObservableCollection<TaskTodo> Tasks = new ObservableCollection<TaskTodo>();
        ObservableCollection<TaskTodo> SubTasks = new ObservableCollection<TaskTodo>();
        TaskTodo task = new TaskTodo();
        Func<DateTimeOffset, string> SetDate = (date) => string.Format("{0}-{1}-{2}",  date.Month, date.Day, date.Year);
     
        public MainView()
        {
            this.InitializeComponent();    
            //Get the tasks 
            Tasks = task.GetTasks();         
            task_list.ItemsSource = Tasks;
            task_list.SelectedValuePath = "TaskId";

            //Get the subtasks          
            subtask_list.ItemsSource = SubTasks;
            number_repeat.MaxLength = 3;
            
          
        }
       
        private void add_Task_textbox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                TaskTodo Task = new TaskTodo();
                //add task to de database and the observable collection
                Task.NameTask = add_Task_textbox.Text;
                Task.AddTask(Task);
                Tasks.Add(Task);
                //update the observable collection 
                Tasks = task.GetTasks();
                task_list.ItemsSource = Tasks;
                //select the new task 
                task_list.SelectedItem = Task;              
                //clean the texbox and focus
                add_Task_textbox.Text = String.Empty;
                add_Task_textbox.Focus(Windows.UI.Xaml.FocusState.Keyboard);                          

            }
            
        }
        private void add_Task_textbox_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e) => add_Task_textbox.Text = string.Empty;

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
             (task_list.SelectedItem as TaskTodo).Date = SetDate(calendar_date.SelectedDates[0]);
             (task_list.SelectedItem as TaskTodo).UpdateTask();
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
            (task_list.SelectedItem as TaskTodo).Reminder = string.Format("{0}-{1}-{2} {3}:{4}:{5}", Date.Month, Date.Day, Date.Year, Date.Hour, Date.Minute, Date.Second);
            //update in DB
            //clear values
            reminder_calendar.SelectedDates.Clear();
            reminder_time_picker.SelectedTime = null;
           
           
        }
        private void priority_checkbox_click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            task.Priority = radioButton.Name;
         
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
              SubTask.ParentTask = (task_list.SelectedItem as TaskTodo).TaskId; //the id of the task selected
              SubTask.AddTask(SubTask);
              SubTasks.Add(SubTask);
              SubTask_TexBox.Text = String.Empty;
              
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
                       
                        break;


                    case "Weekly":

                        task.NextRep = DateTime.Today.AddDays(7);
                       
                        break;


                    case "Montly":

                        task.NextRep = DateTime.Today.AddMonths(1);
                        
                       
                        break;

                    case "Yearly":

                        task.NextRep = DateTime.Today.AddYears(1);
                       

                        break;
                }
            }
        }

        private void list_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if((task_list.SelectedItem as TaskTodo) != null)
            {
                (task_list.SelectedItem as TaskTodo).NameList = textbox.Text;
                (task_list.SelectedItem as TaskTodo).UpdateTask();
            }
            
        }

        
        private void CheckPriority()
        {
            if((task_list.SelectedItem as TaskTodo) != null)
               
            {
                switch ((task_list.SelectedItem as TaskTodo).Priority)
                {
                    case "low":
                        low.IsChecked = true;
                        break;

                    case "medium":
                        medium.IsChecked = true;
                        break;

                    case "high":
                        high.IsChecked = true;
                        break;

                    default:
                        low.IsChecked = false;
                        medium.IsChecked = false;
                        high.IsChecked = false;

                        break;
                }

            }
           
        }

        private void descriptions_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if ((task_list.SelectedItem as TaskTodo) != null)
            {
                (task_list.SelectedItem as TaskTodo).Description = textbox.Text;
                (task_list.SelectedItem as TaskTodo).UpdateTask();
            }
             
        }

        private void NameTaskForm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((task_list.SelectedItem as TaskTodo)!= null)
            {
                (task_list.SelectedItem as TaskTodo).NameTask = NameTaskForm.Text;
                (task_list.SelectedItem as TaskTodo).UpdateTask();
            }
            
        }
   

        private void task_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskForm.Visibility = Visibility.Visible;
            CheckPriority();
            if(task_list.SelectedItem != null)
            {
                SubTasks = (task_list.SelectedItem as TaskTodo).GetSubtasks();
                subtask_list.ItemsSource = SubTasks;
            }
            
        }

        private void Proritycheckbox_Checked (object sender, RoutedEventArgs e)
        {
            RadioButton RadioB = sender as RadioButton;
            (task_list.SelectedItem as TaskTodo).Priority = RadioB.Name;
            (task_list.SelectedItem as TaskTodo).UpdateTask();
        }
    }

}