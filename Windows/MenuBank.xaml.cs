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
using System.Data;

namespace ITBankBigFarm.Windows
{
    /// <summary>
    /// Логика взаимодействия для MenuBank.xaml
    /// </summary>
    public partial class MenuBank : Window
    {
        int NumberFaceFiz, NumberFaceYur,ClickProf, ClickHome=1;
        public MenuBank()
        {
            InitializeComponent();
            Profile.Visibility = Visibility.Hidden;
            LoadProfelComb();
            LoadcmbTypeScore();
            LoadHome();
        }

        private void imgclouse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }
        public void InfoFace() //Узнаем физ или юр
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    string query = $@"SELECT NameType,ID FROM BillsTypes WHERE IDFace = 1";
                    SQLiteDataReader dr = null;
                    SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                    dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        Saver.IDAcc = dr["ID"].ToString();
                        //  Saver.IDAcc = countID;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }
        public void LoadcmbTypeScore()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    string query = $@"SELECT BillsTypes.ID, BillsTypes.NameType FROM Bills 
                                      JOIN BillsTypes on Bills.IDType = BillsTypes.ID
                                      WHERE IDAccount = {Saver.IDAcc} ";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("BillsTypes");
                    SDA.Fill(dt);
                    cmbScoreHome.ItemsSource = dt.DefaultView;
                    cmbScoreHome.DisplayMemberPath = "NameType";
                    cmbScoreHome.SelectedValuePath = "ID";
                    //cmbScoreHome.SelectedIndex = 0;
                    cmbScoreHome.SelectedValue = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }

        public void LoadHome() //Загрузка счетов клиента
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
                        cmbScoreHome.Visibility = Visibility.Hidden;
                        // MessageBox.Show("Login занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else
                    {

                      bool resultClass = int.TryParse(cmbScoreHome.SelectedValue.ToString(), out int idScoreHome);
                        query = $@"SELECT Bills.ID, NameType, DataOpen, Money FROM Bills 
                                JOIN BillsTypes on Bills.IDType = BillsTypes.ID
                                WHERE IDAccount = {Saver.IDAcc} and Bills.IDType = {idScoreHome}";
                        cmd = new SQLiteCommand(query, connection);
                        SQLiteDataReader dr = null;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            txtnumberscore.Text = dr["ID"].ToString();
                            txtmoney.Text = dr["Money"].ToString();
                            txttypescope.Text = dr["NameType"].ToString();
                            //Saver.IDAcc = dr["ID"].ToString();
                            //  Saver.IDAcc = countID;
                        }
                    }
                    connection.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }

        }

        private void imageProf_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) //Иконка профиля
        {
            if (ClickProf != 1)
            {
                ClickProf = 1;
                textBlock5.Foreground = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#f1d3bc"));
                textBlock6.Foreground = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("Black"));
                FuzPerson.Visibility = Visibility.Hidden;
                btnsave.Visibility = Visibility.Hidden;
                btneddit.Visibility = Visibility.Hidden;
                Profile.Visibility = Visibility.Visible;
                Home.Visibility = Visibility.Hidden;
                ClickHome = 0;
                image.IsEnabled = true;
            }
            else
            {
                imageProf.IsEnabled = false;
            }
        }

        private void Image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StartLocal Aftoriz = new StartLocal();
            this.Close();
            Aftoriz.ShowDialog();
        }

        public void LoadProfelComb() //данные для комбобокса
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    connection.Open();
                    string query = $@"SELECT * FROM Pols";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Pols");
                    SDA.Fill(dt);
                    cmbPols.ItemsSource = dt.DefaultView;
                    cmbPols.DisplayMemberPath = "Pol";
                    cmbPols.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }
        private void cmbFace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFace.SelectedIndex == 0)
            {
                MessageBox.Show("Физ");
               // NumberFaceFiz = 1;
                FizFace();
                FuzPerson.Visibility = Visibility.Visible;
            }
            else
            {
                NumberFaceFiz = 0;
            }
            if (cmbFace.SelectedIndex == 1)
            {
                MessageBox.Show("Юр");
                NumberFaceYur = 1;
            }
            else
            {
                NumberFaceYur = 0;
            }
        }
        public void FizFace()
        {
           try
           {
                    using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                    {
                        connection.Open();
                        string query = $@"SELECT COUNT(1) FROM PhysicalPerson WHERE ID = {Saver.IDAcc};"; //Получение данных из таблицы Девайсы
                        SQLiteCommand cmd = new SQLiteCommand(query, connection); 
                       // cmd3.Parameters.AddWithValue("IDBrand", IdKab);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                    {
                            MessageBox.Show("1");
                            btneddit.Visibility = Visibility.Visible;
                            query = $@"SELECT Name,Family,MiddleName,SerriaPas,NumberPas,Pols.Pol 
                                        FROM PhysicalPerson JOIN Pols 
                                        ON PhysicalPerson.IDPol = Pols.ID
                                        WHERE PhysicalPerson.ID = {Saver.IDAcc};";
                            SQLiteDataReader dr = null;
                            SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                            dr = cmd1.ExecuteReader();
                            while (dr.Read())

                        {

                            txtfame.Text = dr["Family"].ToString();
                            txtname.Text = dr["Name"].ToString();
                            txtOtchest.Text = dr["MiddleName"].ToString();
                            txtserpass.Text = dr["SerriaPas"].ToString();
                            txtnumberpas.Text = dr["NumberPas"].ToString();
                            cmbPols.Text = dr["Pol"].ToString();

                        }
                    }
                        else
                        {
                            MessageBox.Show("0");
                            btnsave.Visibility = Visibility.Visible;
                        }
                    }
           }
           catch (Exception ex)
           {
                    MessageBox.Show("Ошибка" + ex);
           }
        }
        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) //Главаня Иконка
        {
          
            if (ClickHome != 1)
            {
                ClickHome = 1;
                textBlock6.Foreground = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#f1d3bc"));
                textBlock5.Foreground = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("Black"));
                textBlock7.Foreground = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("Black"));
                FuzPerson.Visibility = Visibility.Hidden;
                btnsave.Visibility = Visibility.Hidden;
                btneddit.Visibility = Visibility.Hidden;
                Profile.Visibility = Visibility.Hidden;
                Home.Visibility = Visibility.Visible;
                ClickProf = 0;
                cmbFace.SelectedIndex = -1;
                image.IsEnabled = true;
                imageProf.IsEnabled = true;
            }
            else
            {
                image.IsEnabled = false;
            }
        }

        private void cmbScoreHome_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadHome();
        }

        public void Checker() //Для проверки
        {
            try
            {
                SimpleComand.CheckTextBox(txtfame);
                SimpleComand.CheckTextBox(txtname);
                SimpleComand.CheckTextBox(txtfame);
                SimpleComand.CheckTextBox(txtfame);
                SimpleComand.CheckComboBox(cmbPols);
                SimpleComand.CheckTextBox(txtfame);

                //SimpleComand.CheckTextBox(txtpassreg);
                //SimpleComand.CheckPassBox(txtpassreg);

                if (txtserpass.Text.Length != 4)
                {
                    // MessageBox.Show("Логин должне быть больше 5 символов!", "Ошбика", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtserpass.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtserpass.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000")); ;
                }


                if (txtnumberpas.Text.Length != 6)
                {
                    // MessageBox.Show("Пароль должне быть больше 5 символов!", "Ошбика", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtnumberpas.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtnumberpas.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000")); ;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(Convert.ToString(er));
            }
        }
        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
                if(txtfame.Text != "" && txtname.Text != "" && cmbPols.SelectedIndex !=-1 && txtserpass.Text != "" && txtserpass.Text.Length ==4 && txtnumberpas.Text.Length ==6 && txtnumberpas.Text != "")
                {
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                        {
                        MessageBox.Show("111");
                        Checker();
                        connection.Open();
                        bool resultClass = int.TryParse(cmbPols.SelectedValue.ToString(), out int idPols);
                        string query = $@"INSERT INTO PhysicalPerson ('ID','Name','Family','MiddleName','SerriaPas','NumberPas','IDPol') VALUES ({Saver.IDAcc},{txtname.Text},{txtfame.Text},{txtOtchest.Text},{txtserpass.Text},{txtnumberpas.Text},{idPols})"; //Получение данных из таблицы Девайсы
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Данные сохранены");
                        FizFace();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка" + ex);
                    }
                }
                else
                {
                    Checker();
                    MessageBox.Show("2222");
                }



        }
        private void buttonopenscore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    connection.Open();
                    string query = $@"SELECT COUNT(1) FROM PhysicalPerson WHERE ID = {Saver.IDAcc};"; //Получение данных из таблицы Физ.лица
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    // cmd3.Parameters.AddWithValue("IDBrand", IdKab);
                    int countfiz = Convert.ToInt32(cmd.ExecuteScalar());
                    query = $@"SELECT COUNT(1) FROM LegalPerson WHERE ID = {Saver.IDAcc};"; //Получение данных из таблицы Юр.лица
                    SQLiteCommand cmd1 = new SQLiteCommand(query, connection);
                    int countyur = Convert.ToInt32(cmd1.ExecuteScalar());
                    if (countfiz == 1 || countyur == 1)
                    {
                        MessageBox.Show("Yes acc");
                        //OpenScore EddTitl = new OpenScore();
                        //EddTitl.Owner = this;
                        //bool? result = EddTitl.ShowDialog();
                        //switch (result)
                        //{
                        //    default:
                        //    break;
                        //}
                        OpenScore Aftoriz = new OpenScore();
                        this.Close();
                        Aftoriz.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No acc");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }
    }
}
