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
    /// Логика взаимодействия для StartLocal.xaml
    /// </summary>
    public partial class StartLocal : Window
    {
       string  IPReg, IPLast, koll;
        int provekraLogin = 0;
        public StartLocal()
        {
            InitializeComponent();
        }
        public void InfoIP() //Получение ip-адреса.
        {
            // Получение имени компьютера.
            String host = System.Net.Dns.GetHostName();
            // Получение ip-адреса.
            System.Net.IPAddress IPReg0 = System.Net.Dns.GetHostByName(host).AddressList[0];
            System.Net.IPAddress IPLast0 = System.Net.Dns.GetHostByName(host).AddressList[0];
            IPLast = IPLast0.ToString();
            IPReg = IPReg0.ToString();
            // MessageBox.Show(IPReg.ToString());
        }

        public void GoHome()
        {
            try
            {
               using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    InfoIP();
                    connection.Open();
                    string query = $@"UPDATE Account SET IPLast=@IPLast WHERE Login=@Login;";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IPLast", IPLast);
                    cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                    cmd.ExecuteReader();
                    string smaltxt = txtlog.Text.ToLower();
                    query = $@"SELECT ID FROM Account WHERE Login={smaltxt}";
                    // cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                    // int countID = Convert.ToInt32(cmd.ExecuteScalar());
                    Saver.Login = txtlog.Text.ToLower();
                    SQLiteDataReader dr = null;
                    SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                    dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        Saver.IDAcc = dr["ID"].ToString();
                        //  Saver.IDAcc = countID;
                    }

                    connection.Close();
                    MessageBox.Show("Добро пожаловать! " + $@"{txtlog.Text}");
                    MenuBank Aftoriz = new MenuBank();
                    this.Close();
                    Aftoriz.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }

        private void btnavtoriz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    if (txtlog.Text != "" && txtlog.Text.Length > 2 && txtpass.Password != "" && txtpass.Password.Length > 2)
                    {

                        MessageBox.Show("1");
                        var Pass = SimpleComand.GetHash(txtpass.Password);
                        connection.Open();
                        string query = $@"SELECT  COUNT(1) FROM Account WHERE Login=@Login AND Pass=@Pass and IDStatus != 3";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        string LoginLower = txtlog.Text.ToLower();
                        cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                        cmd.Parameters.AddWithValue("@Pass", Pass);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        query = $@"SELECT  COUNT(1) FROM Account WHERE Login = {txtlog.Text.ToLower()} and Pass != @Pass";
                        cmd = new SQLiteCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Pass", Pass);
                        int proverka = Convert.ToInt32(cmd.ExecuteScalar());
                        query = $@"SELECT  COUNT(1) FROM Account WHERE Login = {txtlog.Text.ToLower()} and Pass = @Pass and IDStatus = 3";
                        cmd = new SQLiteCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Pass", Pass);
                        int proverkanapazban = Convert.ToInt32(cmd.ExecuteScalar());
                        if (proverkanapazban == 1)
                        {
                            query = $@"UPDATE Account SET IDStatus=@IDStatus WHERE Login=@Login;";
                            cmd = new SQLiteCommand(query, connection);
                            cmd.Parameters.AddWithValue("@IDStatus", 2);
                            cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                            cmd.ExecuteReader();
                            query = $@"UPDATE Proverka SET Kolltry=@Kolltry WHERE Login=@Login;";
                            cmd = new SQLiteCommand(query, connection);
                            cmd.Parameters.AddWithValue("@Kolltry", 0);
                            cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                            cmd.ExecuteReader();
                            GoHome();
                        }
                        if (count == 1)
                        {
                            InfoIP();
                            query = $@"UPDATE Account SET IPLast=@IPLast WHERE Login=@Login;";
                            cmd = new SQLiteCommand(query, connection);
                            cmd.Parameters.AddWithValue("@IPLast",IPLast);
                            cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                            cmd.ExecuteReader();
                            string smaltxt = txtlog.Text.ToLower();
                            query = $@"SELECT ID FROM Account WHERE Login={smaltxt}";
                            // cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                            // int countID = Convert.ToInt32(cmd.ExecuteScalar());
                            Saver.Login = txtlog.Text.ToLower();
                            SQLiteDataReader dr = null;
                            SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                            dr = cmd1.ExecuteReader();
                            while (dr.Read())
                            {
                                Saver.IDAcc = dr["ID"].ToString();
                                //  Saver.IDAcc = countID;
                            }

                            connection.Close();
                            MessageBox.Show("Добро пожаловать! " + $@"{txtlog.Text}");
                            MenuBank Aftoriz = new MenuBank();
                            this.Close();
                            Aftoriz.ShowDialog();
                        }
                        else
                        {
                          //MessageBox.Show("BANNN");
                        }
                       
                        if (proverka == 1)
                        {
                            query = $@"SELECT Kolltry,TimeEnd FROM Proverka WHERE Login= {txtlog.Text.ToLower()};";
                            cmd = new SQLiteCommand(query, connection);
                            string dateOpen = DateTime.Now.ToString("t");
                            TimeSpan s2 = TimeSpan.Parse(dateOpen);
                            SQLiteDataReader dr = null;
                            string dateban="00:00";
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                koll = dr["Kolltry"].ToString();
                                dateban = dr["TimeEnd"].ToString();
                            }
                            TimeSpan s4 = TimeSpan.Parse(dateban);
                            if (koll == "3" && (TimeSpan.Parse(dateOpen) < TimeSpan.Parse(dateban)))
                            {

                                MessageBox.Show("Ban");
                                query = $@"UPDATE Account SET IDStatus=@IDStatus WHERE Login=@Login;";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@IDStatus", 3);
                                cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                                cmd.ExecuteReader();
                            }
                            else 
                            {
                                query = $@"UPDATE Account SET IDStatus=@IDStatus WHERE Login=@Login;";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@IDStatus", 2);
                                cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                                cmd.ExecuteReader();
                                query = $@"UPDATE Proverka SET Kolltry=@Kolltry WHERE Login=@Login;";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Kolltry", 0);
                                cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                                cmd.ExecuteReader();
                            }                                
                            if (koll != "3")
                            {
                                int kolint =  Convert.ToInt32(koll);                              
                                MessageBox.Show(dateOpen);
                              
                                kolint++;
                                query = $@"UPDATE Proverka SET Kolltry=@Kolltry,TimeBegin=@TimeBegin,TimeEnd=@TimeEnd WHERE Login=@Login;";
                                cmd = new SQLiteCommand(query, connection);                                
                                string timeban = "0:01";
                                TimeSpan s1 = TimeSpan.Parse("0:01");
                                TimeSpan s3 = s1 + s2;
                                string times3 = s3.ToString("hh':'mm");
                                cmd.Parameters.AddWithValue("@Kolltry", kolint);
                                cmd.Parameters.AddWithValue("@TimeBegin", dateOpen);
                                cmd.Parameters.AddWithValue("@TimeEnd", times3);
                                cmd.Parameters.AddWithValue("@Login", txtlog.Text.ToLower());
                                cmd.ExecuteReader();
                                MessageBox.Show("Неверный логин или пароль");
                            }
                        }
                    }

                    else
                    {
                        CheckerLog();
                        MessageBox.Show("2");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
            //MenuBank Aftoriz = new MenuBank();
            //this.Close();
            //Aftoriz.ShowDialog();
        }
        public void CheckerLog() // Для авторизации
        {
            try
            {
                SimpleComand.CheckTextBox(txtlog);
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
                else
                {
                    txtpass.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000")); ;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(Convert.ToString(er));
            }
           
            
        }
        public void Checker() //Для регстирации
        {
            try
            {
                SimpleComand.CheckTextBox(txtlogreg);
                //SimpleComand.CheckTextBox(txtpassreg);
                //SimpleComand.CheckPassBox(txtpassreg);
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
                if (txtlogreg.Text.Length < 1)
                {
                    // MessageBox.Show("Логин должне быть больше 5 символов!", "Ошбика", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtlogreg.BorderBrush = Brushes.Red;
                }
               

                if (txtpassreg.Password.Length < 1)
                {
                    // MessageBox.Show("Пароль должне быть больше 5 символов!", "Ошбика", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtpassreg.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtpassreg.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000")); ;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(Convert.ToString(er));
            }
        }

        public void InfoLogin() //Проверка на повтор логина
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    connection.Open();
                    string query = $@"SELECT  COUNT(1) FROM ACCOUNT WHERE Login=@Login";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Login", txtlogreg.Text.ToLower());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1)
                    {
                        MessageBox.Show("Login занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        provekraLogin = 1;
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }

       

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    
                    Checker();
                   
                    if (chekmail.IsChecked == true && checksecretword.IsChecked == true) //Без почты и слова
                    {
                        MessageBox.Show("1");
                        if (txtlogreg.Text != "" && txtlogreg.Text.Length > 2 && txtpassreg.Password != "" && txtpassreg.Password.Length > 2)
                        {
                            InfoLogin();
                            if (provekraLogin != 1)
                            {
                                InfoIP();
                                connection.Open();
                                MessageBox.Show("1.1"); //Верно
                                var Login = txtlogreg.Text.ToLower();
                                var Pass = SimpleComand.GetHash(txtpassreg.Password);
                                string query = $@"INSERT INTO ACCOUNT ('Login','Pass','IPReg',IPLast,IDStatus) VALUES (@Login,@Pass,@IPReg,@IPLast,@IDStatus)";
                                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Login", txtlogreg.Text.ToLower());
                                cmd.Parameters.AddWithValue("@Pass", Pass);
                                cmd.Parameters.AddWithValue("@IPReg", IPReg);
                                cmd.Parameters.AddWithValue("@IPLast", IPLast);
                                cmd.Parameters.AddWithValue("@IDStatus", 2);
                                cmd.ExecuteNonQuery();
                                query = $@"INSERT INTO Proverka ('TimeBegin','TimeEnd','Kolltry',Login) VALUES (@TimeBegin,@TimeEnd,@Kolltry,@Login)";
                                cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@TimeBegin","00:00");
                                cmd.Parameters.AddWithValue("@TimeEnd", "00:00");
                                cmd.Parameters.AddWithValue("@Kolltry", "0");
                                cmd.Parameters.AddWithValue("@Login", txtlogreg.Text.ToLower());
                                cmd.ExecuteNonQuery();
                                connection.Close();
                                MessageBox.Show("Аккаунт зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                // MessageBox.Show($@"{Pass}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("1.2"); //Неверно
                        }
                    }
                    else if (chekmail.IsChecked == false && checksecretword.IsChecked == true) //С почтой , но без слова
                    {
                        MessageBox.Show("2");
                        if (txtlogreg.Text != "" && txtlogreg.Text.Length > 2 && txtpassreg.Password != "" && txtpassreg.Password.Length > 2 && txtemail.Text != "")
                        {
                          
                            InfoLogin();
                            if (provekraLogin != 1)
                            {
                                InfoIP();
                                connection.Open();
                                MessageBox.Show("2.1"); //Верно
                                var Login = txtlogreg.Text.ToLower();
                                var Pass = SimpleComand.GetHash(txtpassreg.Password);
                                string query = $@"INSERT INTO ACCOUNT ('Login','Pass','IPReg',IPLast,IDStatus,Email) VALUES (@Login,@Pass,@IPReg,@IPLast,@IDStatus,@Email)";
                                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Login", txtlogreg.Text.ToLower());
                                cmd.Parameters.AddWithValue("@Pass", Pass);
                                cmd.Parameters.AddWithValue("@IPReg", IPReg);
                                cmd.Parameters.AddWithValue("@IPLast", IPLast);
                                cmd.Parameters.AddWithValue("@IDStatus", 2);
                                cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                                cmd.ExecuteNonQuery();
                                connection.Close();
                                MessageBox.Show("Аккаунт зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                // MessageBox.Show($@"{Pass}");
                            }

                        }
                        else
                        {
                            MessageBox.Show("2.2"); //Неверно
                        }
                    }
                    else if (chekmail.IsChecked == true && checksecretword.IsChecked == false) //Со словом , но без почты
                    {
                        MessageBox.Show("3");
                        if (txtlogreg.Text != "" && txtlogreg.Text.Length > 2 && txtpassreg.Password != "" && txtpassreg.Password.Length > 2 && txtsecret.Text != "")
                        {
                            if (provekraLogin != 1)
                            {
                                InfoIP();
                                connection.Open();
                                MessageBox.Show("3.1"); //Верно
                                var Login = txtlogreg.Text.ToLower();
                                var Pass = SimpleComand.GetHash(txtpassreg.Password);
                                string query = $@"INSERT INTO ACCOUNT ('Login','Pass','IPReg',IPLast,IDStatus,SecretWord) VALUES (@Login,@Pass,@IPReg,@IPLast,@IDStatus,@SecretWord)";
                                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Login", txtlogreg.Text.ToLower());
                                cmd.Parameters.AddWithValue("@Pass", Pass);
                                cmd.Parameters.AddWithValue("@IPReg", IPReg);
                                cmd.Parameters.AddWithValue("@IPLast", IPLast);
                                cmd.Parameters.AddWithValue("@IDStatus", 2);
                                cmd.Parameters.AddWithValue("@SecretWord", txtsecret.Text);
                                cmd.ExecuteNonQuery();
                                connection.Close();
                                MessageBox.Show("Аккаунт зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                // MessageBox.Show($@"{Pass}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("3.2"); //Неверно
                        }
                    }
                    else if (chekmail.IsChecked == false && checksecretword.IsChecked == false)
                    {
                        MessageBox.Show("4");
                        if (txtlogreg.Text != "" && txtlogreg.Text.Length > 2 && txtpassreg.Password != "" && txtpassreg.Password.Length > 2 && txtsecret.Text != "" && txtemail.Text != "")
                        {
                            if (provekraLogin != 1)
                            {
                                InfoIP();
                                connection.Open();
                                MessageBox.Show("4.1"); //Верно
                                var Login = txtlogreg.Text.ToLower();
                                var Pass = SimpleComand.GetHash(txtpassreg.Password);
                                string query = $@"INSERT INTO ACCOUNT ('Login','Pass','IPReg',IPLast,IDStatus,SecretWord,Email) VALUES (@Login,@Pass,@IPReg,@IPLast,@IDStatus,@SecretWord,@Email)";
                                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Login", txtlogreg.Text.ToLower());
                                cmd.Parameters.AddWithValue("@Pass", Pass);
                                cmd.Parameters.AddWithValue("@IPReg", IPReg);
                                cmd.Parameters.AddWithValue("@IPLast", IPLast);
                                cmd.Parameters.AddWithValue("@IDStatus", 2);
                                cmd.Parameters.AddWithValue("@SecretWord", txtsecret.Text);
                                cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                                cmd.ExecuteNonQuery();
                                connection.Close();
                                MessageBox.Show("Аккаунт зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                // MessageBox.Show($@"{Pass}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("4.2"); //Неверно
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }

        }

        private void checksecretword_Checked(object sender, RoutedEventArgs e)
        {
            if (checksecretword.IsChecked == true)
            {
                txtsecret.IsEnabled = false;
                txtsecret.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000")); ;

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
                txtemail.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000")); ;

            }
        }

        private void image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
