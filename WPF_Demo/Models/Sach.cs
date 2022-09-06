using System;
using System.Collections.Generic;

namespace WPF_Demo.Models
{
    public partial class Sach
    {
        public string MaSach { get; set; } = null!;
        public string TenSach { get; set; } = null!;
        public int? NamXuatBan { get; set; }
        public int? SoTrang { get; set; }
        public string? MaTg { get; set; }

        public virtual TacGium? MaTgNavigation { get; set; }
    }
}
