using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Security.Cryptography;
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

namespace Kino
{
    /// <summary>
    /// Логика взаимодействия для PokupkaBiletov.xaml
    /// </summary>
    public partial class PokupkaBiletov : Window
    {
        Entities1 entities = new Entities1();
        public int Id_user { get; set; }
        public PokupkaBiletov(string Id_user)
        {
            InitializeComponent();
            foreach (var tipzal in entities.ZAL)
            {
                cmbHall.Items.Add(tipzal);
            }
            foreach (var filmm in entities.FILM)
            {
                cmbFilms.Items.Add(filmm);
            }
            foreach (var pokup in entities.POKUPKA_BILETA)
            {
                lstTickets.Items.Add(pokup);
            }
        }

        private void lstTickets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstTickets.SelectedItem = null;
            var selectedItem = lstTickets.SelectedItem as POKUPKA_BILETA;
            if (selectedItem != null)
            {
                txtMesto.Text = selectedItem.mesto.ToString();
                txtPrice.Text = selectedItem.price.ToString();
                data_filma.SelectedDate = selectedItem.data_time_pokup;

                var filmComboBoxItem = cmbFilms.Items.OfType<ComboBoxItem>()
                    .FirstOrDefault(item => (item.Content as FILM) == selectedItem.FILM);
                cmbFilms.SelectedItem = filmComboBoxItem;
                var hallComboBoxItem = cmbHall.Items.OfType<ComboBoxItem>()
                    .FirstOrDefault(item => (item.Content as ZAL) == selectedItem.ZAL);
                cmbHall.SelectedItem = hallComboBoxItem;
            }
            else
            {
                txtMesto.Text = null;
                txtPrice.Text = "300";
                data_filma.SelectedDate = null;
                cmbFilms.SelectedItem = null;
                cmbHall.SelectedItem = null;
            }
        }
        private void Kupit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lstTickets.SelectedItem as POKUPKA_BILETA;
            if (txtMesto.Text == "" || txtPrice.Text == "" || cmbFilms.SelectedItem == null ||
                cmbHall.SelectedItem == null || data_filma.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedHall = cmbHall.SelectedItem as ZAL;
            int maxMesto;
            switch (selectedHall.tip_zal)
            {
                case "VIP-зал":
                    maxMesto = 20;
                    break;
                case "Детский зал":
                    maxMesto = 50;
                    break;
                default:
                    maxMesto = 100;
                    break;
            }
            try
            {
                if (selectedItem == null)
                {
                    selectedItem = new POKUPKA_BILETA();
                    entities.POKUPKA_BILETA.Add(selectedItem);
                    lstTickets.Items.Add(selectedItem);
                }
                if (int.Parse(txtMesto.Text) > maxMesto)
                {
                    MessageBox.Show($"Максимальное количество мест для зала \"{selectedHall.tip_zal}\" " +
                        $"- {maxMesto}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(txtMesto.Text, out int mesto))
                {
                    MessageBox.Show("Поле \"Место\" должно содержать только цифры!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (txtPrice.Text != "300")
                {
                    MessageBox.Show("Цена билета должна быть равна 300!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!DateTime.TryParse(data_filma.Text, out DateTime date) || date.Year > 2024)
                {
                    MessageBox.Show("Некорректная дата фильма! Пожалуйста, введите дату в формате дд.мм.гггг и не позднее 2024 года.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                selectedItem.data_time_pokup = data_filma.SelectedDate;
                selectedItem.price = int.Parse(txtPrice.Text);
                selectedItem.mesto = int.Parse(txtMesto.Text); 
                selectedItem.Id_film = (cmbFilms.SelectedItem as FILM).Id_film;
                selectedItem.Id_zal = (cmbHall.SelectedItem as ZAL).Id_zal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                lstTickets.Items.Remove(selectedItem);
                return;
            }
            entities.SaveChanges();
            lstTickets.Items.Refresh();
            MessageBox.Show("Cохранено!");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lstTickets.SelectedItem as POKUPKA_BILETA;

            if (selectedItem == null)
            {
                MessageBox.Show("Выберите билет для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                entities.POKUPKA_BILETA.Remove(selectedItem);
                lstTickets.Items.Remove(selectedItem);
                entities.SaveChanges();
                MessageBox.Show("Билет удален!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtMesto.Text = "";
            txtPrice.Text = "300";
            data_filma.SelectedDate = null;
            cmbFilms.SelectedIndex = -1;
            cmbHall.SelectedIndex = -1;
            lstTickets.SelectedIndex = -1;
        }
        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            Glavnaya window = new Glavnaya(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
    }
}
//lstTickets.SelectedItem = null;
//var selectedItem = lstTickets.SelectedItem as POKUPKA_BILETA;
//if (selectedItem != null)
//{
//    txtMesto.Text = selectedItem.mesto.ToString();
//    txtPrice.Text = selectedItem.price.ToString();
//    data_filma.SelectedDate = selectedItem.data_time_pokup;

//    cmbFilms.SelectedIndex = selectedItem.Id_film -1;
//    cmbFilms.SelectedItem = selectedItem.FILM;

//    cmbHall.SelectedIndex = selectedItem.Id_zal - 1;
//    cmbHall.SelectedItem = selectedItem.ZAL;


//var selectedItem = lstTickets.SelectedItem as POKUPKA_BILETA;
//if (txtMesto.Text == "" || txtPrice.Text == "" || cmbFilms.SelectedItem == null ||
//    cmbHall.SelectedItem == null || data_filma.SelectedDate == null)
//{
//    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
//    return;
//}
//try
//{
//    if (selectedItem == null)
//    {
//        selectedItem = new POKUPKA_BILETA();
//        entities.POKUPKA_BILETA.Add(selectedItem);
//        lstTickets.Items.Add(selectedItem);
//    }
//    selectedItem.mesto = txtMesto.Text;
//    selectedItem.price = txtPrice.Text;
//    selectedItem.data_time_pokup = data_filma.SelectedDate;
//    selectedItem.Id_film = (cmbFilms.SelectedItem as FILM).Id_film;
//    selectedItem.Id_zal = (cmbHall.SelectedItem as ZAL).Id_zal;
//}
//catch (Exception ex)
//{
//    MessageBox.Show(ex.Message);
//    lstTickets.Items.Remove(selectedItem);
//    return;
//}
//entities.SaveChanges();
//lstTickets.Items.Refresh();
//MessageBox.Show("Cохранено!");




