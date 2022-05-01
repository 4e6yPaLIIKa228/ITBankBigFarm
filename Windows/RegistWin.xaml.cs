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
using System.Data.SQLite;
using System.Data.SqlClient;
using ITBankBigFarm.Connection;

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
            try
            {
                SimpleComand.CheckTextBox(txtlog);
               // SimpleComand.CheckTextBox(txtpass);
                if (chekmail.IsChecked == false)
                {
                    SimpleComand.CheckTextBox(txtemail);
                }
                else
                {
                    txtemail.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000"));
                    
                }
                if (checksecretword.IsChecked == false)
                {
                    SimpleComand.CheckTextBox(txtsecret);
                }
                else
                {
                    txtsecret.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000"));
                    
                }
                if (txtlog.Text.Length < 1)
                {
                    // MessageBox.Show("Логин должне быть больше 5 символов!", "Ошбика", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtlog.BorderBrush = Brushes.Red;
                }

                if (txtpass.Password.Length < 1)
                {
                   // MessageBox.Show("Пароль должне быть больше 5 символов!", "Ошбика", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtpass.BorderBrush = Brushes.Red;
                }
            }
            catch(Exception er)
            {
                MessageBox.Show(Convert.ToString(er));
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SqlDBConnection.connection))
                {
                  //  connection.Open(); 
                    Checker();
                    if (chekmail.IsChecked == true && checksecretword.IsChecked == true) //Без почты и слова
                    {
                        MessageBox.Show("1");
                        if (txtlog.Text != "" && txtlog.Text.Length > 2 && txtpass.Password != "" && txtpass.Password.Length > 2)
                        {
                            MessageBox.Show("1.1"); //Верно
                            var Login = txtlog.Text.ToLower();
                            var Pass = SimpleComand.GetHash(txtpass.Password);
                            MessageBox.Show($@"{Pass}");
                        }
                        else
                        {
                            MessageBox.Show("1.2"); //Неверно
                        }
                    }
                    else if (chekmail.IsChecked == false && checksecretword.IsChecked == true) //С почтой , но без слова
                    {
                        MessageBox.Show("2");
                        if (txtlog.Text != "" && txtlog.Text.Length > 2 && txtpass.Password != "" && txtpass.Password.Length > 2 && txtemail.Text != "")
                        {
                            MessageBox.Show("2.1"); //Верно
                        }
                        else
                        {
                            MessageBox.Show("2.2"); //Неверно
                        }
                    }
                    else if (chekmail.IsChecked == true && checksecretword.IsChecked == false) //Со словом , но без почты
                    {
                        MessageBox.Show("3");
                        if (txtlog.Text != "" && txtlog.Text.Length > 2 && txtpass.Password != "" && txtpass.Password.Length > 2 && txtsecret.Text != "")
                        {
                            MessageBox.Show("3.1"); //Верно
                        }
                        else
                        {
                            MessageBox.Show("3.2"); //Неверно
                        }
                    }
                    else if (chekmail.IsChecked == false && checksecretword.IsChecked == false)
                    {
                        MessageBox.Show("4");
                        if (txtlog.Text != "" && txtlog.Text.Length > 2 && txtpass.Password != "" && txtpass.Password.Length > 2 && txtsecret.Text != "" && txtemail.Text != "")
                        {
                            MessageBox.Show("4.1"); //Верно
                        }
                        else
                        {
                            MessageBox.Show("4.2"); //Неверно
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }

        }

        private void checksecretword_Checked(object sender, RoutedEventArgs e)
        {
            if (checksecretword.IsChecked == true)
            {
                txtsecret.IsEnabled = false;
                
            }
        }
        private void checksecretword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checksecretword.IsChecked == false)
            {
                txtsecret.IsEnabled = true;                
            }
        }

        private void chekmail_Checked(object sender, RoutedEventArgs e)
        {
            if (chekmail.IsChecked == true)
            {
                txtemail.IsEnabled = false;
                
            }
        }

        private void chekmail_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chekmail.IsChecked == false)
            {
                txtemail.IsEnabled = true;
                
            }
        }

      
    }
}

