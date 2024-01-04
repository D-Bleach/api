using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
namespace form_api
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<NhanVien> ShowNV()
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:63734/api/NVs")
            };
            var res = httpClient.GetAsync("NVs").Result;
            if (res.IsSuccessStatusCode)
            {
                return res.Content.ReadAsAsync<List<NhanVien>>().Result;
            }
            return null;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ShowNV();
            dataGridView1.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dataGridView1.Columns["HoTen"].HeaderText = "Họ tên ";
            dataGridView1.Columns["TrinhDo"].HeaderText = "Trình Độ";
            dataGridView1.Columns["Luong"].HeaderText = "Luong";

            dataGridView1.Columns[0].Width = 150;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 150;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var select = dataGridView1.CurrentRow;
            txtMNV.Text = select.Cells["MaNV"].Value.ToString();
            txtHoTen.Text = select.Cells["HoTen"].Value.ToString();
            txtTrinhDo.Text = select.Cells["TrinhDo"].Value.ToString();
            txtLuong.Text = select.Cells["Luong"].Value.ToString();
        }
        private void btnThem_Click_1(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:63734/api/NVs")
            };
            NhanVien nv = new NhanVien
            {
                MaNV = txtMNV.Text,
                HoTen = txtHoTen.Text,
                TrinhDo = txtTrinhDo.Text,
                Luong = txtLuong.Text
            };
            var res = httpClient.PostAsJsonAsync("NVs", nv).Result;
            if (res.IsSuccessStatusCode)
            {
                dataGridView1.DataSource = ShowNV();
                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo");
            }
            else if (res.StatusCode == HttpStatusCode.Conflict)
            {
                MessageBox.Show("Mã nhân viên đã tồn tại, vui lòng nhập mã khác", "Thông báo");
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult =MessageBox.Show("Xác nhận sửa","Xác nhận",MessageBoxButtons.OKCancel);
            if (DialogResult.OK == dialogResult)
            {
                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("http://localhost:63734/api/")
                };
                NhanVien nv = new NhanVien
                {
                    MaNV = txtMNV.Text,
                    HoTen = txtHoTen.Text,
                    TrinhDo = txtTrinhDo.Text,
                    Luong = txtLuong.Text
                };
                var res = httpClient.PutAsJsonAsync("NVs", nv).Result;
                if (res.IsSuccessStatusCode)
                {
                    dataGridView1.DataSource = ShowNV();
                    MessageBox.Show("Sửa nhân viên thành công!", "Thông báo");
                }
                else if (res.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Mã nhân viên không tồn tại, vui lòng nhập mã khác", "Thông báo");
                }
            }
            
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhan viên này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("http://localhost:63734/api/NVs")
                };

                var res = httpClient.DeleteAsync($"?MaNV={txtMNV.Text}").Result;
                if (res.IsSuccessStatusCode)
                {
                    dataGridView1.DataSource = ShowNV();
                    MessageBox.Show("Xóa nhân viên thành công!", "Thông báo");
                }
                else if (res.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Mã nhân viên không tồn tại, vui lòng nhập mã khác", "Thông báo");
                }

            }
        }

        private async void txtTim_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:63734/api/NVs")
            };
            var res = httpClient.GetAsync($"?MaNV={txtMNV.Text}").Result;
            if (res.IsSuccessStatusCode)
            {
                var nv = await res.Content.ReadAsAsync<NhanVien>();
                dataGridView1.DataSource = new List<NhanVien> { nv };
            }
            else if (res.StatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show("Mã nhân viên không tồn tại, vui lòng nhập mã khác", "Thông báo");
            }
        }
    }
}
