using FontAwesome.UWP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace uwp_to_do_list
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    /// resolver que 
    public sealed partial class MainPage : Page
    {
      
        private ObservableCollection<Task> tasks_;
        public ObservableCollection<Task> Tasks

        {
            get
            { return tasks_; }

            set { tasks_ = value; }

        }

        public MainPage()
        {
            this.InitializeComponent();

        }

     


        public static ObservableCollection<Task> GetTask(string connectionString)
        {
            string GetTask_query = "select * from task";

            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            try
            {
                Task task = new Task();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetTask_query;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    try
                                    {

                                        task.Name_task = reader.GetString(1);
                                        task.date = reader.GetDateTime(2);
                                        task.Reminder = reader.GetDateTime(3);
                                        task.name_list = reader.GetString(4);
                                        task.subtask = reader.GetString(5);
                                        task.priority = reader.GetString(6);
                                        tasks.Add(task);
                                    }
                                    catch (Exception e)
                                    { Debug.Print(e.Message); }
                                }
                            }
                        }

                    }

                }
                return tasks;
            }
            catch (Exception esql)
            {
                Debug.Print("Exception: " + esql.Message);
            }
            return null;

        }
    }
}