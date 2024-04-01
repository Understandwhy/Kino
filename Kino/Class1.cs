using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Polzovatel
    {
        public static string role { get; set; }
    }
    partial class USER
    {
        public override string ToString()
        {
            return Id_user + "/" + name_user + " " + surname_user + "/" +role;
        }
    }
    partial class FILM
    {
        public override string ToString()
        {
            return "Название:" + name_film + "\n"+ " Описание :" + opisaniey_film;
        }
    }
    partial class USER_HISTIRY
    {
        public override string ToString()
        {
            return Id_histiry + "." + time_VHOD + ":"+ time_VIHOD;
        }
    }
    partial class ZAL
    {
        public override string ToString()
        {
            return tip_zal +"/"+ kolvo_mest + "мест";
        }
    }
    partial class POKUPKA_BILETA
    {
        public override string ToString()
        {
            return "Номер билета :" + Id_bilet + "/"+ "Цена билета :" + price + "/"+ "Зал :" + Id_zal + "/" + "Место :" + mesto+ "/" + "Фильм :" + Id_film;
        }
    }
}
