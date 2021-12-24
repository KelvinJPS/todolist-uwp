using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace uwp_to_do_list
{    
    public sealed partial class MainView : Page
    {
        ObservableCollection<TaskTodo> Tasks = new ObservableCollection<TaskTodo>();
        TaskTodo task = new TaskTodo();
        Func<DateTimeOffset,string> SetDate = (date) => string.Format("{0}-{1}-{2}", date.Day, date.Month, date.Year);
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

        private void calendar_button (object sender, Windows.UI.Xaml.RoutedEventArgs e) => calendar_popup.IsOpen = true;

        private void quit_TaskForm_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => TaskForm.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        private void cancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => calendar_popup.IsOpen = false;

        private void save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => task.Date = SetDate(calendar_date.SelectedDates[0]);    
      
        private void reminder_cancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => reminder_popup.IsOpen = false;
  

        private void reminder_save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TimeSpan time = (TimeSpan)reminder_time_picker.SelectedTime;
            task.Reminder = time.ToString();            
        }
        private void priority_checkbox_click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
           
            task.Priority = radioButton.Name;
            Debug.WriteLine("estoy");
        }
        private void descriptions_textbox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                task.Description = descriptions_textbox.Text;               
                descriptions_textbox.Text = string.Empty;
            }
        }
        private void reminder_button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => reminder_popup.IsOpen = true;
     
    }



 }

