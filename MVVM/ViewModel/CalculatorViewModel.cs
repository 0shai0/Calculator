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

            // '('와 '-'가 맨 앞에 입력되는 경우
            if (FormulaAndResult.Length == 0 && (input == "(" || input == "-"))
            {
                FormulaAndResult += input;
            }


            // '.'이 맨 앞에 올 경우
            if (FormulaAndResult.Length == 0 && (operators.Contains(input) || input == "."))
            {
                return;
            }


            // 0 뒤에 0이 올 경우
            if (FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '0' && input == "0")
            {
                return;
            }


            // 앞에 숫자가 없는데 0 뒤에 숫자가 입력될 경우 (연산자 뒤에 '0'이 있는 경우도 포함)
            if (FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '0' 
                && char.IsDigit(input[0]) 
                && (FormulaAndResult.Length == 1 || operators.Contains(FormulaAndResult[^2])))
            {
                // 마지막 문자인 0을 제거
                FormulaAndResult = FormulaAndResult.Remove(FormulaAndResult.Length - 1);

                FormulaAndResult += input;
            }


            // '.' 뒤에 '.'과 연산자가 오는 경우
            if (FormulaAndResult.Length > 0 && FormulaAndResult[^1] == '.' && operators.Contains(input) && input == ".")
            {
                return;
            }


            // 마지막 문자가 연산자인 경우
            if (FormulaAndResult.Length > 0 && operators.Contains(FormulaAndResult[^1]))
            {
                // 마지막이 ')'로 끝나고 다음에 입력되는 것이 '('일 경우
                if (FormulaAndResult[^1] == ')' && input == "(")
                {
                    FormulaAndResult += "×" + input;
                }

                // 마지막이 ')'로 끝나고 다음에 입력되는 것이 숫자인 경우
                if (FormulaAndResult[^1] == ')' && char.IsDigit(input[0]))
                {
                    FormulaAndResult += "×" + input;
                }

                // 마지막이 ')'로 끝나고 다음에 입력되는 것이 ')'이 아닌 경우
                if (FormulaAndResult[^1] == ')' && input != ")")
                {
                    FormulaAndResult += input;
                }     
                
                // 연산자 뒤에 연산자가 입력될 경우
                if (operators.Contains(input) && input != "(")
                {
                    return;
                }

                // '(' 뒤에 '('이 입력될 경우 
                if (FormulaAndResult[^1] == '(' && input == "(")
                {
                    return;
                }

                // 연산자 뒤에 '.'이 입력될 경우
                if (input == ".")
                {
                    return;
                }
            }


            // 마지막 연산자의 위치를 찾기 위해 for문을 활용한 변수 lastOperatorIndex
            int lastOperatorIndex = -1;
            for (int i = FormulaAndResult.Length - 1; i >= 0; i--)
            {
                if (operators.Contains(FormulaAndResult[i]))
                {
                    lastOperatorIndex = i;
                    break;
                }
            }





            // 수정해야 할 곳 시작




            // 마지막 연산자 다음에 숫자가 있는지 확인
            if (lastOperatorIndex != -1 && lastOperatorIndex < FormulaAndResult.Length - 1)
            {
                // 마지막 연산자 다음 문자부터 숫자를 찾기
                for (int i = lastOperatorIndex + 1; i < FormulaAndResult.Length; i++)
                {
                    if (char.IsDigit(FormulaAndResult[i]))
                    {
                        // 숫자 다음에 '.'이 없는지 확인
                        if (!FormulaAndResult.Substring(lastOperatorIndex + 1, i - lastOperatorIndex - 1).Contains("."))
                        {
                            FormulaAndResult += input; // 소수점 추가
                        }
                        break; // 숫자가 더 이상 없으면 루프 종료
                    }
                    else if (operators.Contains(FormulaAndResult[i]))
                    {
                        break; // 다음에 연산자가 나오면 더 이상 탐색할 필요 없음
                    }
                }
            }


            // 맨 앞에 있는 숫자 뒤에 '.'이 올 경우
            if (FormulaAndResult.Length > 0 && char.IsDigit(FormulaAndResult[^1]) 
                && input == "." && )
            {
                FormulaAndResult += input;
            }




            // 수정해야 할 곳 끝








            // 숫자 입력 추가
            if (char.IsDigit(input[0]))
            {
                FormulaAndResult += input;
            }


            // 마지막이 숫자이고 마무리되지 않은 괄호가 있는 경우
            if (FormulaAndResult.Length > 0 && char.IsDigit(FormulaAndResult[^1])
                && FormulaAndResult.Count(c => c == '(') != FormulaAndResult.Count(c => c == ')'))
            {
                if (input == "(")
                {
                    return;
                }
            }


            // 시작되는 '('가 없는 상태에서 숫자 뒤에 ')'가 온 경우
            if (FormulaAndResult.Length > 0 && char.IsDigit(FormulaAndResult[^1]) && input == ")"
                && FormulaAndResult.Count(c => c == '(') <= FormulaAndResult.Count(c => c == ')'))
            {
                return;
            }


            // 숫자가 나오고 '('가 올 경우
            if (char.IsDigit(FormulaAndResult[^1]) && input == "(")
            {
                FormulaAndResult += "×";
            }


            // 연산자 입력 추가
            if (operators.Contains(input))
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

                // 괄호가 열려있다면 닫는 괄호 추가
                if (FormulaAndResult.Count(c => c == '(') > FormulaAndResult.Count(c => c == ')'))
                {
                    FormulaAndResult += ")";
                }

                // '='이 맨 앞에 입력될 경우
                if (FormulaAndResult.Length == 0)
                {
                    return;
                }

                // 연산자 뒤에 '='을 입력될 경우
                if (operators.Contains(FormulaAndResult[^1]))
                {
                    return;
                }

                // 수식을 계산
                FormulaAndResult = _model.Calculation(FormulaAndResult).ToString();


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
