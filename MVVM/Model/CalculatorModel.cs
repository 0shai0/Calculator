using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Calculator.MVVM.Model
{
    public class CalculatorModel
    {
        public object Calculation(string expression)
        {
            var dataTable = new DataTable();
            var result = Convert.ToDouble(dataTable.Compute(expression, string.Empty));

            // 지수 표기법으로 변환 (숫자가 매우 클 때만), 아니라면 정수 결과 처리
            return result > 1e9 || result < -1e9 ? result.ToString("E") : (result % 1 == 0 ? (int)result : result);
        }
    }
}
