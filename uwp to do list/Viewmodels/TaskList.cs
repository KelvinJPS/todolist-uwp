using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwp_to_do_list.Data_access;

namespace uwp_to_do_list.Viewmodels
{
    public class TaskList
    {
        TasklistSqliteDataAccess tasklistSqlite = new TasklistSqliteDataAccess();
        public ObservableCollection<string> Getlists() => tasklistSqlite.GetListsDB();
        public void AddList(string NameList) => tasklistSqlite.AddList(NameList);
     
    }
}
