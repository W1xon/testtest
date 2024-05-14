using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AForTest
{
    public class Human
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; } 
        public string SecondName { get; set; }

        public IEnumerator<string> Task { get; set; }
        public Bitmap BMP { get; set; }
        //public List<bool> TaskCheck { get; set; }
        public Human() { }
        public Human(string name, string secondName)
        {
            Name = name;
            SecondName = secondName;
        }
    }
}
