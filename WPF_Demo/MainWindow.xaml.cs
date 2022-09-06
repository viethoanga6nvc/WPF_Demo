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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Demo.Models;
using System.Text.RegularExpressions;
using System.Reflection;

namespace WPF_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QLSachContext db = new QLSachContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienthiDL();
            HienthiCB();
        }

        private void HienthiCB()
        {
            var query = from tg in db.TacGia
                        select tg;
            cbTG.ItemsSource = query.ToList();
            cbTG.DisplayMemberPath = "TenTacGia";
            cbTG.SelectedValuePath = "MaTg";
            cbTG.SelectedIndex = 0;
        }

        private List<TT> LayDL()
        {
            var query = from dl in db.Saches
                        select new TT
                        {
                            MaSach = dl.MaSach,
                            TenSach = dl.TenSach,
                            MaTg = dl.MaTg,
                            NamXuatBan = dl.NamXuatBan,
                            SoTrang = dl.SoTrang,
                            TongTien = dl.SoTrang * 80000
                        };
            return query.ToList<TT>();
        }

        private void HienthiDL()
        {
            dgSach.ItemsSource = LayDL();
        }

        private void dgSach_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dgSach.SelectedItem != null)
            {
                try
                {
                    Type t = dgSach.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtMa.Text = p[0].GetValue(dgSach.SelectedValue).ToString();
                    txtName.Text = p[1].GetValue(dgSach.SelectedValue).ToString();
                    txtNamXB.Text = p[3].GetValue(dgSach.SelectedValue).ToString();
                    txtTrang.Text = p[4].GetValue(dgSach.SelectedValue).ToString();
                    cbTG.SelectedValue = p[2].GetValue(dgSach.SelectedValue).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi chọn hàng "+ex.Message,"Thông báo",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CheckDL()
        {
            string mess = "";
            if (txtMa.Text == "" || txtName.Text == "" || txtNamXB.Text == "" || txtTrang.Text == "")
            {
                mess += "\nCan nhap day du du lieu";
            }
            if (!Regex.IsMatch(txtTrang.Text, @"\d+"))
            {
                mess += "\nSo trang phai la so nguyen";
            }
            else
            {
                int sotrang=int.Parse(txtTrang.Text);
                if (sotrang < 0)
                {
                    mess += "\nSo trang phai la so duong";
                }
            }
            if (mess != "")
            {
                MessageBox.Show(mess, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckDL())
                {
                    var query = db.Saches.SingleOrDefault(t=>t.MaSach.Equals(txtMa.Text));
                    if (query != null)
                    {
                        MessageBox.Show("Da ton tai", "Thong bao", MessageBoxButton.OK);
                        HienthiDL();
                    }
                    else
                    {
                        Sach sach = new();
                        sach.MaSach = txtMa.Text;
                        sach.TenSach = txtName.Text;
                        sach.SoTrang = int.Parse(txtTrang.Text);
                        sach.NamXuatBan = int.Parse(txtName.Text);
                        TacGium tacGia = (TacGium)cbTG.SelectedItem;
                        sach.MaTg = tacGia.MaTg;
                        db.Saches.Add(sach);
                        db.SaveChanges();
                        MessageBox.Show("Them thanh cong", "Thong bao", MessageBoxButton.OK);
                        HienthiDL();
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Co loi khi them: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sua = db.Saches.SingleOrDefault(sp=>sp.MaSach==txtMa.Text);
                if (sua != null)
                {
                    if (CheckDL())
                    {
                        sua.TenSach = txtName.Text;
                        sua.SoTrang = int.Parse(txtTrang.Text);
                        sua.NamXuatBan = int.Parse(txtName.Text);
                        sua.MaTg = ((TacGium)cbTG.SelectedItem).MaTg;
                        db.SaveChanges();
                        HienthiDL();
                    }
                }
                else
                {
                    MessageBox.Show("Khong tim thay san pham can sua");
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Co loi khi sua: " + ex1.Message);
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = db.Saches.SingleOrDefault(t => t.MaSach.Equals(txtMa.Text));
                if (query != null)
                {
                    MessageBoxResult rs = MessageBox.Show("Ban co chac muon xoa?", "Thong bao", MessageBoxButton.YesNoCancel);
                    if(rs == MessageBoxResult.Yes)
                    {
                        db.Saches.Remove(query);
                        db.SaveChanges();
                        HienthiDL();
                    }
                }
                else
                {
                    MessageBox.Show("Khong ton tai du lieu de xoa", "Thong bao");
                    HienthiDL();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Co loi khi xoa: " + e1.Message, "Thong bao", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnTK_Click(object sender, RoutedEventArgs e)
        {
            Thongke thongke = new Thongke();
            thongke.ShowDialog();
        }
    }
}
