using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_Generator
{
    public class Example
    {
        public string ExampleString { get; set; }
        public int Answer { get; set; }
        private Random rand = new Random();
        private List<String> PartExample = new List<string>();
        public Example(int Length, string operators, int Min, int Max)
        {
            PartExample = GenerateExample(Length, operators, Min, Max);
            Answer = Decision(PartExample);
            while (Answer < 0)
            {
                PartExample = GenerateExample(Length, operators, Min, Max);
                Answer = Decision(PartExample);
            }
            Console.Write(ExampleString);
        }

        public Example(int number)
        {
            int num1, num2;
            num1 = rand.Next(0, number + 1);
            num2 = rand.Next(0, 11);
            Answer = num1 * num2;
            ExampleString = $"{num1}*{num2}";
            Console.Write(ExampleString);
        }

        private void DivisionAndMinusCheck(List<string> PartExample, ref bool minus, ref bool division, int Min, int Max, string operators, bool End)
        {
            int num = 0;
            if (minus == true)
            {
                num = rand.Next(Min, int.Parse(PartExample[PartExample.Count - 2]));
                PartExample.Add(num.ToString());
                if (End == false) PartExample.Add(operators);
            }
            else if (division == true)
            {
                num = rand.Next(Min, Max + 1);
                int i = 0;
                while ((Convert.ToDouble(PartExample[PartExample.Count - 2]) / num) % 1 != 0 || num == 0)
                {
                    Max = int.Parse(PartExample[PartExample.Count - 2]) + 1;
                    i++;
                    Min = Min == 0 ? 1 : Min;
                    Max = Max < Min ? Min + 1 : Max;
                    num = rand.Next(Min, Max);
                }
                PartExample.Add(num.ToString());
                if (End == false) PartExample.Add(operators);
            }
            else
            {
                num = rand.Next(Min, Max + 1);
                PartExample.Add(num.ToString());
                if (End == false) PartExample.Add(operators);
            }
        }
        private List<string> GenerateExample(int Length, string operators, int Min, int Max)
        {
            List<string> PartExample = new List<string>();
            bool minus = false;
            bool division = false;
            for (int i = 0; i < Length; i++)
            {
                int operationindex = rand.Next(0, operators.Length);
                if (operators[operationindex] == '-')
                {
                    if (i == Length - 1)
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "-", true);
                    }
                    else
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "-", false);
                        minus = true;
                        division = false;
                    }
                }
                else if (operators[operationindex] == '+')
                {
                    if (i == Length - 1)
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "+", true);
                    }
                    else
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "+", false);
                        minus = false;
                        division = false;
                    }
                }
                else if (operators[operationindex] == '*')
                {
                    if (i == Length - 1)
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "*", true);
                    }
                    else
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "*", false);
                        minus = false;
                        division = false;
                    }
                }
                else
                {
                    if (i == 0)
                    {

                        int num = rand.Next(Min, Max);
                        while (num == 0)
                            num = rand.Next(Min, Max);
                        PartExample.Add(num + "");
                        PartExample.Add("/");
                        minus = false;
                        division = true;
                    }
                    else if (i == Length - 1)
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "/", true);
                        minus = false;
                        division = true;
                    }
                    else
                    {
                        DivisionAndMinusCheck(PartExample, ref minus, ref division, Min, Max, "/", false);
                        minus = false;
                        division = true;
                    }
                }
            }
            return PartExample;
        }

        private int Decision(List<string> PartExample)
        {
            double number = 0;
            ExampleString = String.Join("", PartExample);
            int index = 0;
            while (PartExample.Contains("*") || PartExample.Contains("/"))
            {
                if ((PartExample.IndexOf("*") < PartExample.IndexOf("/") || PartExample.IndexOf("/") < 0) && PartExample.Contains("*"))
                {
                    number = DecisionByOperators("*", PartExample, index);
                    if (number < 0) return -1;
                }
                if ((PartExample.IndexOf("/") < PartExample.IndexOf("*") || PartExample.IndexOf("*") < 0) && PartExample.Contains("/"))
                {
                    number = DecisionByOperators("/", PartExample, index);
                    if (number % 1 != 0) return -1;
                }
            }
            while (PartExample.Count() > 1)
            {
                if (PartExample.Count() > 1 && ((PartExample.IndexOf("-") < PartExample.IndexOf("+") || PartExample.IndexOf("+") < 0) && PartExample.Contains("-")))
                    number = DecisionByOperators("-", PartExample, index);

                if (PartExample.Count() > 1 && ((PartExample.IndexOf("+") < PartExample.IndexOf("-") || PartExample.IndexOf("-") < 0) && PartExample.Contains("+")))
                    number = DecisionByOperators("+", PartExample, index);
            }
            return (int)number;
        }
        private double DecisionByOperators(string operator1, List<string> PartExample, int index)
        {
            double number = 0;
            index = PartExample.IndexOf(operator1);
            if (operator1 == "/")
                number = Convert.ToDouble(PartExample[index - 1]) / Convert.ToDouble(PartExample[index + 1]);
            else if (operator1 == "*")
                number = Convert.ToDouble(PartExample[index - 1]) * Convert.ToDouble(PartExample[index + 1]);
            else if (operator1 == "-")
                number = Convert.ToDouble(PartExample[index - 1]) - Convert.ToDouble(PartExample[index + 1]);
            else if (operator1 == "+")
                number = Convert.ToDouble(PartExample[index - 1]) + Convert.ToDouble(PartExample[index + 1]);
            PartExample[index + 1] = number + "";
            PartExample.RemoveAt(index - 1);
            PartExample.RemoveAt(index - 1);
            return number;
        }
    }
}
