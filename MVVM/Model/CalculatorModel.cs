using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MVVM.Model
{
    public class CalculatorModel
    {
        public object Calculation(string expression)
        {
            var dataTable = new System.Data.DataTable();
            var result = Convert.ToDouble(dataTable.Compute(expression, string.Empty));

            return result % 1 == 0 ? (int)result : result;
        }
    }
}
