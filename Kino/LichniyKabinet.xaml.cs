using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Kino
{
    /// <summary>
    /// Логика взаимодействия для LichniyKabinet.xaml
    /// </summary>
    public partial class LichniyKabinet : Window
    {
        Entities1 entities = new Entities1();
        public int Id_user { get; set; }
        public LichniyKabinet(string Id_user)
        {
            InitializeComponent();
            foreach (var us in entities.USER)
            {
                LbPOlz.Items.Add(us);
            }
            Rol.Items.Add("Администратор");
            Rol.Items.Add("Пользователь");
        }
        private void LbPOlz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rol.SelectedItem = null;
            NewParol.Text = null;
            var selectedItem = LbPOlz.SelectedItem as USER;
            if (selectedItem != null)
            {
                firstNameTextBox.Text = selectedItem.name_user.ToString();
                lastNameTextBox.Text = selectedItem.surname_user.ToString();
                emailTextBox.Text = selectedItem.login.ToString();
                phoneTextBox.Text = selectedItem.phone.ToString();
                dateOfBirthPicker.SelectedDate = selectedItem.data_bith;
                NewParol.Text = selectedItem.parol.ToString();
                Rol.SelectedItem = selectedItem.role;
            }
            else
            {
                firstNameTextBox.Text = null; 
                lastNameTextBox.Text = null;
                emailTextBox.Text = null;
                phoneTextBox.Text = null;
                dateOfBirthPicker.SelectedDate = null;
                NewParol.Text = null;
                Rol.SelectedItem = null;
            }
        }

        private void Bsave_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = LbPOlz.SelectedItem as USER;
            if (NewParol.Text == "" && Shifr.Text == "")
            {
                MessageBox.Show("Заполните поле пароля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (emailTextBox.Text == "" || firstNameTextBox.Text == "" || lastNameTextBox.Text == "" || phoneTextBox.Text == "" || dateOfBirthPicker.SelectedDate == null || Rol.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                // Добавление проверки на формат даты и ограничение на год
                if (!DateTime.TryParse(dateOfBirthPicker.Text, out DateTime dateOfBirth) || dateOfBirth.Year > 2018)
                {
                    MessageBox.Show("Пожалуйста, введите корректную дату рождения (не позднее 2018 года).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Добавление проверки на формат мобильного телефона
                Regex phoneRegex = new Regex(@"^\+\d{1,3}\d{3}\d{3}\d{2}\d{2}$");
                if (!phoneRegex.IsMatch(phoneTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите корректный номер мобильного телефона в формате +XXXXXXXXXXX.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedItem == null)
                {
                    selectedItem = new USER();
                    entities.USER.Add(selectedItem);
                    LbPOlz.Items.Add(selectedItem);
                }
                selectedItem.name_user = firstNameTextBox.Text;
                selectedItem.surname_user = lastNameTextBox.Text;
                selectedItem.login = emailTextBox.Text;
                selectedItem.phone = phoneTextBox.Text;
                selectedItem.data_bith = dateOfBirthPicker.SelectedDate;
                selectedItem.parol = NewParol.Text;
                selectedItem.role = Rol.SelectedItem.ToString();

                if (NewParol.Text != "")
                {
                    // Хеширование пароля 
                    MD5 md5 = MD5.Create();
                    byte[] bytes = Encoding.ASCII.GetBytes(NewParol.Text);
                    byte[] hash = md5.ComputeHash(bytes);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var byte_ in hash)
                    {
                        stringBuilder.Append(byte_.ToString("X2"));
                    }
                    selectedItem.parol = stringBuilder.ToString();
                    Shifr.Text = stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LbPOlz.Items.Remove(selectedItem);
                return;
            }
            entities.SaveChanges();
            LbPOlz.Items.Refresh();
            MessageBox.Show("Cохранено!");
        }

        private void Bback_Click(object sender, RoutedEventArgs e)
        {
            Glavnaya window = new Glavnaya(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }

        private void Bdelete_Click(object sender, RoutedEventArgs e)
        {
            if (Polzovatel.role == "Администратор")
            {
                MessageBox.Show("У вас нет прав на это действие!");
                return;
            }
            var selectedItem = LbPOlz.SelectedItem as USER;

                if (selectedItem == null)
                {
                    MessageBox.Show("Выберите пользователя для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    entities.USER.Remove(selectedItem);
                    LbPOlz.Items.Remove(selectedItem);
                    entities.SaveChanges();
                    MessageBox.Show("Пользователь удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            
        }

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Polzovatel.role == "Администратор")
            {
                MessageBox.Show("У вас нет прав на это действие!");
                return;
            }
            LbPOlz.SelectedItem = null;

            firstNameTextBox.Text = null; ;
            lastNameTextBox.Text = null;
            emailTextBox.Text = null;
            phoneTextBox.Text = null;
            dateOfBirthPicker.SelectedDate = null;
            NewParol.Text = null;
            Rol.SelectedItem = null;
        }

        private void BClear_Click(object sender, RoutedEventArgs e)
        {
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";
            dateOfBirthPicker.SelectedDate = null;
            NewParol.Text = "";
            Rol.SelectedIndex = -1;
        }
    }
}

        //    var selectedItem = LbPOlz.SelectedItem as USER;
        //    if (NewParol.Text == "" && Shifr.Text == "")
        //    {
        //        MessageBox.Show("Заполните поле пароля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }
        //    if (emailTextBox.Text == "" || firstNameTextBox.Text == "" || lastNameTextBox.Text == "" || 
        //        phoneTextBox.Text == null || dateOfBirthPicker.SelectedDate == null ||
        //        Rol.SelectedItem == null)
        //    {
        //        MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }
        //    try
        //    {
        //        if (selectedItem == null)
        //        {
        //            selectedItem = new USER();
        //            entities.USER.Add(selectedItem);
        //            LbPOlz.Items.Add(selectedItem);
        //        }
        //        selectedItem.name_user = firstNameTextBox.Text;
        //        selectedItem.surname_user = lastNameTextBox.Text;
        //        selectedItem.login = emailTextBox.Text;
        //        selectedItem.phone = phoneTextBox.Text ;
        //        selectedItem.data_bith = dateOfBirthPicker.SelectedDate;
        //        selectedItem.parol = NewParol.Text ;
        //        selectedItem.role = Rol.SelectedItem.ToString();

        //        if (NewParol.Text != null)
        //        {
    //    MD5 md5 = MD5.Create();
    //    byte[] bytes = Encoding.ASCII.GetBytes(NewParol.Text);
    //    byte[] hash = md5.ComputeHash(bytes);
    //    StringBuilder stringBuilder = new StringBuilder();
    //                foreach (var byte_ in hash)
    //                {
    //                    stringBuilder.Append(byte_.ToString("X2"));
    //                }
    //selectedItem.parol = stringBuilder.ToString();
    //                Shifr.Text = stringBuilder.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        LbPOlz.Items.Remove(selectedItem);
        //        return;
        //    }
        //    entities.SaveChanges();
        //    LbPOlz.Items.Refresh();
        //    MessageBox.Show("Cохранено!");
        //}

//    }
//}





