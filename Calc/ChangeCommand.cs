using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calc
{
    public class ChangeCommand : ICommand
    {
        private readonly Calculator _calculator;
        private readonly string _previousExpression;
        private readonly string _previousHistory;
        private readonly string _newExpression;
        private readonly string _newHistory;

        public ChangeCommand(Calculator calculator, string newExpr, string newHist)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _previousExpression = calculator.CurrentExpression;
            _previousHistory = calculator.HistoryDisplay;
            _newExpression = newExpr;
            _newHistory = newHist;
        }

        public void Execute()
        {
            _calculator.UpdateState(_newExpression, _newHistory);
        }

        public void Unexecute()
        {
            _calculator.UpdateState(_previousExpression, _previousHistory);
        }
    }
}
