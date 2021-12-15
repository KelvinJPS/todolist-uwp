using Windows.UI.Xaml.Controls;
using uwp_to_do_list.view_model;



// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace uwp_to_do_list.views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainView : Page 
    {
        public MainView()
        {
            this.InitializeComponent();
            Task_Todo task_Todo = new Task_Todo();
            task_list.ItemsSource = task_Todo.model.GetTasks();


        }

 
    }
}
