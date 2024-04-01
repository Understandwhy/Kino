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

namespace Kino
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        Entities1 entities = new Entities1();
        public int Id_user { get; set; }
        public Admin(string Id_user)

        {
            InitializeComponent();
         }
        private void B_Redact_Click(object sender, RoutedEventArgs e)
        {
            RedactFilm window = new RedactFilm(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
        private void B_Lichn_Click(object sender, RoutedEventArgs e)
        {
            LichniyKabinet window = new LichniyKabinet(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
        private void B_Stori_Click(object sender, RoutedEventArgs e)
        {
            Histori window = new Histori(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
        private void B_back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.ShowDialog();
        }
        private void Film_Click(object sender, RoutedEventArgs e)
        {
            Film window = new Film(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
    }
}
