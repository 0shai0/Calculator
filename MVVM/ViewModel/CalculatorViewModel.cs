using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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
        private string _calculationHistory;

        public CalculatorViewModel()
        {
            _model = new CalculatorModel();
            FormulaAndResult = string.Empty;
        }


        public string CalculationHistory
        {
            get => _calculationHistory;
            set
            {
                _calculationHistory = value;
                OnPropertyChanged();
            }
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


        // 여러 메서드에서 공통적으로 사용되는 변수를 상수, 메서드로 변경

        // 연산자 리스트를 상수로 정의
        private const string operators = "+-×÷()";

        // 마지막 문자가 연산자일 경우
        private bool IsLastCharOperator(string operators) =>
            FormulaAndResult.Length > 0 && operators.Contains(FormulaAndResult[^1]);

        // 마지막 문자가 ')'일 경우
        private bool IsLastCharParenthesisClose() =>
            FormulaAndResult.Length > 0 && FormulaAndResult[^1] == ')';

        // FormulaAndResult의 '('의 개수가 ')'의 개수보다 많을 경우
        private bool IsOpeningParenthesisExcess() =>
            FormulaAndResult.Count(c => c == '(') > FormulaAndResult.Count(c => c == ')');

        // 입력 값을 FormulaAndResult에 추가하는 메서드
        private void Add(string input)
        {
            FormulaAndResult += input;
        }


        // 클릭 이벤트, 키보드 입력 시 FormulaAndResult에 추가
        // FormulaAndResult[^1]는 char이기 때문에 ""이 아닌 ''로 작성
        // FormulaAndResult.Length > 0를 확인하는 이유는 빈 문자열인 상태에서 마지막 문자를 확인할 경우 오류가 발생하기 때문

        public void UpdateFormula(string input)
        {
            // FormulaAndResult가 빈 문자열일 경우
            bool isEmpty = FormulaAndResult.Length == 0;

            // 숫자가 입력될 경우
            bool isInputNumber = char.IsDigit(input[0]);

            // 연산자가 입력될 경우
            bool isInputOperator = operators.Contains(input);

            // 마지막 문자가 숫자일 경우
            bool isLastCharNumber = FormulaAndResult.Length > 0 && char.IsDigit(FormulaAndResult[^1]);

            // 마지막 문자가 숫자 '0'일 경우
            bool isLastCharZero = FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '0';

            // 마지막 문자가 '('일 경우
            bool isLastCharParenthesisOpen = FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '(';

            // 마지막 문자가 '.'일 경우
            bool isLastCharDot = FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '.';

            // FormulaAndResult의 '('와 ')'의 개수가 일치하지 않을 경우
            bool unmatchedParentheses = FormulaAndResult.Count(c => c == '(') != FormulaAndResult.Count(c => c == ')');

            // FormulaAndResult의 '('의 개수가 ')'의 개수보다 많거나 같을 경우
            bool isParenthesesBalancedOrOpenLess = FormulaAndResult.Count(c => c == '(') <= FormulaAndResult.Count(c => c == ')');

            // FormulaAndResult의 길이가 1이거나 길이가 1보다 크면서 마지막 문자 앞이 연산자일 경우
            bool isSingleOrSecondLastIsOperator = FormulaAndResult.Length == 1 || (FormulaAndResult.Length > 1 && operators.Contains(FormulaAndResult[^2]));

            // FormulaAndResult에서 마지막으로 사용된 operators의 위치가 어디가 마지막인지 확인
            int lastOperatorIndex = FormulaAndResult.LastIndexOfAny(operators.ToCharArray());

            // 연산자가 없으면서 '.'이 있는 경우이거나 연산자가 있으면서 마지막 연산자 이후에 '.'이 있을 경우
            bool dotExists = (lastOperatorIndex == -1 && FormulaAndResult.Contains(".")) ||
                             (lastOperatorIndex != -1 && FormulaAndResult.Substring(lastOperatorIndex + 1).Contains("."));


            // FormulaAndResul가 Error이면서 숫자나 연산자, '.'을 입력할 경우
            if (FormulaAndResult == "Error" && (isInputNumber || isInputOperator || input == "."))
            {
                if (input == "+" || input == "×" || input == "÷" || input == ")" || input == ".")
                {
                    Clear();
                    return;
                }
                else
                {
                    Clear();
                }
            }


            // 빈 문자열인 상태에서 '('이나 '-'가 입력할 경우
            if (isEmpty && (input == "(" || input == "-"))
            {
                Add(input);
                return;
            }


            // 빈 문자열인 상태에서 연산자나 '.'을 입력할 경우
            if (isEmpty && (isInputOperator || input == "."))
            {
                return;
            }


            // 마지막 문자가 숫자 '0'이면서 그 앞에 다른 숫자가 있는 경우에 숫자 '0'을 입력할 경우
            if (isLastCharZero && !isSingleOrSecondLastIsOperator && input == "0")
            {
                Add(input);
            }


            // 마지막 문자가 숫자 '0'이고 숫자 '0'이 입력할 경우
            if (isLastCharZero && input == "0")
            {
                return;
            }


            // 마지막 문자가 숫자 '0'이고 '0'만 있거나 '0' 앞에 연산자가 있는 경우에 숫자를 입력할 경우
            if (isLastCharZero && isSingleOrSecondLastIsOperator && isInputNumber)
            {
                FormulaAndResult = FormulaAndResult.Remove(FormulaAndResult.Length - 1);
                Add(input);
                return;
            }


            // 마지막 문자가 '.'이고 연산자나 '.'를 입력할 경우
            if (isLastCharDot && (isInputOperator || input == "."))
            {
                return;
            }


            // 마지막 문자가 연산자이면서 그 연산자가 '('이고 '-'를 입력할 경우
            if (IsLastCharOperator(operators) && isLastCharParenthesisOpen && input == "-")
            {
                Add(input);
                return;
            }


            // 마지막 문자가 연산자이면서 그 연산자가 ')'이고 '('를 입력할 경우
            if (IsLastCharOperator(operators) && IsLastCharParenthesisClose() && input == "(")
            {
                FormulaAndResult += "×" + input;
                return;
            }


            // 마지막 문자가 연산자이면서 그 연산자가 ')'이고 숫자를 입력할 경우
            if (IsLastCharOperator(operators) && IsLastCharParenthesisClose() && isInputNumber)
            {
                FormulaAndResult += "×" + input;
                return;
            }


            // 마지막 문자가 연산자이면서 그 연산자가 ')'이고 ')'이외의 연산자를 입력할 경우
            if (IsLastCharOperator(operators) && IsLastCharParenthesisClose() && input != ")" && isInputOperator)
            {
                Add(input);
                return;
            }


            // 마지막 문자가 연산자이면서 '(' 이외의 연산자를 입력할 경우
            if (IsLastCharOperator(operators) && input != "(" && isInputOperator)
            {
                return;
            }


            // 마지막 문자가 연산자이면서 그 연산자가 '('이고 '('을 입력할 경우 
            if (IsLastCharOperator(operators) && isLastCharParenthesisOpen && input == "(")
            {
                return;
            }


            // 마지막 문자가 연산자이면서 '.'을 입력할 경우
            if (IsLastCharOperator(operators) && input == ".")
            {
                return;
            }


            // '.'이 입력할 경우
            if (input == ".")
            {
                // '.'이 존재하지 않으면 추가 (자세한 조건은 변수에서 선언함)
                if (!dotExists)
                {
                    Add(input);
                }
                else
                {
                    return;
                }
            }


            // 숫자를 입력할 경우
            if (isInputNumber)
            {
                Add(input);
            }


            // 마지막 문자가 숫자이고 '('와 ')'의 개수가 일치하지 않은 경우에 '('이 입력될 경우
            if (isLastCharNumber && unmatchedParentheses && input == "(")
            {
                return;
            }


            // 마지막 문자가 숫자이고 시작되는 '('가 ')'의 개수보다 적거나 같은 경우에 ')'를 입력할 경우
            if (isLastCharNumber && isParenthesesBalancedOrOpenLess && input == ")")
            {
                return;
            }


            // 마지막 문자가 숫자이면서 '('를 입력할 경우
            if (isLastCharNumber && input == "(")
            {
                FormulaAndResult += "×";
            }


            // 연산자를 입력할 경우
            if (isInputOperator)
            {
                Add(input);
            }


        }


        // backSpace 클릭 및 키보드 입력 시 FormulaAndResult에서 제일 뒤에 있는 한 글자 삭제

        public void DeleteFormula()
        {
            // FormulaAndResult가 null이나 빈 문자열이 아닐 경우
            if (!string.IsNullOrEmpty(FormulaAndResult))
            {
                FormulaAndResult = FormulaAndResult.Substring(0, FormulaAndResult.Length - 1);
            }


            if (FormulaAndResult == "Error")
            {
                Clear();
            }
        }


        // '=' 버튼 클릭 시 결과값을 FormulaAndResult에 저장
        // FormulaAndResult[^1]는 char이기 때문에 ""이 아닌 ''로 작성
        // FormulaAndResult.Length > 0를 확인하는 이유는 빈 문자열인 상태에서 마지막 문자를 확인할 경우 오류가 발생하기 때문

        public void CalculateResult()
        {
            try
            {
                // FormulaAndResult가 빈 문자열일 경우
                bool isEmpty = FormulaAndResult.Length == 0;

                // 계산 시 ×, ÷는 오류가 발생하기에 연산자로 변환
                string modifiedFormula = FormulaAndResult.Replace('×', '*').Replace('÷', '/');


                // 마지막 문자가 연산자이면서 그 연산자가 ')'이 아닐 경우
                if (IsLastCharOperator(operators) && !IsLastCharParenthesisClose())
                {
                    return;
                }


                // 마지막 문자가 연산자가 아니면서 '('의 개수가 ')'의 개수보다 많을 경우
                if (IsOpeningParenthesisExcess() && !IsLastCharOperator(operators))
                {
                    FormulaAndResult += ")";
                }


                // 빈 문자열인 상태에서 '='을 입력할 경우
                if (isEmpty)
                {
                    return;
                }


                // CalculationModel의 Calculation으로 이동 후 수식을 계산
                FormulaAndResult = _model.Calculation(modifiedFormula).ToString();

            }
            catch
            {
                MessageBox.Show("오류가 발생했습니다." + Environment.NewLine + "다시 계산하시려면 숫자, 연산자 등을 입력해주십시오.", "계산기 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                FormulaAndResult = "Error";
            }
        }


        // EqualsClick 이벤트 실행 시 FormulaAndResult의 정보를 CalculationHistory에 업로드
        public void HandleEqualsClick()
        {
            // FormulaAndResult가 null이나 빈 문자열이 아닐 경우
            bool isFormulaNotEmpty = !string.IsNullOrEmpty(FormulaAndResult);

            // CalculationHistory가 null이나 빈 문자열이 아닐 경우
            bool isHistoryNotEmpty = !string.IsNullOrEmpty(CalculationHistory);


            if (isFormulaNotEmpty)
            {
                // CalculationHistory에 문자가 있을 경우 빈 문자열로 초기화
                if (isHistoryNotEmpty)
                {
                    CalculationHistory = string.Empty;
                }


                // 마지막 문자가 연산자이면서 그 연산자가 ')'가 아닐 경우
                if (IsLastCharOperator(operators) && !IsLastCharParenthesisClose())
                {
                    return;
                }


                // '('의 개수가 ')'의 개수보다 많을 경우
                if (IsOpeningParenthesisExcess())
                {
                    FormulaAndResult += ")";
                }

                CalculationHistory = FormulaAndResult;
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