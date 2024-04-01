using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Kino
{
    /// <summary>
    /// Логика взаимодействия для Film.xaml
    /// </summary>
    public partial class Film : Window
    {
        Entities1 entities = new Entities1();
        public int Id_user { get; set; }
        public Film(string Id_user)
        {
            InitializeComponent();
            lstView.ItemsSource = entities.FILM.ToList();
            tx_vsego.Text = lstView.Items.Count.ToString();
            tx_naideno.Text = "...";

            cmb_poisk.Items.Add("Все");
            cmb_poisk.Items.Add("Детектив");
            cmb_poisk.Items.Add("Фентези");
            cmb_poisk.Items.Add("Драма");
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (Polzovatel.role != "Администратор")
            {
                MessageBox.Show("Отсутствуют права доступа!");
                return;
            }
            if (lstView.SelectedItem == null)
            {
                MessageBox.Show("Выделите запись!");
                return;
            }

            var deletedItem = lstView.SelectedItem as FILM;

            var resalt = MessageBox.Show($"Удалить запись №{deletedItem.Id_film}?", "Удаление", MessageBoxButton.YesNo);
            if (resalt == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    entities.FILM.Remove(deletedItem);
                    entities.SaveChanges();

                    lstView.ItemsSource = entities.FILM.ToList();

                    MessageBox.Show("Удаление прошло успешно", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    tx_vsego.Text = lstView.Items.Count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void tx_poisk_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (tx_poisk.Text != "")
            {
                var items = from item in entities.FILM
                            where item.name_film.ToLower().Contains(tx_poisk.Text.ToLower()) ||
                            item.reziser.ToLower().Contains(tx_poisk.Text.ToLower()) ||
                            item.opisaniey_film.ToLower().Contains(tx_poisk.Text.ToLower())
                            select item;
                lstView.ItemsSource = items.ToList();
                tx_naideno.Text = items.Count().ToString();
                if (items.Count() == 0)
                {
                    MessageBox.Show("Такого фильма нет.");
                }
                return;
            }
            else
            {
                lstView.ItemsSource = entities.FILM.ToList();
                tx_naideno.Text = "...";
            }
        }

        private void kupit_Click(object sender, RoutedEventArgs e)
        {
            PokupkaBiletov window = new PokupkaBiletov(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
        private void redactirovat_Click(object sender, RoutedEventArgs e)
        {
            if (Polzovatel.role != "Администратор")
            {
                MessageBox.Show("Отсутствуют права доступа!");
                return;
            }
            else
            {
                RedactFilm window = new RedactFilm(Id_user.ToString());
                this.Close();
                window.ShowDialog();
            }

        }

        private void cmb_poisk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                string selectedGenre = cmb_poisk.SelectedItem.ToString();

                switch (selectedGenre)
                {
                    case "Все":
                        lstView.ItemsSource = entities.FILM.ToList();
                        break;
                    case "Детектив":
                    case "Фентези":
                    case "Драма":
                        var genreFilms = from film in entities.FILM
                                         where film.zanr == selectedGenre
                                         select film;
                        lstView.ItemsSource = genreFilms.ToList();
                        break;
                    default:
                        MessageBox.Show("Выбран неверный жанр.");
                        break;
                }

                // Подсчет количества найденных фильмов
                int foundFilmsCount = lstView.Items.Count;
                tx_naideno.Text = foundFilmsCount.ToString();

                if (foundFilmsCount == 0)
                {
                    MessageBox.Show("Фильмов данного жанра нет.");
                }
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (Polzovatel.role != "Пользователь")
            {
                Glavnaya window = new Glavnaya(Id_user.ToString());
                this.Close();
                window.ShowDialog();
            }
            else if (Polzovatel.role != "Администратор")
            {

                Admin adminWindow = new Admin(Id_user.ToString());
                this.Close();
                adminWindow.Show(); // Показываем окно для администратора
            }
        }
    }
}

        //string cb = cmb_poisk.SelectedItem.ToString();
        //int index = cb.IndexOf(" ");
        //cb = cb.Remove(0, index + 1);

        //if (cb == "Все")
        //{
        //    foreach (var row in entities.FILM)
        //    {
        //        lstView.Items.Add(row);
        //    }
        //}

        //if (cb == "Детектив")
        //{
        //    var zan = (
        //            from a in entities.FILM
        //            where a.zanr == "Детектив"
        //            select a
        //            ).First<FILM>();

        //    foreach (var fil in entities.FILM)
        //    {
        //        if (fil.Id_film == fil.Id_film)
        //            lstView.Items.Add(fil);
        //    }
        //}
        //if (cb == "Драма")
        //{
        //    var zan = (
        //            from a in entities.FILM
        //            where a.zanr == "Драма"
        //            select a
        //            ).First<FILM>();

        //    foreach (var fil in entities.FILM)
        //    {
        //        if (fil.Id_film == fil.Id_film)
        //            lstView.Items.Add(fil);
        //    }
        //}
        //if (cb == "Фентези")
        //{
        //    var zan = (
        //            from a in entities.FILM
        //            where a.zanr == "Фентези"
        //            select a
        //            ).First<FILM>();

        //    foreach (var fil in entities.FILM)
        //    {
        //        if (fil.Id_film == fil.Id_film)
        //            lstView.Items.Add(fil);
        //    }
        //}



