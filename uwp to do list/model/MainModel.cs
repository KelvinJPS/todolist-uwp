using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_to_do_list.view_model;

namespace uwp_to_do_list.model
{
    public class  MainModel
    {
     public ObservableCollection<Task_Todo> GetTasks() 
        {
         var _Task = new Task_Todo();
         _Task.Name_Task = "task1";
         _Task.Tasks.Add(_Task);
        
           return  _Task.Tasks;
        }

     public string s()
        {
            return "hola";
        }








    }
}


