using Calculator.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator.MVVM.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private CalculatorViewModel ViewModel => (CalculatorViewModel)DataContext;

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Clear();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ViewModel.UpdateFormula(button.Content.ToString());
        }

        private void BackSpaceClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteFormula();
        }

        private void EqualsClick(object sender, RoutedEventArgs e)
        {
            ViewModel.HandleEqualsClick();
            ViewModel.CalculateResult();           
        }

        private void CalculationHistoryMouseDown(object sender, MouseButtonEventArgs e)
        {
            // ViewModel로부터 CalculationHistory와 FormulaAndResult의 값을 가져오기 위해 선언
            var viewModel = (CalculatorViewModel)DataContext;

            // CalculationHistory가 공백이 아닐 경우
            if (!string.IsNullOrEmpty(viewModel.CalculationHistory))
            {
                // CalculationHistory의 내용을 FormulaAndResult로 이동
                viewModel.FormulaAndResult = viewModel.CalculationHistory;

                // CalculationHistory 초기화
                viewModel.CalculationHistory = string.Empty;
            }
        }

        // 마우스가 RichTextBox에 들어왔을 경우 커서를 손가락 모양으로 변경
        private void CalculationHistoryMouseEnter(object sender, MouseEventArgs e)
        {
            ResultBox.Cursor = Cursors.Hand;

            // 마우스가 CalculationHistory에 들어왔을 경우 글자 색상을 검정색으로 변경
            if (sender is Run run)
            {
                run.Foreground = Brushes.Black;
            }
        }

        // 마우스가 RichTextBox에서 나갔을 경우 기본 커서로 변경
        private void CalculationHistoryMouseLeave(object sender, MouseEventArgs e)
        {
            ResultBox.Cursor = Cursors.Arrow;

            // 마우스가 CalculationHistory에 나갔을 경우 글자 색상을 회색으로 변경
            if (sender is Run run)
            {
                run.Foreground = Brushes.Gray;
            }

        }

        // 키보드로 입력했을 경우
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string content = string.Empty;

            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.D0:
                    content = "0";
                    break;
                case Key.NumPad1:
                case Key.D1:
                    content = "1";
                    break;
                case Key.NumPad2:
                case Key.D2:
                    content = "2";
                    break;
                case Key.NumPad3:
                case Key.D3:
                    content = "3";
                    break;
                case Key.NumPad4:
                case Key.D4:
                    content = "4";
                    break;
                case Key.NumPad5:
                case Key.D5:
                    content = "5";
                    break;
                case Key.NumPad6:
                case Key.D6:
                    content = "6";
                    break;
                case Key.NumPad7:
                case Key.D7:
                    content = "7";
                    break;
                case Key.NumPad8:
                case Key.D8:
                    content = "8";
                    break;
                case Key.NumPad9:
                case Key.D9:
                    content = "9";
                    break;
                case Key.Back:
                    BackSpaceClick(this, new RoutedEventArgs());
                    return;
                case Key.OemPeriod:
                case Key.Decimal:
                    content = ".";
                    break;
                case Key.Add:
                    content = "+";
                    break;
                case Key.Subtract:
                    content = "-";
                    break;
                case Key.Multiply:
                    content = "×";
                    break;
                case Key.Divide:
                    content = "÷";
                    break;
                case Key.Enter:
                    EqualsClick(this, new RoutedEventArgs());
                    return;
                case Key.OemOpenBrackets:
                    content = "(";
                    break;
                case Key.OemCloseBrackets:
                    content = ")";
                    break;
            }

            // content가 null이거나 빈 문자열이 아닌 경우 (switch에서 선언한 숫자나 연산자만을 입력받기 위한 조치)
            if (!string.IsNullOrEmpty(content))
            {
                // content에서 입력받은 숫자나 연산자와 같은 ButtonClick 호출
                ButtonClick(new Button { Content = content }, new RoutedEventArgs());
            }
        }
    }
}
