using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System;
using System.IO;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.Runtime.InteropServices;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace uwp_to_do_list
{

    public sealed partial class MainView : Page
    {
        ObservableCollection<TaskTodo> Tasks = new ObservableCollection<TaskTodo>();
        public MainView()
        {
            this.InitializeComponent();
            TaskTodo taskTodo = new TaskTodo();           
            Tasks = taskTodo.GetTasks();
            task_list.ItemsSource = Tasks;
         
           
        }

        private void add_Task_textbox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            TaskTodo taskTodo = new TaskTodo();


            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                try
                {  //add task to de database and the observable collection
                    taskTodo.NameTask = add_Task_textbox.Text;
                    taskTodo.AddTask(taskTodo);
                    Tasks.Add(taskTodo);

                    //show the task data 
                    TaskForm.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    //clean the texbox and focus
                    add_Task_textbox.Text = String.Empty;
                    add_Task_textbox.Focus(Windows.UI.Xaml.FocusState.Keyboard);

                    task_list.SelectedItem = taskTodo;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }

        private void add_Task_textbox_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e) => add_Task_textbox.Text = string.Empty;

        private void task_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskForm.Visibility = Windows.UI.Xaml.Visibility.Visible;

            TaskTodo task = (TaskTodo)task_list.SelectedItem;
            NameTask_form.Text = task.NameTask;          
            
        }

        private void calendar_button (object sender, Windows.UI.Xaml.RoutedEventArgs e) => calendar_popup.IsOpen = true;

        private void quit_TaskForm_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => TaskForm.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        private void cancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => calendar_popup.IsOpen = false;

        private void save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TaskTodo task = new TaskTodo();
            try
            {
                DateTimeOffset date = calendar_date.SelectedDates[0];
                task.Date = string.Format("{0}-{1}-{2}", date.Day, date.Month, date.Year);
            }
           
            catch (COMException ex)
            {
                task.Date = string.Format("{0}-{1}-{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                task.updateTask(task);

            }
           
            calendar_popup.IsOpen = false;

        }

        private void reminder_cancel_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => reminder_popup.IsOpen = false;
  

        private void reminder_save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TaskTodo task = new TaskTodo();
            task.Reminder = (TimeSpan)reminder_time_picker.SelectedTime;
            task.updateTask(task);

            
        }

        private void priority_checkbox_click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TaskTodo task = new TaskTodo();
            RadioButton radioButton = sender as RadioButton;
            task.Priority = radioButton.Name;
            task.updateTask(task);

        }

        private void descriptions_textbox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TaskTodo task = new TaskTodo();
                task.description = descriptions_textbox.Text;
                task.updateTask(task);
                descriptions_textbox.Text = string.Empty;
            }
        }

        private void reminder_button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => reminder_popup.IsOpen = true;
        

    }



 }

