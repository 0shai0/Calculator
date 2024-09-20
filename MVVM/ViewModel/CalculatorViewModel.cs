using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Calculator.MVVM.Model;

namespace Calculator.MVVM.ViewModel
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _formulaAndResult;
        private readonly CalculatorModel _model;

        public CalculatorViewModel()
        {
            _model = new CalculatorModel();
            FormulaAndResult = string.Empty;
        }

        public string FormulaAndResult
        {
            get => _formulaAndResult;
            set
            {
                _formulaAndResult = value;
                OnPropertyChanged();
            }
        }


        // 클릭 이벤트, 키보드 입력 시 FormulaAndResult에 추가
        // FormulaAndResult[^1]는 char이기 때문에 ""이 아닌 ''로 작성

        public void UpdateFormula(string input)
        {
            // 연산자 리스트 정의
            string operators = "+-×÷()";

            // FormulaAndResult에서 마지막으로 사용된 operators의 위치가 어디가 마지막인지 확인
            int lastOperatorIndex = FormulaAndResult.LastIndexOfAny(operators.ToCharArray());

            bool isEmpty = FormulaAndResult.Length == 0;
            bool isInputNumber = char.IsDigit(input[0]);
            bool isInputOperator = operators.Contains(input);
            bool isLastNumber = FormulaAndResult.Length > 0 && char.IsDigit(FormulaAndResult[^1]);
            bool isLastOperator = FormulaAndResult.Length > 0 && operators.Contains(FormulaAndResult[^1]);
            bool isLastZero = FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '0';
            bool isLastParenthesisClose = FormulaAndResult.Length > 0 && FormulaAndResult[^1] == ')';
            bool isLastDot = FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '.';
            bool unmatchedParentheses = FormulaAndResult.Count(c => c == '(') != FormulaAndResult.Count(c => c == ')');
            bool isParenthesesBalancedOrOpenLess = FormulaAndResult.Count(c => c == '(') <= FormulaAndResult.Count(c => c == ')');
            bool isSingleOrSecondLastIsOperator = FormulaAndResult.Length == 1 || (FormulaAndResult.Length > 1 && operators.Contains(FormulaAndResult[^2]));
            bool dotExists = (lastOperatorIndex == -1 && FormulaAndResult.Contains(".")) ||
                             (lastOperatorIndex != -1 && FormulaAndResult.Substring(lastOperatorIndex + 1).Contains("."));




            // '('와 '-'가 맨 앞에 입력되는 경우
            if (isEmpty && (input == "(" || input == "-"))
            {
                FormulaAndResult += input;
                return;
            }


            // '.'이 맨 앞에 올 경우
            if (isEmpty && (isInputOperator || input == "."))
            {
                return;
            }


            // 마지막이 0이고 그 앞에 다른 숫자가 있는 경우에는 0 추가 입력을 허용
            if (isLastZero && isInputNumber && input == "0" && !isSingleOrSecondLastIsOperator)
            {
                FormulaAndResult += input;
            }


            // 0 뒤에 0이 올 경우
            if (isLastZero && input == "0")
            {
                return;
            }


            // 마지막이 0이고 0 앞에 다른 숫자가 없는데 숫자가 입력될 경우
            if (isLastZero && isInputNumber && isSingleOrSecondLastIsOperator)
            {
                FormulaAndResult = FormulaAndResult.Remove(FormulaAndResult.Length - 1);
                FormulaAndResult += input;
                return;
            }


            // '.' 뒤에 '.'과 연산자가 오는 경우
            if (isLastDot && isInputOperator && input == ".")
            {
                return;
            }

            // 마지막 문자가 연산자인 경우
            if (isLastOperator)
            {
                // 마지막이 ')'로 끝나고 다음에 입력되는 것이 '('일 경우
                if (isLastParenthesisClose && input == "(")
                {
                    FormulaAndResult += "×" + input;
                    return;
                }

                // 마지막이 ')'로 끝나고 다음에 입력되는 것이 숫자인 경우
                if (isLastParenthesisClose && isInputNumber)
                {
                    FormulaAndResult += "×" + input;
                    return;
                }

                // 연산자 뒤에 연산자가 입력될 경우
                if (isInputOperator && input != "(")
                {
                    return;
                }

                // '(' 뒤에 '('이 입력될 경우 
                if (isLastOperator && input == "(")
                {
                    return;
                }

                // 연산자 바로 뒤에 '.'이 입력될 경우
                if (input == ".")
                {
                    return;
                }
            }


            // '.'이 입력될 경우
            if (input == ".")
            {
                // '.'이 존재하지 않으면 추가
                if (!dotExists)
                {
                    FormulaAndResult += input;
                }
                else
                {
                    return;
                }
            }


            // 숫자 입력 추가
            if (isInputNumber)
            {
                FormulaAndResult += input;
            }

            // 마지막이 숫자이고 마무리되지 않은 괄호가 있는 경우
            if (isLastNumber && unmatchedParentheses)
            {
                if (input == "(")
                {
                    return;
                }
            }

            // 시작되는 '('가 없는 상태에서 숫자 뒤에 ')'가 온 경우
            if (isLastNumber && input == ")" && isParenthesesBalancedOrOpenLess)
            {
                return;
            }

            // 숫자가 나오고 '('가 올 경우
            if (isLastNumber && input == "(")
            {
                FormulaAndResult += "×";
            }

            // 연산자 입력 추가
            if (isInputOperator)
            {
                FormulaAndResult += input;
            }
        }




        // backSpace 클릭, 키보드 입력 시 FormulaAndResult에서 제일 뒤에 있는 한 글자 삭제

        public void DeleteFormula()
        {
            if (!string.IsNullOrEmpty(FormulaAndResult))
            {
                FormulaAndResult = FormulaAndResult.Substring(0, FormulaAndResult.Length - 1);
            }
        }


        // '=' 버튼 클릭 시 결과값을 FormulaAndResult에 저장

        public void CalculateResult()
        {
            try
            {
                string operators = "+-×÷()";

                // 수식에서 연산자 변환
                string modifiedFormula = FormulaAndResult.Replace('×', '*').Replace('÷', '/');

                bool isEmpty = FormulaAndResult.Length == 0;

                // 괄호가 열려있다면 닫는 괄호 추가
                if (FormulaAndResult.Count(c => c == '(') > FormulaAndResult.Count(c => c == ')'))
                {
                    FormulaAndResult += ")";
                }

                // '='이 맨 앞에 입력될 경우
                if (isEmpty)
                {
                    return;
                }

                // 연산자 뒤에 '='을 입력될 경우
                if (operators.Contains(FormulaAndResult[^1]))
                {
                    return;
                }
                

                // 수식을 계산
                FormulaAndResult = _model.Calculation(modifiedFormula).ToString();



                // 연산자 우선순위대로 처리하는 코드 작성하기
                // -곱하기 -는 +로 변환, 계산
                // 소수점 계산
            }
            catch
            {
                FormulaAndResult = "Error";
            }
        }


        // 'C' 버튼 클릭 시 FormulaAndResult에 빈 문자열로 초기화

        public void Clear()
        {
            FormulaAndResult = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
