using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vacancies.Models
{
    public static class SimpleComand
    {
        public static bool CheckTextBox(TextBox tb)
        {
            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                tb.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000"));
                return true;
            }
        }
        public static bool CheckTextBox(PasswordBox tb)
        {
            if (string.IsNullOrEmpty(tb.Password))
            {
                tb.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                tb.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000"));
                return true;
            }
        }

        public static bool CheckComboBox(ComboBox cb)
        {
            if (cb.SelectedIndex == -1)
            {
                cb.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                cb.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000"));
                return true;
            }
        }
        public static bool CheckDatePicker(DatePicker dp)
        {
            if (dp.Text == null)
            {
                dp.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                dp.BorderBrush = new SolidColorBrush(color: (Color)ColorConverter.ConvertFromString("#89000000"));
                return true;
            }
        }
        public static bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }
        public static byte[] GetHash(string text)
        {
            byte[] tmpSource;
            byte[] tmpHash;
            //Create a byte array from source data
            tmpSource = Encoding.ASCII.GetBytes(text);

            //Compute hash based on source data
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return tmpHash;
        }

        public static bool ChechHashEquil(byte[] one, byte[] two)
        {
            bool bEqual = false;
            if (two.Length == one.Length)
            {
                int i = 0;
                while ((i < two.Length) && (two[i] == one[i]))
                {
                    i += 1;
                }
                if (i == two.Length)
                {
                    bEqual = true;
                }
            }

            if (bEqual)
                return true;
            else
                return false;
        }
    }
}
