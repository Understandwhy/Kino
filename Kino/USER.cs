//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kino
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER
    {
        public USER()
        {
            this.POKUPKA_BILETA = new HashSet<POKUPKA_BILETA>();
            this.USER_HISTIRY = new HashSet<USER_HISTIRY>();
        }
    
        public int Id_user { get; set; }
        public string name_user { get; set; }
        public string surname_user { get; set; }
        public string login { get; set; }
        public string phone { get; set; }
        public Nullable<int> Id_bilet { get; set; }
        public Nullable<System.DateTime> data_bith { get; set; }
        public string role { get; set; }
        public string parol { get; set; }
    
        public virtual ICollection<POKUPKA_BILETA> POKUPKA_BILETA { get; set; }
        public virtual ICollection<USER_HISTIRY> USER_HISTIRY { get; set; }
    }
}