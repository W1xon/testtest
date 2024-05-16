using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_Generator
{
    public class ExampleData
    {
        public int ID { get; set; }
        public string Примеры { get; set; }
        public string Имя { get; set; }
        public string Ответы { get; set; }
        public string Время_решения_всех_примеров { get; set; }
        public string Дата_решения_примеров { get; set; }
        public int Оценка { get; set; }
    }
}
