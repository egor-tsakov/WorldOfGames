using System;
using System.Collections.Generic;
using System.Data.Linq;
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

namespace WorldOfGames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double hRect; //нужно для рисования списка игр
        private double wRect; //нужно для рисования списка игр
        private int money; //нужно для отображения общей цены в корзине
        Users user; //пользователь под которым залогинился пользователь

        public MainWindow(Users user)
        {
            this.user = user;
            InitializeComponent();
            InitializeStore();
            InitializeBasket();
            labelHomeControl.Text = "Я рад приветствовать тебя в своем магазине\n"+ user.username.ToString() +"\nЗдесь много интересных игр для тебя\nТы обязательно найдешь то, что искал";
        }


        //общая инициализация всех скрытых и видимых окон, необходима чтобы меньше кода писать
        //number нужен для случая когда боковое меню раскрываем или скрываем
        private void InitializeApp(int number)
        {
            storeControl.Width = Content.ActualWidth + number;
            basketControl.Width = Content.ActualWidth + number;
            labraryControl.Width = Content.ActualWidth + number;
            userControl.Width = Content.ActualWidth + number;
            storeControl.Height = Content.ActualHeight;
            basketControl.Height = Content.ActualHeight;
            labraryControl.Height = Content.ActualHeight;
            userControl.Height = Content.ActualHeight;
            InitializeStore();
            InitializeBasket();
            InitializeLabrary();
            InitializeUser();
        }
        //инициализация корзины
        private void InitializeBasket()
        {
            //чистим, чтобы перерисовать
            listBasketControl.Children.Clear();
            money = 0;
            //перерисовываем
            if (user.basket != null)
            {
                string[] ids = (user.basket).Split(' ');
                foreach (var id in ids)
                {
                    if (id != "") listBasketControl.Children.Add(elementList(id + ".jpg", 2,""));
                    Table<Games> games = Games.GetTable();
                    foreach (var game in games)
                    {
                        if (id == game.Id.ToString()) money += game.price;
                    }
                }
                butPrice.Content = "купить все, за " + money.ToString();
                //делаем в конце списка пустое пространство, без этого баги отображения наверно будут, они были в другом пробном проекте:)
                listBasketControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
                listBasketControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
                listBasketControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
                listBasketControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
            }
        }
        //инициализация магазина(список игр)
        private void InitializeStore()
        {
            //чистим, чтобы перерисовать
            listStoreControl.Children.Clear();
            //метод из dll, получаем таблицу игр и перерисовываем
            Table<Games> games = Games.GetTable();
            foreach (var game in games)
            {
                listStoreControl.Children.Add(elementList(game.sourse.ToString(), 1, game.price.ToString().Trim()));
            }
            //делаем в конце списка пустое пространство, без этого баги отображения наверно будут, они были в другом пробном проекте:)
            listStoreControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
            listStoreControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
            listStoreControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
            listStoreControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
        }
        //инициализация библиотеки(список игр пользователя, которые он уже себе купил)
        private void InitializeLabrary()
        {
            //чистим, чтобы перерисовать
            listLabraryControl.Children.Clear();
            //перерисовываем
            if (user.labrary != null)
            {
                string[] ids = (user.labrary).Split(' ');
                foreach (var id in ids)
                {
                    if (id != "") listLabraryControl.Children.Add(elementList(id + ".jpg", 3, ""));
                }
                //делаем в конце списка пустое пространство, без этого баги отображения наверно будут, они были в другом пробном проекте:)
                listLabraryControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
                listLabraryControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
                listLabraryControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
                listLabraryControl.Children.Add(new Rectangle { Width = wRect, Height = hRect, Fill = null });
            }
        }
        //инициализация окна редактирования пользователя
        private void InitializeUser()
        {
            //тут все просто
            userControlUsername.Text = user.username.Trim();
            userControlPassword.Text = user.password.Trim();
            if (user.email == null) user.email = ""; // делаем на всякий случай чтобы не было ошибок (костыль)
            userControlEmail.Text = user.email.Trim();
            userControlMoney.Text = user.money.ToString();
        }

        //елемент списка, используется везде где рисуется список игр
        //sourse - название картнки
        //type - влияет какая кнопка будет добавлена в stackpanek, либо удалить или добавить товар, либо вообще без этой кнопки
        //price - цена, используется не всегда
        private StackPanel elementList(string sourse, int type, string price)
        {
            //коротко что делает эта функция, делаем элемент stackpanel, запихиваем в него картинку и кнопки
            //ах да, контент окно, в строке содержит по 4 элемента всегда!!!.
            double k = 0.46; // отношение ширины и высоты моих картинок (округленно, расширение 460х215 вроде)
            StackPanel element = new StackPanel();
            element.Width = this.storeControl.Width / 4 - 10;

            Image image = new Image();
            image.Width = element.Width;
            image.Height = image.Width * k + 5;
            image.Source = new BitmapImage(new Uri(@"Image/StoreImages/" + sourse, UriKind.Relative));

            Button button1 = new Button { Width = element.Width, Height = 30, Content = "подробнее" + price, Style = (Style)App.Current.FindResource("defButtonList") };
            button1.AddHandler(Button.ClickEvent, new RoutedEventHandler(detailsButList_MouseLeftButtonDown));
            Button button2;

            switch (type)
            {
                case 1:
                    button2 = new Button { Width = element.Width, Height = 30, Content = "добавить в корзину", Style = (Style)App.Current.FindResource("defButtonList") };
                    button2.AddHandler(Button.ClickEvent, new RoutedEventHandler(addButList_MouseLeftButtonDown));
                    element.Height = image.Height + button1.Height + button2.Height + 10;
                    element.Margin = new Thickness(5, 5, 5, 5);

                    element.Children.Add(image);
                    element.Children.Add(button1);
                    element.Children.Add(button2);
                    break;
                case 2:
                    button2 = new Button { Width = element.Width, Height = 30, Content = "удалить из корзины", Style = (Style)App.Current.FindResource("defButtonList") };
                    button2.AddHandler(Button.ClickEvent, new RoutedEventHandler(delButList_MouseLeftButtonDown));
                    element.Height = image.Height + button1.Height + button2.Height + 10;
                    element.Margin = new Thickness(5, 5, 5, 5);

                    element.Children.Add(image);
                    element.Children.Add(button1);
                    element.Children.Add(button2);
                    break;
                default:
                    element.Height = image.Height + button1.Height + 10;
                    element.Margin = new Thickness(5, 5, 5, 5);

                    element.Children.Add(image);
                    element.Children.Add(button1);
                    break;
            }
            //требуется для рисования пустого пространства вверху
            hRect = element.Height;
            wRect = element.Width;
            return element;
        }

        #region кнопки в list
        //кнопка подробне на элементах списка, увы я не сделал
        private void detailsButList_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("подробнее", "Я пока не сделал эту часть приложения", MessageBoxButton.OK);
        }
        //кнопка добавить в корзину на элементах списка
        private void addButList_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //этим говнокодом я получаю название картинки и от туду извлекаю id, так как картинки у меня пронумерованы так же как id игр
            int id = Int32.Parse(((((Image)((StackPanel)((Button)sender).Parent).Children[0]).Source.ToString()).Split('/')[6]).Split('.')[0]);
            if (user.basket == null) user.basket = "";
            user.basket = user.basket.Trim() + " " + id.ToString();
            //функция из dll, обновляет запись в таблице, этого пользователя в аргументе
            Users.Update(user);
            MessageBox.Show("", "Товар добавлен в корзину", MessageBoxButton.OK);
        }
        //удаление из карзины на элементах списка
        private void delButList_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //этим говнокодом я получаю название картинки и от туду извлекаю id, так как картинки у меня пронумерованы так же как id игр
            int id = Int32.Parse(((((Image)((StackPanel)((Button)sender).Parent).Children[0]).Source.ToString()).Split('/')[6]).Split('.')[0]);
            string[] basket = (user.basket.Trim()).Split(' ');
            string newBasket = "";
            foreach (string x in basket)
            {
                if (x != id.ToString()) newBasket += " " + x.ToString();
            }
            user.basket = newBasket.Trim();
            //функция из dll, обновляет запись в таблице, этого пользователя в аргументе
            Users.Update(user);
            InitializeBasket();
        }
        //кнопка купить весь товар, на стрнице корзина внизу
        private void butPrice_Click(object sender, RoutedEventArgs e)
        {
            if (user.basket != null)
            {
                if (user.money >= money)
                {
                    if (user.labrary == null) user.labrary = "";
                    user.labrary = user.labrary.Trim() + " " + user.basket.Trim();
                    user.basket = null;
                    user.money = user.money - money;
                    //функция из dll, обновляет запись в таблице, этого пользователя в аргументе
                    Users.Update(user);
                    money = 0;
                    InitializeBasket();
                    return;
                } else
                {
                    MessageBox.Show("", "Недостаточно средств, покупка не возможна", MessageBoxButton.OK);
                    return;
                }
            }
            MessageBox.Show("", "Корзина пуста, покупка не возможна", MessageBoxButton.OK);
        }
        //кнопка обновить пользователя на странице редактирования пользователя
        private void userControlUpdate_Click(object sender, RoutedEventArgs e)
        {
            user.username = userControlUsername.Text.ToString().Trim();
            user.password = userControlPassword.Text.ToString().Trim();
            user.email = userControlEmail.Text.ToString().Trim();
            user.money = Int32.Parse(userControlMoney.Text);
            //функция из dll, обновляет запись в таблице, этого пользователя в аргументе
            Users.Update(user);
            MessageBox.Show("", "Пользователь обновлен", MessageBoxButton.OK);
            InitializeUser();
        }
        //удалить аккаунт на странице редактирования пользователя
        private void userControlDelete_Click(object sender, RoutedEventArgs e)
        {
            //функция из dll, удаляет данного пользователя
            Users.DeleteUser(this.user);
            MessageBox.Show("", "Пользователь удален", MessageBoxButton.OK);
            Login log = new Login();
            log.Show();
            this.Close();
        }

        #endregion

        //функция раотает когда мы зажимаем кнопку мыши на верней полоске, и делает перетаскивание окна
        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        //кнопка раскрывает боковое меню
        private void openAsideButton_Click(object sender, RoutedEventArgs e)
        {
            openAsideButton.Visibility = Visibility.Collapsed;
            closeAsideButton.Visibility = Visibility.Visible;
            InitializeApp(-100);
        }
        //кнопка скрывает боковое меню
        private void closeAsideButton_Click(object sender, RoutedEventArgs e)
        {
            openAsideButton.Visibility = Visibility.Visible;
            closeAsideButton.Visibility = Visibility.Collapsed;
            InitializeApp(100);
        }



        #region боковое меню
        //логика везде одинаковая, просто показывает или скрывает окна, которые выбраны в боковом меню
        private void ListViewItemHome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            homeControl.Visibility = Visibility.Visible;
            storeControl.Visibility = Visibility.Collapsed;
            basketControl.Visibility = Visibility.Collapsed;
            labraryControl.Visibility = Visibility.Collapsed;
            userControl.Visibility = Visibility.Collapsed;
        }
        private void ListViewItemStore_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitializeStore();
            homeControl.Visibility = Visibility.Collapsed;
            storeControl.Visibility = Visibility.Visible;
            basketControl.Visibility = Visibility.Collapsed;
            labraryControl.Visibility = Visibility.Collapsed;
            userControl.Visibility = Visibility.Collapsed;
        }
        private void ListViewItemLabrary_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitializeLabrary();
            basketControl.Visibility = Visibility.Collapsed;
            homeControl.Visibility = Visibility.Collapsed;
            storeControl.Visibility = Visibility.Collapsed;
            labraryControl.Visibility = Visibility.Visible;
            userControl.Visibility = Visibility.Collapsed;
        }
        private void ListViewItemBasket_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitializeBasket();
            basketControl.Visibility = Visibility.Visible;
            homeControl.Visibility = Visibility.Collapsed;
            storeControl.Visibility = Visibility.Collapsed;
            labraryControl.Visibility = Visibility.Collapsed;
            userControl.Visibility = Visibility.Collapsed;
        }
        private void ListViewItemUser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitializeUser();
            userControl.Visibility = Visibility.Visible;
            basketControl.Visibility = Visibility.Collapsed;
            homeControl.Visibility = Visibility.Collapsed;
            storeControl.Visibility = Visibility.Collapsed;
            labraryControl.Visibility = Visibility.Collapsed;
        }
        private void ListViewItemClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Close();
        }
        #endregion
        #region кнопки верхне правого угла
        //тут все просто, 3 кнопки (закрыть, свернуть, развернуть)
        private void MinWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void MaxWindow_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;

            InitializeApp(0);
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
