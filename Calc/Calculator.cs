using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class Calculator
    {
        public string CurrentExpression { get; private set; } = "";
        public string HistoryDisplay { get; private set; } = "";
        public bool IsSandwichBtnClk { get; set; }

        public void UpdateState(string newExpression, string newHistory)
        {
            CurrentExpression = newExpression;
            HistoryDisplay = newHistory;
        }
        public void Clear()
        {
            CurrentExpression = "";
            HistoryDisplay = "";
        }

        public string CalculateBasicExpression(string expression)
        {
            try
            {
                string preparedExpression = expression.Replace('×', '*').Replace('÷', '/').Replace('−', '-');

                var dataTable = new DataTable();
                var value = dataTable.Compute(preparedExpression, string.Empty);

                return value.ToString();
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public string CalculateSqrt(string numberStr)
        {
            try
            {
                if (double.TryParse(numberStr, out double number))
                {
                    if (number < 0)
                    {
                        return "Error";
                    }
                    double result = Math.Sqrt(number);
                    return result.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    return "Помилка введення!";
                }
            }
            catch (Exception)
            {
                return "Error";
            }
        }
        public string CalculateLn(string numberStr)
        {
            try
            {
                if (double.TryParse(numberStr, out double number))
                {
                    if (number <= 0)
                    {
                        return "Error";
                    }
                    double result = Math.Log(number);
                    return result.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    return "Помилка введення!";
                }
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public string CalculatePower(string expression)
        {
            try
            {
                string cleanedExpression = expression;
                string[] parts = cleanedExpression.Split('^');

                if (parts.Length != 2)
                {
                    return "Невірний формат степеня!";
                }

                if (double.TryParse(parts[0], out double baseNumber) &&
                    double.TryParse(parts[1], out double exponent))
                {
                    double result = Math.Pow(baseNumber, exponent);
                    return result.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    return "Помилка введення чисел!";
                }
            }
            catch (Exception)
            {
                return "Error";
            }
        }
    }
}
