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
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorldOfGames
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public Login(string newUsername, string newPassword)
        {
            InitializeComponent();
            username.Text = newUsername;
            password.Password = newPassword;
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text.ToString() == "" || password.Password.ToString() == "")
            {
                MessageBox.Show("", "Ошибка регистрации!\nВведи все поля пожалуйста", MessageBoxButton.OK);
                return;
            }
            try
            {
                bool check = false;
                //search функция из dll, возвращает user если такой есть в базе данных
                Users user = Users.Search(username.Text, password.Password);
                if (user != null) {
                    check = true;
                    MainWindow main = new MainWindow(user);
                    main.Show();
                    this.Close();
                }
                if (!check)
                {
                    MessageBox.Show("", "Ошибка авторизации!\nИзвини, мы не нашли тебя в нашем магазине\nПопробуй зарегистрироваться", MessageBoxButton.OK);
                }
            }
            catch (SqlException x)
            {
                MessageBox.Show("", "Ошибка базы данных!\n" + x.Message, MessageBoxButton.OK);
            }
        }
        //необходимо для перетаскивания окна
        private void DragMove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
