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
using WPF_Demo.Models;

namespace WPF_Demo
{
    /// <summary>
    /// Interaction logic for Thongke.xaml
    /// </summary>
    public partial class Thongke : Window
    {
        public Thongke()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            QLSachContext db = new();
            var query = from dl in db.Saches
                        group dl by dl.MaTg into gr
                        select new
                        {
                            MaTg = gr.Key,
                            TongTien = gr.Sum(x => x.SoTrang * 80000)
                        };
            var query2 = from dl2 in query
                         join tg in db.TacGia on dl2.MaTg equals tg.MaTg
                         select new
                         {
                             tg.MaTg,
                             tg.TenTacGia,
                             dl2.TongTien
                         };
            dgTG.ItemsSource = query2.ToList();
        }
    }
}
