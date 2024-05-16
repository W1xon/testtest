using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Example_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int iteration = 100;
            string operators = "/*-+";
            int LengthExample = 50;
            string[] MinMax = new string[2] { "2", "2" };
            List<Example> ArrayExample = new List<Example>();
            List<string> AnswersInput = new List<string>();
            string AnswerInput = "";
            string answerTrue = "";
            string answerFalse = "";
            int valueCorrectAnswer = 0;
            string Name = "";
            string Time = "";
            string command = "";
            //логика приложения
            while (true)
            {
                valueCorrectAnswer = 0;
                ArrayExample.Clear();
                AnswersInput.Clear();
                answerTrue = "";
                answerFalse = "";
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("Если хотите получить данные введите команду если не знаете команды введите <<0>>. Если хотите решать примеры нажмите <<Enter>>: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    command = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (command.Replace(" ", "") == "") break;
                    OutputData(command);
                }
                Console.Write("Введите имя: ");
                Name = Console.ReadLine().Replace(" ", "");
                Name = OutputDataFromDataBase.NameCorrection(Name);
                Console.Write("Введите цифру что вы хотитие сделать <<1 - решать примеры, 2 - решать таблицу умножения>> ");
                if (Console.ReadLine() == "1")
                    GenerationExmple();
                else MultiplicationGeneratorTable();
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine("Ждите...");
                SaveResultInDataBase();
                OutputResultInConsole();
                Console.WriteLine($"Время решения примеров: {Time}");
                //Console.WriteLine(DateTime.Now.ToString());
            }
            //Сохренение в бд
            void SaveResultInDataBase()
            {
                using (var context = new MyDbContext())
                {
                    var Data = new ExampleData()
                    {
                        Имя = Name,
                        Примеры = String.Join("; \n", ArrayExample.Select(x => x.ExampleString)),
                        Ответы = $"\nПравильные ответы: {(answerTrue == "" ? "Правильных ответов нет неудачник" : answerTrue)}\n" +
                        $"Неправильные ответы: {(answerFalse == "" ? Name.ToUpper() == "ВОВА" || Name.ToUpper() == "МАКСИМ" ? "Молодец все сделал правильно" : "Молодец все сделалa правильно" : answerFalse)}",
                        Время_решения_всех_примеров = Time,
                        Дата_решения_примеров = DateTime.Now.ToString(),
                        Оценка = Appraisal(iteration, valueCorrectAnswer)
                    };
                    context.ExampleData.Add(Data);
                    var N = new Name() { Имя = Name };
                    context.Name.Add(N);
                    context.SaveChanges();
                }
            }
            //Вывод из бд
            void OutputData(string Command)
            {
                if (Command == "0") OutputDataFromDataBase.Help();
                else if (Command == "1") OutputDataFromDataBase.DataEndGame();
                else if (Command == "2") OutputDataFromDataBase.AverageOfAllPlayers();
                else if (Command == "3") OutputDataFromDataBase.AllData();
                else if (Command == "4") OutputDataFromDataBase.AllDataOfOnePlayer();
                else if (Command == "5") OutputDataFromDataBase.DataOneGame();
                else if (Command == "6") OutputDataFromDataBase.DataLastGame();
                else if (Command == "7") OutputDataFromDataBase.CountAllGames();
                else if (Command == "8") OutputDataFromDataBase.ComparisonPlayer();
            }
            //Вывод результата в консоль
            void OutputResultInConsole()
            {

                Console.WriteLine("Ваша оценка: " + Appraisal(iteration, valueCorrectAnswer));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Правильные ответы: {(answerTrue == "" ? "Правильных ответов нет неудачник" : answerTrue)}\n" +
                        $"Неправильные ответы: {(answerFalse == "" ? Name.ToUpper() == "ВОВА" || Name.ToUpper() == "МАКСИМ" ? "Молодец все сделал правильно" : "Молодец все сделалa правильно" : answerFalse)}");
            }
            //Создание примера из таблицы умножения
            void MultiplicationGeneratorTable()
            {
                Console.Write("Введите колличество примеров: ");
                if (int.TryParse(Console.ReadLine(), out iteration) == false)
                    iteration = 10;

                Console.Write("Введите число: ");
                MinMax = Console.ReadLine().Split(' ');
                if (MinMax[0] == "")
                    MinMax = new string[1] { "10" };

                sw.Restart();
                for (int i = 0; i < iteration; i++)
                {
                    Console.Write($"{i + 1}) ");
                    ArrayExample.Add(new Example(int.Parse(MinMax[0])));
                    Console.Write(" = ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    AnswerInput = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    AnswersInput.Add(AnswerInput);
                }
                sw.Stop();
                Time = sw.Elapsed.ToString();
                for (int i = 0; i < AnswersInput.Count(); i++)
                {
                    if (AnswersInput[i] == ArrayExample[i].Answer.ToString())
                    {
                        answerTrue += $"{i + 1}): {AnswersInput[i]} ";
                        valueCorrectAnswer++;
                    }
                    else
                        answerFalse += $"\n{i + 1}): было {(AnswersInput[i] == "" ? "пусто" : AnswersInput[i])} > Правильный ответ {ArrayExample[i].Answer} ";
                }

            }
            //Создание примера
            void GenerationExmple()
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Если пример не появляется дольше 10 секунд выключите и включите программу!!!, или можете ждать пока он не появится)");
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write("Введите колличество примеров: ");
                if (int.TryParse(Console.ReadLine(), out iteration) == false)
                    iteration = 10;

                Console.Write("Введите длинну примера: ");
                if (int.TryParse(Console.ReadLine(), out LengthExample) == false)
                    LengthExample = 2;

                Console.Write("Введите два числа через пробел <<Min, Max>>: ");
                MinMax = Console.ReadLine().Split(' ');
                if (MinMax[0] == "" || MinMax[1] == "")
                    MinMax = new string[2] { "0", "100" };

                Console.Write("Введите какие знаки использовать <<-+/*>>: ");
                operators = Console.ReadLine();
                if (operators == "")
                    operators = "/*-+";

                sw.Restart();
                for (int i = 0; i < iteration; i++)
                {
                    Console.Write($"{i + 1}) ");
                    ArrayExample.Add(new Example(LengthExample, operators, int.Parse(MinMax[0]), int.Parse(MinMax[1])));
                    Console.Write(" = ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    AnswerInput = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    AnswersInput.Add(AnswerInput);
                }
                sw.Stop();
                Time = sw.Elapsed.ToString();
                for (int i = 0; i < AnswersInput.Count(); i++)
                {
                    if (AnswersInput[i] == ArrayExample[i].Answer.ToString())
                    {
                        answerTrue += $"{i + 1}: {AnswersInput[i]} ";
                        valueCorrectAnswer++;
                    }
                    else
                        answerFalse += $"\n{i + 1}): было {(AnswersInput[i] == "" ? "пусто" : AnswersInput[i])} > Правильный ответ {ArrayExample[i].Answer} ";
                }
            }
            //Подсчет оценки
            int Appraisal(int CountExample, int valueAnswerCorrect)
            {
                int apparaisal = 0;
                int precent = (valueAnswerCorrect * 100) / CountExample;
                if (precent >= 90)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    apparaisal = 5;
                }
                else if (precent >= 70)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    apparaisal = 4;
                }
                else if (precent >= 50)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    apparaisal = 3;
                }
                else if (precent <= 50 && precent != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    apparaisal = 2;
                }
                else if (precent == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    apparaisal = 0;
                }
                return apparaisal;
            }
        }
    }
}
