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
    /// Логика взаимодействия для OpenScore.xaml
    /// </summary>
    public partial class OpenScore : Window
    {
        public OpenScore()
        {
            InitializeComponent();
            LoadScore();

        }
        public void LoadScore() //данные для комбобокса
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                {
                    connection.Open();
                    //string query = $@"SELECT NameType,ID FROM BillsTypes WHERE IDFace = 1";
                    string query = $@"SELECT NameType,ID FROM BillsTypes";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("BillsTypes");
                    SDA.Fill(dt);
                    txtscore.ItemsSource = dt.DefaultView;
                    txtscore.DisplayMemberPath = "NameType";
                    txtscore.SelectedValuePath = "ID";

                    //query = $@"SELECT City FROM Cities ";
                    //cmd = new SQLiteCommand(query, connection);
                    //SDA = new SQLiteDataAdapter(cmd);
                    //dt = new DataTable("Cities");
                    //SDA.Fill(dt);
                    //txtOtchest.ItemsSource = dt.DefaultView;
                    //txtOtchest.DisplayMemberPath = "City";
                    //txtOtchest.SelectedValuePath = "ID";

                    query = $@"SELECT Adress,ID  FROM Filials ";
                    cmd = new SQLiteCommand(query, connection);
                    SDA = new SQLiteDataAdapter(cmd);
                    dt = new DataTable("Filials");
                    SDA.Fill(dt);
                    txtfill.ItemsSource = dt.DefaultView;
                    txtfill.DisplayMemberPath = "Adress";
                    txtfill.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }

        }

        private void imgclouse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuBank Aftoriz = new MenuBank();
            this.Close();
            Aftoriz.ShowDialog();
        }

        public void Open()
        {
            Saver.Date = DateTime.Now.ToString("dd/MM/yyyy");
             MessageBox.Show(Saver.Date);
            //DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            //{
            //    Saver.Date = DateTime.Now.ToString("yyyy/MM/dd");
            //}, 
            //this.Dispatcher);
            //timer.Start();
            //MessageBox.Show(Saver.Date);

        }

        private void btnopen_Click(object sender, RoutedEventArgs e)
        {
           
            if (txtfill.SelectedIndex != -1 && txtscore.SelectedIndex != -1)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                    {
                        connection.Open();
                        bool resultClass = int.TryParse(txtfill.SelectedValue.ToString(), out int idFils);
                        bool resultClass2 = int.TryParse(txtscore.SelectedValue.ToString(), out int idScore);
                        string query = $@"SELECT  COUNT(1) FROM Bills WHERE IDAccount = {Saver.IDAcc} AND IDType = {idScore}"; //Получение данных из таблицы Девайсы
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        MessageBox.Show("111");
                        if (count != 1)
                        {
                            Open();
                            // int.TryParse(txtname.SelectedValue.ToString(), out int idScore);
                            query = $@"INSERT INTO Bills ('IDFilial','IDType','IDAccount','DataOpen','Money') VALUES ({idFils},{idScore},{Saver.IDAcc},'{Saver.Date}',1000)"; //Получение данных из таблицы Девайсы
                            cmd = new SQLiteCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                            MessageBox.Show("Счет Открыт");
                        }
                        else
                        {
                            MessageBox.Show("У вас уже открыт данный вид счета!");
                        }   
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка" + ex);
                }
            }
            else
            {
                MessageBox.Show("2222");
            }
        }
    }
}
