using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kino
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities1 entities = new Entities1();
        private System.Threading.Timer timer;
        private int seconds = 0;
        public MainWindow()
        {
            InitializeComponent();
            GridCaptcha.Visibility = Visibility.Hidden;
            timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (
              TextBox_Login.Text == "" ||
              PasswordBox_Password.Password == ""
              )
            {
                MessageBox.Show("Заполните все поля!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                bool flag = false;
                foreach (var log in entities.USER)
                {


                    if (TextBox_Login.Text == log.login)
                    {
                        if (PasswordBox_Password.Password == log.parol)
                        {
                            switch (log.role)
                            {
                                case ("Администратор"):
                                    flag = true;
                                    MessageBox.Show("Вы вошли как Администратор");
                                    var window = new Admin(log.Id_user.ToString());
                                    Close();
                                    window.ShowDialog();
                                    break;

                                case ("Пользователь"):
                                    flag = true;
                                    MessageBox.Show("Вы вошли как Пользователь");
                                    var window1 = new Glavnaya(log.Id_user.ToString());
                                    Close();
                                    window1.ShowDialog();
                                    break;

                            }

                        }
                    }


                }
                if (!flag)
                {

                    GridCaptcha.Visibility = Visibility.Visible;
                    GridAuto.Visibility = Visibility.Hidden;
                    sozdenie();

                }
            }
        }
        private void TimerCallback(Object o)
        {
            Button_Vhod.Dispatcher.Invoke(() =>
            {
                Button_Vhod.IsEnabled = true;
                TextBox_Login.BorderBrush = Brushes.Black;
                PasswordBox_Password.BorderBrush = Brushes.Black;
            });
        }
        public void sozdenie()
        {
            Random rand = new Random();
            int num = rand.Next(6, 8);
            string captcha = "";
            int tot1 = 0;
            do
            {
                int chr = rand.Next(48, 123);
                if ((chr >= 48 && chr <= 57) || (chr >= 65 && chr <= 90) || (chr >= 97 && chr <= 122))
                {
                    captcha = captcha + (char)chr;
                    tot1++;
                    if (tot1 == num)
                        break;
                }
            }
            while (true);
            Label_Captcha.Content = captcha;
        }

        private void Button_Check_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_Prverka.Text == Label_Captcha.Content.ToString())
            {


                MessageBox.Show("Неверный логин или пароль,попробуйте снова через 10 секунд", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);

                Button_Vhod.IsEnabled = false; // Делаем кнопку недоступной
                Thread.Sleep(10000); // Устанавливаем таймер на 10 секунд
                TextBox_Login.BorderBrush = Brushes.Red;
                PasswordBox_Password.BorderBrush = Brushes.Red;
                GridCaptcha.Visibility = Visibility.Hidden;
                GridAuto.Visibility = Visibility.Visible;
                Button_Vhod.IsEnabled = true;

            }
            else
            {
                MessageBox.Show("Неверно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                TextBox_Prverka.Clear();
                sozdenie();
            }
        }
    }
}


//        Random rand = new Random();
//        TextBox_Prverka.Text = "";
//        int countChar = 4;
//        for (int i = 0; i <= countChar; i++)
//        {
//            int intChr = rand.Next(48, 93);
//            TextBox_Prverka.Text += (char)intChr;
//        }
//        TextBox_Prverka.Visibility = Visibility.Hidden;
//        GridCaptcha.Visibility = Visibility.Hidden;
//    }

//    private void Button_Click(object sender, RoutedEventArgs e)
//    {
//            string login = TextBox_Login.Text.Trim();
//            var user = new USER();
//            var password = PasswordBox_Password.Password;

//            MD5 md5 = MD5.Create();
//            byte[] bytes = Encoding.ASCII.GetBytes(password);
//            byte[] hash = md5.ComputeHash(bytes);
//            StringBuilder stringBuilder = new StringBuilder();
//            foreach (var i in hash)
//            {
//                stringBuilder.Append(i.ToString("X2"));
//            }
//            password = stringBuilder.ToString();
//            try
//            {
//                user = entities.USER.Where(p => p.login == login && p.parol == password).First();
//            }
//            catch (Exception ex)
//            {
//                //MessageBox.Show(ex.Message);
//                //return;
//            }

//            if (GridCaptcha.IsEnabled == true)
//            {
//                if (CAPCHA.Text != TextBox_Prverka.Text)
//                {
//                    MessageBox.Show("Неверная captcha!");

//                    Random rand = new Random();
//                    CAPCHA.Text = "";
//                    int countChar = 4;
//                    for (int i = 0; i <= countChar; i++)
//                    {
//                        int intChr = rand.Next(48, 93);
//                        CAPCHA.Text += (char)intChr;
//                    }

//                    MessageBox.Show($"Вход запрещен на 10 секунд.");
//                    Thread.Sleep(10000);
//                    MessageBox.Show($"10 секунд истекли, повторите попытку.");

//                    return;
//                }
//            }

//            if (GridCaptcha.IsEnabled == true)
//            {
//                if (CAPCHA.Text != TextBox_Prverka.Text)
//                {
//                    MessageBox.Show("Неверная captcha!");

//                    Random rand = new Random();
//                    CAPCHA.Text = "";
//                    int countChar = 4;
//                    for (int i = 0; i <= countChar; i++)
//                    {
//                        int intChr = rand.Next(48, 93);
//                        CAPCHA.Text += (char)intChr;
//                    }

//                    MessageBox.Show($"Вход запрещен на 10 секунд.");
//                    Thread.Sleep(10000);
//                    MessageBox.Show($"10 секунд истекли, повторите попытку.");

//                    return;
//                }
//            }
//            int userCount = entities.USER.Where(p => p.login == login && p.parol == password).Count();

//            if (userCount > 0)
//            {
//                if (user != null)
//                {
//                    MessageBox.Show("Вы подключились как " + user.role);
//                // Получаем Id текущего пользователя

//                    int selectedId = user.Id_user;
//                    if (user.role == "Администратор")
//                    {
//                        var win = new Glavnaya(Id_user.ToString());
//                        this.Close();
//                        win.ShowDialog();
//                    }
//                    else if (user.role == "Пользователь")
//                    {
//                        var win = new Admin(Id_user.ToString());
//                        this.Close();
//                        win.ShowDialog();
//                    }
//                    DateTime entryTime = DateTime.Now;

//                    USER_HISTIRY newEntry = new USER_HISTIRY
//                    {
//                        time_VHOD = entryTime,
//                        Id_user = selectedId
//                    };

//                    entities.USER_HISTIRY.Add(newEntry);
//                    entities.SaveChanges();
//                }
//            }
//            else
//            {
//                MessageBox.Show("Неверный логин или пароль, попробуйте еще раз.");
//                GridCaptcha.IsEnabled = true;
//                CAPCHA.Visibility = Visibility.Visible;
//                GridCaptcha.Visibility = Visibility.Visible;
//            }
//        }
//    }
//}







//        int loginAttempts = 0;
//        bool captchaRequired = false;
//        Random random = new Random();

//        public MainWindow()
//        {
//            InitializeComponent();
//            HideCaptcha();
//        }
//        private void ShowCaptcha()
//        {
//            grid_cptch.Visibility = Visibility.Visible;
//            txt_cptch.Text = GenerateRandomCaptcha();
//        }

//        private string GenerateRandomCaptcha()
//        {
//            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//            return new string(Enumerable.Repeat(chars, 6)
//                .Select(s => s[random.Next(s.Length)]).ToArray());
//        }

//        private void HideCaptcha()
//        {
//            grid_cptch.Visibility = Visibility.Collapsed;
//        }

//        private bool VerifyCaptcha()
//        {
//            return txt_cptch.Text == cptch_box.Text;
//        }

//        private void Button_enter_Click(object sender, RoutedEventArgs e)
//        {
//            string login = login_box.Text.Trim();
//            string password = password_box.Password;

//            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
//            {
//                MessageBox.Show("Пожалуйста, заполните все поля.");
//                return;
//            }

//            if (captchaRequired)
//            {
//                if (!VerifyCaptcha())
//                {
//                    MessageBox.Show("Неверная captcha! Попробуйте еще раз.");
//                    BlockWindowForSeconds(15); // Блокировка на 15 секунд после неправильной капчи
//                    return;
//                }
//                else
//                {
//                    captchaRequired = false;
//                }
//            }

//            var user = entities.USER.FirstOrDefault(u => u.login == login && u.parol == password);

//            if (user != null)
//            {
//                if (user.role == "Пользователь")
//                {
//                    MessageBox.Show("Авторизация успешна как пользователь.");
//                    // Переход на окно "Главная" для пользователя
//                    var Glavnaya = new Glavnaya();
//                    this.Close();
//                    Glavnaya.ShowDialog();
//                }
//                else if (user.role == "Администратор")
//                {
//                    MessageBox.Show("Авторизация успешна как администратор.");
//                    // Переход на окно "Админ" для администратора
//                    var Admin = new Admin();
//                    this.Close();
//                    Admin.ShowDialog();
//                }
//            }
//            else
//            {
//                loginAttempts++;
//                if (loginAttempts >= 2)
//                {
//                    ShowCaptcha(); // Показываем капчу после двух неудачных попыток входа
//                }
//                else
//                {
//                    MessageBox.Show("Неверный логин или пароль. Попробуйте еще раз.");
//                }
//            }
//        }

//        private void BlockWindowForSeconds(int seconds)
//        {
//            grid_cptch.IsEnabled = false;
//            txt_cptch.Visibility = Visibility.Hidden;
//            Button_enter.IsEnabled = false;
//            Task.Delay(TimeSpan.FromSeconds(seconds)).ContinueWith(_ =>
//            {
//                Dispatcher.Invoke(() =>
//                {
//                    Button_enter.IsEnabled = true;
//                    ShowCaptcha(); // Показываем капчу после блокировки
//                });
//            });
//        }
//    }
//}


