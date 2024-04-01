using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Логика взаимодействия для RedactFilm.xaml
    /// </summary>
    public partial class RedactFilm : Window
    {
        Entities1 entities = new Entities1();
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        public int Id_user { get; set; }
        public RedactFilm(string Id_user)

        {
            InitializeComponent();

            foreach (var fil in entities.FILM)
            {
                lbFilm.Items.Add(fil);
            }
        }
        private void lbFilm_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var selection = lbFilm.SelectedItem as FILM;
            if (selection != null)
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(projectDirectory + "\\Photo\\" + selection.photo);
                myBitmapImage.DecodePixelWidth = 200;
                myBitmapImage.EndInit();
                IImage.Source = myBitmapImage;

                tbNameFilm.Text = selection.name_film.ToString();
                tbGodVipuska.Text = selection.year_vipuska.ToString();
                tbReziser.Text = selection.reziser.ToString();
                tbZanr.Text = selection.zanr.ToString();
                tbReit.Text = selection.reiting.ToString();
                tbOpis.Text = selection.opisaniey_film.ToString();
                time.Text = selection.time_film.ToString();

            }
            else
            {
                lbFilm.SelectedIndex = -1;
                tbNameFilm.Text = "";
                tbReziser.Text = "";
                tbZanr.Text = "";
                tbOpis.Text = "";
            }
        }
        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            var item = lbFilm.SelectedItem as FILM;
            if (string.IsNullOrWhiteSpace(tbGodVipuska.Text) ||
                string.IsNullOrWhiteSpace(tbOpis.Text) ||
                string.IsNullOrWhiteSpace(tbNameFilm.Text) ||
                IImage.Source == null ||
                string.IsNullOrWhiteSpace(tbNameFilm.Text) ||
                string.IsNullOrWhiteSpace(tbReit.Text) ||
                string.IsNullOrWhiteSpace(tbZanr.Text) ||
                string.IsNullOrWhiteSpace(time.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Проверка поля tbGodVipuska
            if (!int.TryParse(tbGodVipuska.Text, out int year) || year < 1980 || year > 2024)
            {
                MessageBox.Show("Введите число от 1980 до 2024 в поле год выпуска", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Проверка поля tbReit.Text
            if (!double.TryParse(tbReit.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double reit) || reit < 1.0 || reit > 5.0)
            {
                MessageBox.Show("Введите число от 1,0 до 5,0 с запятой в поле рейтинга", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Проверка поля time
            if (!int.TryParse(time.Text, out int timeValue) || timeValue < 80 || timeValue > 320)
            {
                MessageBox.Show("Введите число от 80 до 320 в поле времени", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (item == null)
            {
                item = new FILM();
                entities.FILM.Add(item);
                lbFilm.Items.Add(item);
            }

            string fullFileName = IImage.Source.ToString();
            fullFileName = fullFileName.Replace(@"file:///", "");
            FileInfo fileInfo = new FileInfo(fullFileName);
            item.photo = fileInfo.Name;

            try
            {
                item.name_film = tbNameFilm.Text;
                item.reziser = tbReziser.Text;
                item.year_vipuska = int.Parse(tbGodVipuska.Text);
                item.zanr = tbZanr.Text;
                item.reiting = int.Parse(tbReit.Text);
                item.time_film = int.Parse(time.Text);
                item.opisaniey_film = tbOpis.Text;
                item.opisaniey_film = tbOpis.Text;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            entities.SaveChanges();
            lbFilm.Items.Refresh();
            MessageBox.Show("Готово", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void bDobPhoto_Click(object sender, RoutedEventArgs e)
        {
            var selected_item = lbFilm.SelectedItem as FILM;
            OpenFileDialog mer = new OpenFileDialog();

            mer.InitialDirectory = projectDirectory + "\\Photo\\";

            if (mer.ShowDialog() == true && !string.IsNullOrWhiteSpace(mer.FileName))
                IImage.Source = new BitmapImage(new Uri(mer.FileName));
            try
            {
                File.Copy(mer.FileName, mer.InitialDirectory + mer.SafeFileName);
                MessageBox.Show("Ошибка");
            }
            catch
            {

            }
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            var resalt = MessageBox.Show("Точно удалить?", "Удаление", MessageBoxButton.YesNo);
            if (resalt == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                var deletedItem = lbFilm.SelectedItem as FILM;
                if (deletedItem != null)
                {

                    try
                    {
                        entities.FILM.Remove(deletedItem);
                        entities.SaveChanges();
                        lbFilm.Items.Remove(deletedItem);
                        lbFilm.Items.Refresh();

                        tbGodVipuska.Text = "";
                        IImage.Source = null;
                        lbFilm.SelectedIndex = -1;
                        tbNameFilm.Text = "";
                        tbReziser.Text = "";
                        tbZanr.Text = "";
                        tbOpis.Text = "";
                        time.Text = "";
                        tbReit.Text = "";


                        MessageBox.Show("Удаление прошло успешно", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Эти данные используют другие таблицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                }
            }
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            IImage.Source = new BitmapImage();
            tbGodVipuska.Text = "";
            IImage.Source = null;
            lbFilm.SelectedIndex = -1;
            tbNameFilm.Text = "";
            tbReziser.Text = "";
            tbZanr.Text = "";
            tbOpis.Text = "";
            time.Text = "";
            tbReit.Text = "";
        }

        private void Back_to_vhod_Click(object sender, RoutedEventArgs e)
        {
            Admin window = new Admin(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
    }
    //        var item = lbFilm.SelectedItem as FILM;
    //        if (
    //            tbGodVipuska.Text == "" ||
    //            tbOpis.Text == "" ||
    //            tbNameFilm.Text == "" ||
    //            IImage.Source == null ||
    //            tbNameFilm.Text == "" ||
    //            tbReit.Text == "" ||
    //            tbZanr.Text == "" ||
    //            time.Text == ""
    //            )
    //        {
    //            MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    //            return;
    //        }
    //        else
    //        {
    //            if (item == null)
    //            {
    //                item = new FILM();
    //                entities.FILM.Add(item);
    //                lbFilm.Items.Add(item);
    //            }

    //            string fullFileName = IImage.Source.ToString();
    //            fullFileName = fullFileName.Replace(@"file:///", "");
    //            FileInfo fileInfo = new FileInfo(fullFileName);
    //            item.photo = fileInfo.Name;

    //            try
    //            {
    //item.name_film = tbNameFilm.Text;
    //                item.year_vipuska = int.Parse(tbGodVipuska.Text);
    //item.reziser = tbReziser.Text;
    //                item.zanr = tbZanr.Text;
    //                item.reiting = int.Parse(tbReit.Text);
    //item.opisaniey_film = tbOpis.Text;
    //                item.time_film = int.Parse(time.Text);

    //            }
    //            catch
    //            {
    //                MessageBox.Show("Некорректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
    //                return;
    //            }


    //        }
    //    }
    //}

}



