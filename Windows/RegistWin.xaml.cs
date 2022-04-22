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
using System.Text.RegularExpressions;
using System.Security.Cryptography;


namespace ITBankBigFarm.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistWin.xaml
    /// </summary>
    public partial class RegistWin : Window
    {
        
        public RegistWin()
        {
            InitializeComponent();

        }
     
        public void Checker()
        {
            SimpleComand.CheckTextBox(txtlog);
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

