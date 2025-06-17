using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class Calculator
    {
        public string Expression { get; set; }
        public string DisplayText { get; set; }
        public bool IsNewOp { get; set; }
        public bool IsError { get; set; }
        public bool IsSandwichBtnClk { get; set; }
        ControlPanel controlPanel;

        public Calculator()
        {
            Expression = "";
            DisplayText = "0";
            IsNewOp = true;
            IsError = false;
            controlPanel = new ControlPanel();
        }

        public void DisplayError()
        {
            IsError = false;
            Clear();
        }

        public void ResetError()
        {
            Expression = "";
            DisplayText = "Error";
            IsNewOp = true;
            IsError = true;
        }

        public void Clear()
        {
            Expression = "";
            DisplayText = "0";
            IsNewOp = true;
        }

        public void Work(Command command)
        {
            controlPanel.ExecuteCommand(command);
        }

        public void Backspace()
        {
            if (IsError)
            {
                ResetError();
                return;
            }
            if (DisplayText.Length == 1 || (DisplayText.Length == 2 && DisplayText[0] == '-'))
            {
                DisplayText = "0";
                IsNewOp = true;
            }
            else
            {
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
            }
        }

        public void AddNumber(string number)
        {
            if (IsError)
            {
                ResetError();
            }
            if (IsNewOp)
            {
                DisplayText = number;
                IsNewOp = false;
            }
            else
            {
                if (DisplayText == "0" && number != ".")
                {
                    DisplayText = number;
                }
                else
                {
                    DisplayText += number;
                }
            }
        }

        public void AddDot()
        {
            if (IsError)
            {
                ResetError();
            }
            if (IsNewOp)
            {
                DisplayText = "0.";
                IsNewOp = false;
            }
            else if (!DisplayText.Contains("."))
            {
                DisplayText += ".";
            }
        }

        public void AddOp(string op)
        {
            if (IsError)
            {
                ResetError();
            }
            Expression = $"{DisplayText} {op}";
            IsNewOp = true;
        }

        public void Undo()
        {
            controlPanel.Undo();
        }

        public void Redo()
        {
            controlPanel.Redo();
        }

        public void Calculate(out double firstOperand, out double secondOperand, out string op)
        {
            string[] expression = Expression.Split(' ');

            if (!double.TryParse(expression[0], out firstOperand) || !double.TryParse(DisplayText, out secondOperand))
            {
                throw new Exception("Invalid number format!");
            }

            op = expression[1];

            switch (op)
            {
                case "+":
                case "-":
                case "×":
                case "÷":
                    if (op == "÷" && secondOperand == 0)
                    {
                        throw new DivideByZeroException();
                    }
                    break;
                case "√":
                    if (firstOperand < 0)
                    {
                        throw new ArgumentException("Cannot calculate square root of negative number");
                    }
                    break;
                case "xⁿ":
                    break;
                case "ln":
                    if (firstOperand <= 0)
                    {
                        throw new ArgumentException("Cannot calculate logarithm of non-positive number");
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid operation");
            }
        }
    }
}
