using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace uwp_to_do_list
{
    public class Resource_Manager
    {
       
     ResourceManager rm = new ResourceManager("Resources", Assembly.GetExecutingAssembly());

        public string GetDefaultDate
        {
            get =>  rm.GetString("DefaultDueDate");                   
        }

     
       
    }
}

  

