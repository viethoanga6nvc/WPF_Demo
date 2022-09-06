using System;
using System.Collections.Generic;

namespace WPF_Demo.Models
{
    public partial class TacGium
    {
        public TacGium()
        {
            Saches = new HashSet<Sach>();
        }

        public string MaTg { get; set; } = null!;
        public string TenTacGia { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
