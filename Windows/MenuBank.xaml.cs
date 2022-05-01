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
using ITBankBigFarm.Connection;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace ITBankBigFarm.Windows
{
    /// <summary>
    /// Логика взаимодействия для MenuBank.xaml
    /// </summary>
    public partial class MenuBank : Window
    {
        public MenuBank()
        {
            InitializeComponent();
            Load();
            Profile.Visibility = Visibility.Hidden;
        }

        private void imgclouse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        public void Load()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    txtlogin.Text = Saver.Login;
                    connection.Open();
                    string query = $@"SELECT count (ID) FROM Bills WHERE IDAccount = {Saver.IDAcc}";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        txtnumberscore.Text = "Null";
                        txtmoney.Text = "Null";
                        txttypescope.Text = "Null";
                       // MessageBox.Show("Login занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }


            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }

        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            textBlock5.Foreground = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#f1d3bc"));
            textBlock6.Foreground = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("Black"));
        }

        private void Image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StartLocal Aftoriz = new StartLocal();
            this.Close();
            Aftoriz.ShowDialog();
        }
    }
}
