using System.Data.Common;
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
using static Calc.Command;

namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calculator;

        public MainWindow()
        {
            InitializeComponent();

            calculator = new Calculator();

            Update();
        }

        private void SandwichButton_Click(object sender, RoutedEventArgs e)
        {
            if (!calculator.IsSandwichBtnClk)
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
                calculator.IsSandwichBtnClk = true;
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
                calculator.IsSandwichBtnClk = false;
            }
        }

        private void Update()
        {
            waitingDisplay.Text = calculator.Expression;
            display.Text = calculator.DisplayText;
        }

        private void CBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.Work(new ClearCommand(calculator));
            Update();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            calculator.Undo();
            Update();
        }

        private void RedoBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.Redo();
            Update();
        }

        private void BackspaceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (calculator.IsError)
            {
                return;
            }
            if (!calculator.IsNewOp && calculator.DisplayText.Length > 0)
            {
                var command = new BackSpace(calculator);
                calculator.Work(command);
                Update();
            }
        }

        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            string digit = (sender as Button).Content.ToString();
            Command command = new EnterDigit(calculator, digit);
            calculator.Work(command);
            Update();
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            string op = (sender as Button).Content.ToString();

            try
            {
                if (!calculator.IsNewOp && !string.IsNullOrEmpty(calculator.Expression))
                {
                    CalcRes();
                }

                Command command = new EnterOperation(calculator, op);
                calculator.Work(command);
            }
            catch
            {
                calculator.DisplayError();
            }

            Update();
        }

        private void Extended_Click(object sender, RoutedEventArgs e)
        {
            string ext = (sender as Button).Content.ToString();

            try
            {
                calculator.Work(new EnterOperation(calculator, ext));
            }
            catch
            {
                calculator.DisplayError();
            }

            Update();
        }

        private void EqualsBtn_Click(object sender, RoutedEventArgs e)
        {
            CalcRes();
        }

        private void DotBtn_Click(object sender, RoutedEventArgs e)
        {
            calculator.Work(new DotCommand(calculator));
            Update();
        }

        void CalcRes()
        {
            if (calculator.IsError || string.IsNullOrEmpty(calculator.Expression) || calculator.IsNewOp)
            {
                return;
            }

            try
            {
                var command = new CalculateComand(calculator);
                calculator.Work(command);
                Update();
            }
            catch
            {
                calculator.DisplayError();
                Update();
            }
        }
    }
}