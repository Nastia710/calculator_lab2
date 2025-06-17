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

        public Calculator()
        {
            Expression = "";
            DisplayText = "0";
            IsNewOp = true;
            IsError = false;
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
            
        }

        public void Redo()
        {
            
        }
    }
}
