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
    }
}
