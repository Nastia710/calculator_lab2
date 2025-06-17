using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public abstract class Command(Calculator calculator)
    {
        string prevExpression = calculator.Expression;
        string prevValue = calculator.DisplayText;

        protected double operand;

        public abstract void Execute();

        public virtual void Undo()
        {
            calculator.Expression = prevExpression;
            calculator.DisplayText = prevValue;
        }

        public class BackSpace(Calculator calculator) : Command(calculator)
        {
            public override void Execute()
            {
                calculator.Backspace();
            }
        }

        public class EnterDigit(Calculator calculator, string digit) : Command(calculator)
        {
            public override void Execute()
            {
                calculator.AddNumber(digit);
            }
        }

        public class DotCommand(Calculator calculator) : Command(calculator)
        {
            public override void Execute()
            {
                calculator.AddDot();
            }
        }

        public class ClearCommand(Calculator calculator) : Command(calculator)
        {
            public override void Execute()
            {
                calculator.Clear();
            }
        }

        public class EnterOperation(Calculator calculator, string op) : Command(calculator)
        {
            public override void Execute()
            {
                calculator.AddOp(op);
            }
        }

        public class CalculateComand(Calculator calculator) : Command(calculator)
        {
            private double firstOp;
            private double secondOp;
            private string operation;

            public override void Execute()
            {
                calculator.Calculate(out firstOp, out secondOp, out operation);

                double result = operation switch
                {
                    "+" => firstOp + secondOp,
                    "-" => firstOp - secondOp,
                    "×" => firstOp * secondOp,
                    "÷" => firstOp / secondOp,
                    "√" => Math.Sqrt(firstOp),
                    "xⁿ" => Math.Pow(firstOp, secondOp),
                    "ln" => Math.Log(firstOp),
                    _ => throw new ArgumentException("Invalid operation")
                };

                calculator.DisplayText = result.ToString();
                calculator.Expression = "";
                calculator.IsNewOp = true;
            }

            /*public override void Undo()
            {
                base.Undo();
                calculator.IsNewOp = false;
            }*/
        }
    }
}
