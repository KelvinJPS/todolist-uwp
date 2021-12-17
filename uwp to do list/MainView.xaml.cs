using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System;
using System.IO;
using System.Collections.ObjectModel;


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
                   {
                      taskTodo.NameTask = add_Task_textbox.Text;
                      taskTodo.AddTask(taskTodo.NameTask);
                      Tasks.Add(taskTodo);
                      add_Task_textbox.Text = String.Empty;
                      add_Task_textbox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                     

                    }
                   catch (Exception ex)
                    {
                      Debug.WriteLine(ex.ToString());
                    }

                }
            }

        private void add_Task_textbox_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e) => add_Task_textbox.Text = String.Empty;

        private void add_Task_textbox_LosingFocus(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.Input.LosingFocusEventArgs args) => add_Task_textbox.Text = "add task";
       
    }



 }

