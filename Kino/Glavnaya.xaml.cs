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
    /// Логика взаимодействия для Glavnaya.xaml
    /// </summary>
    public partial class Glavnaya : Window
    {
        Entities1 entities = new Entities1();
        public int Id_user { get; set; }
        public Glavnaya(string Id_user)
        
        {
            InitializeComponent();
        }

        private void B_Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.ShowDialog();
        }

          private void B_Film_Click(object sender, RoutedEventArgs e)
        {
            Film window = new Film(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }

        private void B_Lichn_Click(object sender, RoutedEventArgs e)
        {
            LichniyKabinet window = new LichniyKabinet(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }

        private void B_Kup_Click(object sender, RoutedEventArgs e)
        {
            PokupkaBiletov window = new PokupkaBiletov(Id_user.ToString());
            this.Close();
            window.ShowDialog();
        }
    }
}
