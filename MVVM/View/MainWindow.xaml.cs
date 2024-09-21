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
            // ViewModel로부터 CalculationHistory와 FormulaAndResult 값을 가져옴
            var viewModel = (CalculatorViewModel)DataContext;

            // CalculationHistory가 공백이 아닐 때만 처리
            if (!string.IsNullOrEmpty(viewModel.CalculationHistory))
            {
                // CalculationHistory의 내용을 FormulaAndResult로 이동
                viewModel.FormulaAndResult = viewModel.CalculationHistory;

                // CalculationHistory 초기화
                viewModel.CalculationHistory = string.Empty;
            }
        }

        // 마우스가 RichTextBox에 들어왔을 때 커서를 손가락 모양으로 변경
        private void CalculationHistoryMouseEnter(object sender, MouseEventArgs e)
        {
            ResultBox.Cursor = Cursors.Hand;
        }

        // 마우스가 RichTextBox에서 나갔을 때 기본 커서로 변경
        private void CalculationHistoryMouseLeave(object sender, MouseEventArgs e)
        {
            ResultBox.Cursor = Cursors.Arrow;
        }

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

            if (!string.IsNullOrEmpty(content))
            {
                // ButtonClick 호출
                ButtonClick(new Button { Content = content }, new RoutedEventArgs());
            }
        }
    }
}
