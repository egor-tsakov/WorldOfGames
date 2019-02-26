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
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorldOfGames
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if(username.Text.ToString() == "" || password.Password.ToString() == "")
            {
                MessageBox.Show("", "Ошибка регистрации!\nВведи все поля пожалуйста", MessageBoxButton.OK);
                return;
            }

            try
            {
                if (password.Password.ToString() == repassword.Password.ToString())
                {
                    //add функция из dll
                    Users.Add(new Users { username = username.Text.ToString(), password = password.Password.ToString(), money = 0 });
                    MessageBox.Show("", "Поздравляю, ты создал себе аккаунт!!!", MessageBoxButton.OK);
                    Login log = new Login(username.Text.ToString(), password.Password.ToString());
                    log.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("", "Ошибка регистрации!\nПовтори пароли, они не совпадают.", MessageBoxButton.OK);
                }
            }
            catch (SqlException x)
            {
                MessageBox.Show("", "Ошибка базы данных!\n" + x.Message, MessageBoxButton.OK);
            }
        }
        //нужно для перетаскивания окна
        private void DragMove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
