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
using System.Windows.Shapes;

namespace SomeInterestingStuff.ReversePolishNotation
{
    /// <summary>
    /// Interaction logic for RPNWindow.xaml
    /// </summary>
    public partial class RPNWindow : Window
    {
        string expression;

        public RPNWindow()
        {
            InitializeComponent();
            expression = null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RPN notation = new RPN(expression);
                ExpTextBox.Text = notation.ConvertExpressionToRPN();
                ResTextBox.Text = notation.GetResult();
            }
            catch (Exception)
            {
                ExpTextBox.Text = "We've got an error here :)";
                ResTextBox.Text = "Check your input";
            }
        }

        private void IncTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            expression = IncTextBox.Text;
        }
    }
}
