using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Calculator _calculator;
        private CommandManager _commandManager;

        private bool _isOperatorAdded = false;
        private bool _isResultDisplayed = false;
        private bool _isPowerOperationPending = false;

        private string _currentExpressionText;
        public string CurrentExpressionText
        {
            get => _currentExpressionText;
            set
            {
                if (_currentExpressionText != value)
                {
                    _currentExpressionText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _historyDisplayText;
        public string HistoryDisplayText
        {
            get => _historyDisplayText;
            set
            {
                if (_historyDisplayText != value)
                {
                    _historyDisplayText = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();

            _calculator = new Calculator();
            _commandManager = new CommandManager();

            DataContext = this;

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            CurrentExpressionText = _calculator.CurrentExpression;
            HistoryDisplayText = _calculator.HistoryDisplay;
        }

        private void ExecuteAndSaveCommand(ICommand command)
        {
            _commandManager.ExecuteCommand(command);
            UpdateDisplay();
        }

        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            string digit = (sender as Button).Content.ToString();
            string currentExpr = _calculator.CurrentExpression;
            string currentHist = _calculator.HistoryDisplay;

            if (_isResultDisplayed)
            {
                currentExpr = digit;
                currentHist = "";
                _isResultDisplayed = false;
            }
            else
            {
                currentExpr += digit;
            }

            _isOperatorAdded = false;
            ExecuteAndSaveCommand(new ChangeCommand(_calculator, currentExpr, currentHist));
        }

        private void DotBtn_Click(object sender, RoutedEventArgs e)
        {
            string currentExpr = _calculator.CurrentExpression;
            string currentHist = _calculator.HistoryDisplay;

            if (_isResultDisplayed || string.IsNullOrEmpty(currentExpr) || currentExpr.Contains("."))
            {
                currentExpr = "0.";
                currentHist = "";
                _isResultDisplayed = false;
            }
            else if (currentExpr.EndsWith("."))
            {
                return;
            }
            else
            {
                currentExpr += ".";
            }

            _isOperatorAdded = false;
            ExecuteAndSaveCommand(new ChangeCommand(_calculator, currentExpr, currentHist));
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            string operation = (sender as Button).Content.ToString();
            string currentExpr = _calculator.CurrentExpression;
            string currentHist = _calculator.HistoryDisplay;

            if (string.IsNullOrEmpty(currentExpr) && !string.IsNullOrEmpty(currentHist))
            {
                currentExpr = currentHist;
                currentHist = "";
            }
            else if (string.IsNullOrEmpty(currentExpr))
            {
                return;
            }

            if (_isOperatorAdded)
            {
                currentExpr = currentExpr.Substring(0, currentExpr.Length - 1) + operation;
            }
            else
            {
                currentExpr += operation;
            }

            _isResultDisplayed = false;
            _isOperatorAdded = true;
            ExecuteAndSaveCommand(new ChangeCommand(_calculator, currentExpr, currentHist));
        }

        private void EqualsBtn_Click(object sender, RoutedEventArgs e)
        {
            string currentExpr = _calculator.CurrentExpression;
            string currentHist = _calculator.HistoryDisplay;

            if (string.IsNullOrEmpty(currentExpr))
            {
                return;
            }

            string result;
            if (_isPowerOperationPending)
            {
                result = _calculator.CalculatePower(currentExpr);
                _isPowerOperationPending = false;
            }
            else
            {
                result = _calculator.CalculateBasicExpression(currentExpr);
            }

            ExecuteAndSaveCommand(new ChangeCommand(_calculator, result, currentExpr + "="));
            _isResultDisplayed = true;
            _isOperatorAdded = false;
        }

        private void CBtn_Click(object sender, RoutedEventArgs e)
        {
            ExecuteAndSaveCommand(new ChangeCommand(_calculator, "", ""));
            _isResultDisplayed = false;
            _isOperatorAdded = false;
            _commandManager.ClearHistory();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            _commandManager.Undo();
            UpdateDisplay();
            _isResultDisplayed = false;
        }

        private void RedoBtn_Click(object sender, RoutedEventArgs e)
        {
            _commandManager.Redo();
            UpdateDisplay();
            _isResultDisplayed = false;
        }

        private void BackspaceBtn_Click(object sender, RoutedEventArgs e)
        {
            string currentExpr = _calculator.CurrentExpression;
            string currentHist = _calculator.HistoryDisplay;

            if (_isResultDisplayed)
            {
                currentExpr = "";
                currentHist = "";
                _isResultDisplayed = false;
            }
            else if (!string.IsNullOrEmpty(currentExpr))
            {
                currentExpr = currentExpr.Substring(0, currentExpr.Length - 1);
            }

            _isOperatorAdded = false;
            ExecuteAndSaveCommand(new ChangeCommand(_calculator, currentExpr, currentHist));
        }

        private void Extended_Click(object sender, RoutedEventArgs e)
        {
            string operation = (sender as Button).Content.ToString();
            string currentExpr = _calculator.CurrentExpression;
            string currentHist = _calculator.HistoryDisplay;
            string result = "";

            if (currentExpr == "Недійсне введення!" || currentExpr == "Error" || currentExpr == "Невірний формат степеня!" || currentExpr == "Помилка обчислення!" || currentExpr == "Помилка обчислення степеня!")
            {
                currentExpr = "";
                currentHist = "";
            }

            if (string.IsNullOrEmpty(currentExpr) && operation != "xⁿ")
            {
                return;
            }

            _isResultDisplayed = false;

            if (operation == "√")
            {
                result = _calculator.CalculateSqrt(currentExpr);
                ExecuteAndSaveCommand(new ChangeCommand(_calculator, result, "√(" + currentExpr + ")"));
                _isResultDisplayed = true;
                _isOperatorAdded = false;
                _isPowerOperationPending = false;
            }
            else if (operation == "ln")
            {
                result = _calculator.CalculateLn(currentExpr);
                ExecuteAndSaveCommand(new ChangeCommand(_calculator, result, "ln(" + currentExpr + ")"));
                _isResultDisplayed = true;
                _isOperatorAdded = false;
                _isPowerOperationPending = false;
            }
            else if (operation == "xⁿ")
            {
                if (currentExpr.Contains("^"))
                {
                    return;
                }

                currentExpr += "^";
                _isOperatorAdded = true;
                _isPowerOperationPending = true;
                ExecuteAndSaveCommand(new ChangeCommand(_calculator, currentExpr, currentHist));
            }
        }

        private void SandwichButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_calculator.IsSandwichBtnClk)
            {
                grid.ColumnDefinitions[4].Width = new GridLength(1, GridUnitType.Star);
                Width += 90;
                SandwichButton.Visibility = Visibility.Collapsed;
                CloseSandwichButton.Visibility = Visibility.Visible;

                foreach (UIElement element in grid.Children)
                {
                    if (Grid.GetColumn(element) == 4)
                    {
                        element.Visibility = Visibility.Visible;
                    }
                }
                _calculator.IsSandwichBtnClk = true;
            }
            else
            {
                grid.ColumnDefinitions[4].Width = new GridLength(0);
                Width -= 90;
                SandwichButton.Visibility = Visibility.Visible;
                CloseSandwichButton.Visibility = Visibility.Collapsed;

                foreach (UIElement element in grid.Children)
                {
                    if (Grid.GetColumn(element) == 4)
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
                _calculator.IsSandwichBtnClk = false;
            }
        }
    }
}