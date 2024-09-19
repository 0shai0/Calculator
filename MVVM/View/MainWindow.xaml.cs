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
            ViewModel.CalculateResult();
        }
    }
}
