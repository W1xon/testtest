using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AForTest
{
    internal class DataBase
    {
        SQLiteConnection database;
        public DataBase(string Path)
        {
            database = new SQLiteConnection(Path);
            database.CreateTable<Human>();
        }
        public List<Human> GetItems()
        {
            return database.Table<Human>().ToList();
        }
        public Human GetItem(int id)
        {
            return database.Get<Human>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<Human>(id);
        }
        public int SaveItem(Human item)
        {
            if (item.ID != 0)
            {
                database.Update(item);
                return item.ID;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}
