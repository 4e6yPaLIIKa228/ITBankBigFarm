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
using ITBankBigFarm.Windows;

namespace ITBankBigFarm.Windows
{
    /// <summary>
    /// Логика взаимодействия для StartLocal.xaml
    /// </summary>
    public partial class StartLocal : Window
    {
        public StartLocal()
        {
            InitializeComponent();
        }

        private void btnavtoriz_Click(object sender, RoutedEventArgs e)
        {
            MenuBank Aftoriz = new MenuBank();
            this.Close();
            Aftoriz.ShowDialog();
        }
    }
}
