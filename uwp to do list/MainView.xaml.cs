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
        public MainView()
        {
            this.InitializeComponent();
            TaskTodo taskTodo = new TaskTodo();
            task_list.ItemsSource = taskTodo.GetTasks();
         

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

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
          
         
        }



 }

