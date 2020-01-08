using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeInterestingStuff.ReversePolishNotation
{
    /// This class created using information about Reverse Polish notation from https://habr.com/ru/post/100869/
    public class RPN
    {
        private string expression;
        private List<string> standart_operators;

        public RPN(string incomingExpression)
        {
            standart_operators = new List<string>(new string[] { "(", ")", "+", "-", "*", "/", "^", "pow", "ln", "sqrt" });
            expression = incomingExpression;
        }

        /// <summary>
        /// Returns result of expression.
        /// </summary>
        public string GetResult()
        {
            string result;
            try
            {
                PrepareExpression();
                result = CalculateExpression().ToString();
            }
            catch (FormatException e)
            {
                return "Error handling the expression. \n Message:\n" + e.Message;
            }
            return result;
        }
        /// <summary>
        /// Returns standart expression in Reverse Polish notation.
        /// </summary>
        public string ConvertExpressionToRPN()
        {
            Queue<string> convertedExpression = new Queue<string>(ConvertToReversePolishNotation());
            StringBuilder result = new StringBuilder();
            while (convertedExpression.Count > 0)
            {
                result.Append(convertedExpression.Dequeue());
            }
            return result.ToString();
        }
        ///<summary>
        ///Preparing the expression deleting all Spaces
        ///</summary>
        private void PrepareExpression()
        {
            expression = expression.Replace(" ", string.Empty);
        }
        /// <summary>
        /// Checking if Expression contains only correct characters
        /// </summary>
        private void CheckExpression(List<string> separatedExpression)
        {
            int bracketsCount = 0;
            foreach (string s in separatedExpression)
            {
                if (double.TryParse(s, out double i) || standart_operators.Contains(s.ToString()))
                {
                    if (String.Equals(s, '('))
                    {
                        bracketsCount++;
                    }
                    if (String.Equals(s, ')'))
                    {
                        bracketsCount--;
                    }
                }
                else
                {
                    throw new FormatException("Wrong expression format. Please check your input.");
                }
            }
            if (bracketsCount != 0)
            {
                throw new FormatException("Wrong expression format. Unacceptable count of brackets.");
            }
        }
        /// <summary>
        /// Separating expression in a List of strings
        /// </summary>
        private IEnumerable<string> SeparateExpression()
        {
            int pos = 0;
            while (pos < expression.Length)
            {
                string s = string.Empty + expression[pos];
                if (!standart_operators.Contains(expression[pos].ToString()))
                {
                    if (Char.IsDigit(expression[pos]))
                    {
                        for (int i = pos + 1; i < expression.Length &&
                            (Char.IsDigit(expression[i]) || expression[i] == ',' || expression[i] == '.'); i++)
                        {
                            s += expression[i];
                        }
                    }
                    else if (Char.IsLetter(expression[pos]))
                    {
                        for (int i = pos + 1; i < expression.Length && Char.IsLetter(expression[i]); i++)
                        {
                            s += expression[i];
                        }
                    }
                }
                yield return s;
                pos += s.Length;
            }
        }
        /// <summary>
        /// Converts standart expression in Reverse Polish notation.
        /// </summary>
        private List<string> ConvertToReversePolishNotation()
        {
            List<string> separatedExpression = SeparateExpression().ToList();
            CheckExpression(separatedExpression);

            List<string> California = new List<string>();
            Stack<string> Texas = new Stack<string>();

            foreach (string c in separatedExpression)
            {
                //Если это оператор
                if (standart_operators.Contains(c))
                {
                    //Если стек команд не пустой и команда не "("
                    if (Texas.Count > 0 && !c.Equals("("))
                    {
                        //Перекладываем команды в очередь, пока не наткнемся на "(".
                        //"(" и ")" в очередь не пишутся, удаляем
                        if (c.Equals(")"))
                        {
                            string s = Texas.Pop();
                            while (s != "(")
                            {
                                //Если уперлись в дно стека, а "(" так и не нашли
                                if (Texas.Count < 1)
                                {
                                    throw new FormatException("Wrong expression format. Somthing wrong with brackits.");
                                }
                                California.Add(s);
                                s = Texas.Pop();
                            }
                        }
                        //Если приоритет команды Выше того, что сверху в стеке, пушим в стек
                        else if (GetActionPriority(c) > GetActionPriority(Texas.Peek()))
                        {
                            Texas.Push(c);
                        }
                        //Иначе вынимаем все команды в очередь пока приоритет команды не будет Выше той, что в стеке,
                        // либо пока не встретим дно стека. Потом пушим.
                        else
                        {
                            while (Texas.Count > 0 && GetActionPriority(c) <= GetActionPriority(Texas.Peek()))
                            {
                                California.Add(Texas.Pop());
                            }
                            Texas.Push(c);
                        }
                    }
                    //Если хочется положить ")" в пустой стек
                    else if (Texas.Count < 1 && c.Equals(")"))
                    {
                        throw new FormatException("Wrong expression format. Somthing wrong with brackits.");
                    }
                    //Если стек пуст или это "("
                    else
                        Texas.Push(c);
                }
                //Если это не оператор (число)
                else if (double.TryParse(c, out double i))
                {
                    // It's a number!
                    California.Add(c);
                }
                else
                {
                    throw new FormatException("Wrong expression format. One of numbers is wrong or one of operations is unsupported");
                }
            }
            //Вынимаем оставшиеся команды из стека в очередь
            if (Texas.Count > 0)
            {
                foreach (string c in Texas)
                {
                    California.Add(c);
                }
            }
            //Возвращаем очередь.
            return California;
        }
        /// <summary>
        /// Returns operation priorities for Convertor
        /// </summary>
        private byte GetActionPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                case "pow":
                case "sqrt":
                    return 3;
                case "ln":
                    return 4;
                default:
                    return 5;
            }
        }
        /// <summary>
        /// Calculates expression in RPN and returns the result.
        /// </summary>
        private double CalculateExpression()
        {
            Stack<string> stack = new Stack<string>();
            Queue<string> chain = new Queue<string>(ConvertToReversePolishNotation());

            while (chain.Count > 0)
            {
                string queueElement = chain.Dequeue();
                //Если число - пушим в стек.
                if (!standart_operators.Contains(queueElement))
                {
                    stack.Push(queueElement);
                }
                //Иначе выбираем что вычислять и делаем.
                else
                {
                    double result = 0;
                    switch (queueElement)
                    {

                        case "+":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                result = b + a;
                                break;
                            }
                        case "-":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                result = b - a;
                                break;
                            }
                        case "*":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                result = b * a;
                                break;
                            }
                        case "/":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                result = b / a;
                                break;
                            }
                        case "^":
                        case "pow":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                result = Math.Pow(b, a);
                                break;
                            }
                        case "sqrt":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                double b = Convert.ToDouble(stack.Pop());
                                result = Math.Pow(b, (1 / a));
                                break;
                            }
                        case "ln":
                            {
                                double a = Convert.ToDouble(stack.Pop());
                                result = Math.Log(a);
                                break;
                            }
                    }
                    stack.Push(result.ToString());
                }
            }
            return Convert.ToDouble(stack.Pop());
        }
    }
}
