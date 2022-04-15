using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_to_do_list.Viewmodels
{
    public  class DefaultList
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public DefaultList(string name, string icon)
        {
            Name = name;
            Icon = icon;
        }
      
    


    }
}
