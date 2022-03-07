using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace uwp_to_do_list.Data_access
{
    public class TasklistSqliteDataAccess
    {

        public async void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("TasksSqlite.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS List (Primary_Key INTEGER PRIMARY KEY, " +
                    "Name_list NVARCHAR(100));";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();

            }
        }
            public ObservableCollection<string> GetListsDB()
        {
            var Lists = new ObservableCollection<string>();
            //get data from sqlite3
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from List", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    try 
                    {
                        Lists.Add(query.GetString(1));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                db.Close();
            }
            return Lists;
        }

        public void AddList(string NameList)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TasksSqlite.db");
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO List VALUES (NULL,@ListName );";
                    insertCommand.Parameters.AddWithValue("@ListName", NameList);
                    insertCommand.ExecuteReader();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
