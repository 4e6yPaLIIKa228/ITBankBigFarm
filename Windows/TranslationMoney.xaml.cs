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
    /// Логика взаимодействия для TranslationMoney.xaml
    /// </summary>
    public partial class TranslationMoney : Window
    {
        public TranslationMoney()
        {
            InitializeComponent();
            LoadScore();
        }

        private void imgback_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuBank Aftoriz = new MenuBank();
            this.Close();
            Aftoriz.ShowDialog();
        }

        public void LoadScore()
        {
            try
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                    {
                        connection.Open();
                        string query = $@"SELECT BillsTypes.ID, BillsTypes.NameType FROM Bills 
                                      JOIN BillsTypes on Bills.IDType = BillsTypes.ID
                                      WHERE IDAccount = {Saver.IDAcc} and BillsTypes.ID = {Saver.IDTypeScore} ";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable("BillsTypes");
                        SDA.Fill(dt);
                        cmbScoreTranslt.ItemsSource = dt.DefaultView;
                        cmbScoreTranslt.DisplayMemberPath = "NameType";
                        cmbScoreTranslt.SelectedValuePath = "ID";
                        //cmbScoreHome.SelectedIndex = 0;
                        cmbScoreTranslt.SelectedValue = Saver.IDTypeScore;
                        cmbScoreTranslt.IsEnabled = false;
                        string query1 = $@"SELECT ID FROM Bills WHERE IDAccount = {Saver.IDAcc} and Bills.IDType = {Saver.IDTypeScore}";
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        SQLiteDataReader dr = null;
                        dr = cmd1.ExecuteReader();
                        TxtScoreyou.IsEnabled = false;
                        while (dr.Read())
                        {
                            TxtScoreyou.Text = dr["ID"].ToString();
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка" + ex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }

        private void btnotpravka_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sum = TxtMoney.Text;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                // Convert.ToInt32(sum);
                if (TxtFriend.Text != "" && Convert.ToSingle(TxtMoney.Text) > 0 && TxtFriend.Text != "")
                {
                    MessageBox.Show("1");
                    using (SQLiteConnection connection = new SQLiteConnection(SqlDBConnection.connection))
                    {
                        connection.Open();
                        string query = $@"SELECT COUNT(ID) FROM Bills WHERE Bills.ID = {TxtFriend.Text} and Bills.ID != {TxtScoreyou.Text} ";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                        {
                            MessageBox.Show("1.1");
                            query = $@"SELECT Money FROM Bills WHERE Bills.ID = {TxtScoreyou.Text} ";
                            cmd = new SQLiteCommand(query, connection);
                            SQLiteDataReader dr = null;
                            string moneyyou = "0";
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            { 
                                moneyyou = dr["Money"].ToString(); //колл денег на счете отрпавителя
                            }
                            //double moneyou = Convert.ToDouble(moneyyou);
                           // MessageBox.Show($@"{moneyou}");
                            if (Convert.ToDouble(sum) < Convert.ToSingle(moneyyou))
                            {
                                MessageBox.Show("3.1");
                                query = $@"SELECT Money FROM Bills WHERE Bills.ID = {TxtFriend.Text} ";
                                cmd = new SQLiteCommand(query, connection);
                                dr = null;
                                string moneyfriend = "0";
                                dr = cmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    moneyfriend = dr["Money"].ToString(); //колл денег на счете отрпавителя
                                }

                                double sumfriend = Convert.ToSingle(moneyfriend); //сколько денег у получателя
                                
                                double totalsumfriend = sumfriend + Convert.ToSingle(sum); // итоговая сумма получателя
                                Math.Round(totalsumfriend, 2);
                                double output1 = Convert.ToDouble(totalsumfriend.ToString("N3"));
                                double totalsumyou = Convert.ToSingle(moneyyou) - Convert.ToSingle(sum); //итоговая сумма отправителя
                                Math.Round(totalsumyou, 2);
                                double output = Convert.ToDouble(totalsumyou.ToString("N3"));
                                query = $@"UPDATE Bills SET Money=@Money WHERE ID=@ID;"; //счет получателя
                                cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Money", output1);
                                cmd.Parameters.AddWithValue("@ID", TxtFriend.Text);
                                cmd.ExecuteReader();
                                query = $@"UPDATE Bills SET Money=@Money  WHERE ID=@ID;"; //счет отправителя
                                cmd = new SQLiteCommand(query, connection);
                                cmd.Parameters.AddWithValue("@Money", output);
                                cmd.Parameters.AddWithValue("@ID", TxtScoreyou.Text);
                                cmd.ExecuteReader();
                                MessageBox.Show("Operation completed");
                            }
                            else
                            {
                                MessageBox.Show("3.2");
                            }
                        }
                        else
                        {
                            MessageBox.Show("2.1");
                            connection.Close();
                        }
                    }
                        
                }
                else
                {
                    MessageBox.Show("2");
                }
            }
           catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TxtFriend_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void TxtMoney_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789,.".IndexOf(e.Text) < 0;
        }
    }
}
