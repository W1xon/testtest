using System;
using System.Collections.Generic;
using System.Linq;

namespace Example_Generator
{
    class AverageValue
    {
        public string Name { get; set; }
        public double Appraisal { get; set; }
        public double Time { get; set; }
    }
    public class OutputDataFromDataBase
    {
        public static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine
                (
                "Введите <<1>> чтобы получить всю информацию об последней игры\n" +
                "Введите <<2>> чтобы получить средние значения всех игроков\n" +
                "Введите <<3>> чтобы получить всю информацию об всех игроках\n" +
                "Введите <<4>> чтобы получить всю информацию об одном игроке\n" +
                "Введите <<5>> чтобы получить всю информацию об одной игре\n" +
                "Введите <<6>> чтобы получить дынные о последних n играх\n" +
                "Введите <<7>> чтобы получить информацию о общем колличестве сыграных игр\n" +
                "Введите <<8>> чтобы сравнить двух игроков"
                );
            string Command = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (Command == "1") DataEndGame();
            else if (Command == "2") AverageOfAllPlayers();
            else if (Command == "3") AllData();
            else if (Command == "4") AllDataOfOnePlayer();
            else if (Command == "5") DataOneGame();
            else if (Command == "6") DataLastGame();
            else if (Command == "7") CountAllGames();
            else if (Command == "8") ComparisonPlayer();
        }
        public static void DataEndGame()
        {
            Console.WriteLine("Ждите...");
            using (var context = new MyDbContext())
            {
                var ListData = context.ExampleData.Select(x => x).ToArray();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Имя игрока: {ListData[ListData.Count() - 1].Имя}");
                Console.WriteLine($"Оценка: {ListData[ListData.Count() - 1].Оценка}");
                Console.WriteLine($"Примеры:\n{ListData[ListData.Count() - 1].Примеры}");
                Console.WriteLine($"Ответы: {ListData[ListData.Count() - 1].Ответы}");
                Console.WriteLine($"Время решения всех примеров: {ListData[ListData.Count() - 1].Время_решения_всех_примеров}");
                Console.WriteLine($"Время когда решили примеры: {ListData[ListData.Count() - 1].Дата_решения_примеров}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
        public static void AverageOfAllPlayers()
        {
            Console.WriteLine("Ждите...");

            using (var context = new MyDbContext())
            {
                double appraisal = 0;
                double time = 0;
                var ListName = context.Name.Select(x => x.Имя).ToArray();
                ListName = ListName.Distinct().ToArray();
                List<AverageValue> averageDataPlayer = new List<AverageValue>();
                for (int j = 0; j < ListName.Length; j++)
                {
                    time = 0;
                    appraisal = 0;
                    var ListDataBase = context.ExampleData.Select(x => x).ToArray();
                    List<ExampleData> ListSortedData = new List<ExampleData>();

                    for (int i = 0; i < ListDataBase.Count(); i++)
                        if (ListDataBase[i].Имя == ListName[j]) ListSortedData.Add(ListDataBase[i]);

                    for (int i = 0; i < ListSortedData.Count(); i++)
                    {
                        appraisal += ListSortedData[i].Оценка;
                        time += StringToNumber(ListDataBase[i].Время_решения_всех_примеров);
                    }
                    averageDataPlayer.Add(new AverageValue
                    {
                        Name = ListName[j],
                        Appraisal = Math.Round(appraisal / ListSortedData.Count()),
                        Time = Math.Round(time / ListSortedData.Count())
                    });
                }
                averageDataPlayer = averageDataPlayer.OrderByDescending(x => x.Appraisal).ToList();
                for (int i = 0; i < averageDataPlayer.Count; i++)
                {
                    if (i == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine("Лучший игрок: ");
                        Console.WriteLine($"{averageDataPlayer[i].Name}\nСредняя оценка: {averageDataPlayer[i].Appraisal}\n" +
                            $"Среднее время решения примеров {averageDataPlayer[i].Time}");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (i == averageDataPlayer.Count - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Худший игрок: ");
                        Console.WriteLine($"{averageDataPlayer[i].Name}\nСредняя оценка: {averageDataPlayer[i].Appraisal}\n" +
                            $"Среднее время решения примеров {averageDataPlayer[i].Time}");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"{averageDataPlayer[i].Name}\nСредняя оценка: {averageDataPlayer[i].Appraisal}\n" +
                            $"Среднее время решения примеров {averageDataPlayer[i].Time}");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                }
            }
        }
        public static void AllData()
        {
            Console.WriteLine("Ждите...");
            using (var context = new MyDbContext())
            {
                var ListData = context.ExampleData.Select(x => x).ToArray();
                for (int i = 0; i < ListData.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{i + 1}-ый решенный пример");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Имя игрока: {ListData[i].Имя}");
                    Console.WriteLine($"Оценка: {ListData[i].Оценка}");
                    Console.WriteLine($"Ответы: {ListData[i].Ответы}");
                    Console.WriteLine($"Время решения всех примеров: {ListData[i].Время_решения_всех_примеров}");
                    Console.WriteLine($"Примеры:\n{ListData[i].Примеры}");
                    Console.WriteLine($"Время когда решили примеры: {ListData[i].Дата_решения_примеров}");
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
        public static void AllDataOfOnePlayer()
        {
            string name;
            Console.Write("Введите имя игрока данные которго вы хотите получить: ");
            name = Console.ReadLine();
            name = NameCorrection(name);
            Console.WriteLine("Ждите...");
            using (var context = new MyDbContext())
            {
                List<ExampleData> ListData = new List<ExampleData>();
                var ListName = context.Name.Select(x => x.Имя).ToArray();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                if (ListName.Contains(name) == false) Console.WriteLine("Такой игрок не решал примеры проверьте правильно ли вы написали имя игрока!!!");
                else
                {
                    var ListDataBase = context.ExampleData.Select(x => x).ToArray();
                    for (int i = 0; i < ListDataBase.Count(); i++)
                        if (ListDataBase[i].Имя == name) ListData.Add(ListDataBase[i]);
                    for (int i = 0; i < ListData.Count(); i++)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"Всего решенных примеров: {ListData.Count()}");
                        Console.WriteLine($"{i + 1}-ый решенный пример");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Имя игрока: {ListData[i].Имя}");
                        Console.WriteLine($"Оценка: {ListData[i].Оценка}");
                        Console.WriteLine($"Ответы: {ListData[i].Ответы}");
                        Console.WriteLine($"Время решения всех примеров: {ListData[i].Время_решения_всех_примеров}");
                        Console.WriteLine($"Примеры:\n{ListData[i].Примеры}");
                        Console.WriteLine($"Время когда решили примеры: {ListData[i].Дата_решения_примеров}");
                    }
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

        }
        public static void DataOneGame()
        {
            int numberGame = 0;
            Console.Write("Введите номер игры: ");
            if (int.TryParse(Console.ReadLine(), out numberGame) == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Введены некоректные данные!!!");
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.WriteLine("Ждите...");
                numberGame--;
                using (var context = new MyDbContext())
                {
                    if (numberGame < 0 || numberGame > context.ExampleData.Sql.Count())
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Игр с таким номером не существует");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        var ListData = context.ExampleData.Select(x => x).ToArray();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Имя игрока: {ListData[numberGame].Имя}");
                        Console.WriteLine($"Оценка: {ListData[numberGame].Оценка}");
                        Console.WriteLine($"Примеры:\n{ListData[numberGame].Примеры}");
                        Console.WriteLine($"Ответы: {ListData[numberGame].Ответы}");
                        Console.WriteLine($"Время решения всех примеров: {ListData[numberGame].Время_решения_всех_примеров}");
                        Console.WriteLine($"Время когда решили примеры: {ListData[numberGame].Дата_решения_примеров}");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                }
            }
        }
        public static void DataLastGame()
        {
            int countLastGame = 0;
            Console.Write("Введите число n чтобы вывести данные последних n игр: ");
            if (int.TryParse(Console.ReadLine(), out countLastGame) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введены некорректные дынные!!!");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                return;
            }
            Console.WriteLine("Ждите...");
            using (var context = new MyDbContext())
            {
                var ListData = context.ExampleData.Select(x => x).ToArray();
                if (ListData.Length - countLastGame < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Не сыграно столько игр {countLastGame++}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    return;
                }
                for (int i = ListData.Length - countLastGame; i < ListData.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"{i + 1}-ый решенный пример");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Имя игрока: {ListData[i].Имя}");
                    Console.WriteLine($"Оценка: {ListData[i].Оценка}");
                    Console.WriteLine($"Ответы: {ListData[i].Ответы}");
                    Console.WriteLine($"Время решения всех примеров: {ListData[i].Время_решения_всех_примеров}");
                    Console.WriteLine($"Примеры:\n{ListData[i].Примеры}");
                    Console.WriteLine($"Время когда решили примеры: {ListData[i].Дата_решения_примеров}");
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
        public static void CountAllGames()
        {
            Console.WriteLine("Ждите...");
            using (var context = new MyDbContext())
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                var ListData = context.ExampleData.Select(x => x).ToArray();
                Console.WriteLine($"Колличество сыграных игр {ListData.Count()}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
        public static void ComparisonPlayer()
        {
            string[] Name;
            Console.Write ("Введите имена <через пробел> 2 игроков которых хотите сравнить ");
            Name = Console.ReadLine().Split(' ');
            Console.WriteLine("Ждите...");
            using (var context = new MyDbContext())
            {
                double appraisal = 0;
                double time = 0;
                var ListName = context.Name.Select(x => x.Имя).ToArray();
                ListName = ListName.Distinct().ToArray();
                ListName = (from n in ListName
                           select n.ToUpper()).ToArray();
                if (Name.Length < 2 || ListName.Contains(Name[0].ToUpper()) == false || ListName.Contains(Name[1].ToUpper()) == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Такие игроки не решали примеры или строка была пуста!!!");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    return;
                }
                List<AverageValue> averageDataPlayer = new List<AverageValue>();
                for (int j = 0; j < ListName.Length; j++)
                {
                    time = 0;
                    appraisal = 0;
                    var ListDataBase = context.ExampleData.Select(x => x).ToArray();
                    List<ExampleData> ListSortedData = new List<ExampleData>();

                    for (int i = 0; i < ListDataBase.Count(); i++)
                        if (ListDataBase[i].Имя.ToUpper() == ListName[j].ToUpper()) ListSortedData.Add(ListDataBase[i]);

                    for (int i = 0; i < ListSortedData.Count(); i++)
                    {
                        appraisal += ListSortedData[i].Оценка;
                        time += StringToNumber(ListDataBase[i].Время_решения_всех_примеров);
                    }
                    averageDataPlayer.Add(new AverageValue 
                    { 
                        Name = ListName[j], Appraisal = Math.Round(appraisal / ListSortedData.Count()), Time = Math.Round(time / ListSortedData.Count())
                    });
                }
                averageDataPlayer = averageDataPlayer.OrderByDescending(x => x.Appraisal).ToList();

                for (int i = 0; i < averageDataPlayer.Count(); i++)
                    if (averageDataPlayer[i].Name.ToUpper() != Name[0].ToUpper() && averageDataPlayer[i].Name.ToUpper() != Name[1].ToUpper()) averageDataPlayer.RemoveAt(i);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                if (averageDataPlayer[0].Appraisal != averageDataPlayer[1].Appraisal)
                {
                    Console.WriteLine($"Игрок {NameCorrection(averageDataPlayer[0].Name)} лучше игрока {NameCorrection(averageDataPlayer[1].Name)} потому что:\n" +
                        $"У игрока {NameCorrection(averageDataPlayer[0].Name)} средняя оценка {averageDataPlayer[0].Appraisal}\n" +
                        $"А у игрока {NameCorrection(averageDataPlayer[1].Name)} средняя оценка {averageDataPlayer[1].Appraisal}");
                }
                else
                {
                    averageDataPlayer = averageDataPlayer.OrderBy(x => x.Time).ToList();
                    Console.WriteLine($"Игрок {NameCorrection(averageDataPlayer[0].Name)} лучше игрока {NameCorrection(averageDataPlayer[1].Name)} потому что:\n" +
                        $"У игрока {NameCorrection(averageDataPlayer[0].Name)} средняя оценка {averageDataPlayer[0].Appraisal}\n" +
                        $"А у игрока {NameCorrection(averageDataPlayer[1].Name)} средняя оценка {averageDataPlayer[1].Appraisal}");
                }
                if(averageDataPlayer[0].Time > averageDataPlayer[1].Time)
                    Console.WriteLine($"Но игрок {NameCorrection(averageDataPlayer[1].Name)} решает примеры быстрее {averageDataPlayer[1].Time} против {averageDataPlayer[0].Time}");
                else if(averageDataPlayer[0].Time < averageDataPlayer[1].Time)
                    Console.WriteLine($"И игрок {NameCorrection(averageDataPlayer[0].Name)} решает примеры быстрее {averageDataPlayer[0].Time} против {averageDataPlayer[1].Time}");
                else
                    Console.WriteLine($"А время решения примеров у них одинаковое {averageDataPlayer[1].Time}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }


        public static string NameCorrection(string name)
        {
            name = name.ToUpper();
            string resultname = "";
            resultname += name[0].ToString();
            for (int i = 1; i < name.Length; i++)
                resultname += name[i].ToString().ToLower();
            return resultname;
        }
        public  static double StringToNumber(string str)
        {
            string[] array = str.Split(':');
            array[2] = array[2].Replace(".",",");
            double number = 0;
            number += Convert.ToDouble(array[0]) * 60;
            number += Convert.ToDouble((array[1]));
            number += Convert.ToDouble((array[2])) / 100;
            return Math.Round(number, 2);
        }
    }
}
