using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        Ql_SinhVienEntities1 db = new Ql_SinhVienEntities1();
        SinhVien sinhvien;
        List<SinhVien> dsSinhVien;
        int idVanCD;
        int idVanDH;

        int idCoHoc;
        int idQuangHoc;
        int idDien;
        int idVlHN;

        int idPascal;
        int idCShap;
        int idSQL;

        public Form1()
        {
            InitializeComponent();
        }

        void refreshTextBox()
        {
            txtVHCD.Text = "";
            txtVHDH.Text = "";

            txtCoHoc.Text = "";
            txtQuangHoc.Text = "";
            txtDien.Text = "";
            txtVLHN.Text = "";

            txtPascal.Text = "";
            txtCshap.Text = "";
            txtSQL.Text = "";
        }

        void loadData()
        {
            lstSinhVien.Items.Clear();
            foreach (var item in db.SinhViens)
            {
                lstSinhVien.Items.Add(item.hoten);
            }
            txtName.Text = sinhvien.hoten;
            dtpNgaySinh.Value = sinhvien.ngaysinh.Value;
            ckbGioiTinh.Checked = sinhvien.gioitinh.Value;
            refreshTextBox();

            var diem = db.QuaTrinhHocTaps.Where(n => n.idSinhVien == sinhvien.idSinhVien).ToArray();
            try
            {
                var monhoc = db.MonHocs.ToArray();
                idVanCD = monhoc.First(n => n.tenmonhoc == "Văn học CĐ").idmonhoc;
                idVanDH = monhoc.First(n => n.tenmonhoc == "Văn học ĐH").idmonhoc;

                idCoHoc = monhoc.First(n => n.tenmonhoc == "Cơ học").idmonhoc;
                idQuangHoc = monhoc.First(n => n.tenmonhoc == "Quang học").idmonhoc;
                idDien = monhoc.First(n => n.tenmonhoc == "Điện").idmonhoc;
                idVlHN = monhoc.First(n => n.tenmonhoc == "VL Hạt nhân").idmonhoc;

                idPascal = monhoc.First(n => n.tenmonhoc == "Pascal").idmonhoc;
                idCShap = monhoc.First(n => n.tenmonhoc == "C#").idmonhoc;
                idSQL = monhoc.First(n => n.tenmonhoc == "SQL").idmonhoc;


                

                var listMonHocSV = db.MonHocs.Where(n => n.nhom == sinhvien.nganhhoc).ToArray();



                

                if (sinhvien.nganhhoc == 1)
                {
                    txtVHCD.Text = diem.First(n => n.idMonHoc == idVanCD).Diem.ToString();
                    txtVHDH.Text = diem.First(n => n.idMonHoc == idVanDH).Diem.ToString();

                }
                else if (sinhvien.nganhhoc == 2)
                {
                    txtCoHoc.Text = diem.First(n => n.idMonHoc == idCoHoc).Diem.ToString();
                    txtQuangHoc.Text = diem.First(n => n.idMonHoc == idQuangHoc).Diem.ToString();
                    txtDien.Text = diem.First(n => n.idMonHoc == idDien).Diem.ToString();
                    txtVLHN.Text = diem.First(n => n.idMonHoc == idVlHN).Diem.ToString();

                }
                else if (sinhvien.nganhhoc == 3)
                {
                    txtPascal.Text = diem.First(n => n.idMonHoc == idPascal).Diem.ToString();
                    txtCshap.Text = diem.First(n => n.idMonHoc == idCShap).Diem.ToString();
                    txtSQL.Text = diem.First(n => n.idMonHoc == idSQL).Diem.ToString();
                    
                }

                double dtb = 0;
                foreach (var mh in listMonHocSV)
                {
                    dtb += (double)db.QuaTrinhHocTaps.Where(n => n.idSinhVien == sinhvien.idSinhVien)
                        .First(n => n.idMonHoc == mh.idmonhoc).Diem;
                }

                dtb /= listMonHocSV.Count();
                lblDTB.Text = dtb.ToString();
            }
            catch(Exception e)
            { 
                //MessageBox.Show(e.Message);
                lblDTB.Text = "0.0";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dsSinhVien = db.SinhViens.ToList();
            var a = db.SinhViens.ToArray()[0];
            sinhvien = a;
            
            loadData(); 
        }

        private void tpCNTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as TabControl;

        }

        private void lstSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as CheckedListBox;
            try
            {
                sinhvien = db.SinhViens.ToList()[a.SelectedIndex];
                loadData();

                tpDiem.SelectedIndex = (int)sinhvien.nganhhoc - 1;
            }
            catch
            {

            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "File text(*.txt)|*.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                
            }

        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            var c = sinhvien.hoten;
            var a = db.SinhViens.Count();
            db.SinhViens.Remove(sinhvien);
            db.SaveChanges();
            var b = db.SinhViens.Count();
            var cd = sinhvien.hoten;
            loadData();
            refreshTTSV();
            MessageBox.Show("Đã Xóa thành công !!!");
        }

        void refreshTTSV()
        {
            txtName.Text = "";
            dtpNgaySinh.Text = "";
            ckbGioiTinh.Checked = false;
            lblDTB.Text = "0.0";
            refreshTextBox();
        }

        private void sVVănnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshTTSV();
            tpDiem.SelectedIndex = 0;
            sinhvien = new SinhVien();
            sinhvien.idSinhVien = db.SinhViens.Max(n=>n.idSinhVien) + 1;
            sinhvien.nganhhoc = 1;
        }

        private void sVVậtLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshTTSV();
            tpDiem.SelectedIndex = 1;
            sinhvien = new SinhVien();
            sinhvien.idSinhVien = db.SinhViens.Max(n => n.idSinhVien) + 1;
            sinhvien.nganhhoc = 2;
        }

        private void sVCNTTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshTTSV();
            tpDiem.SelectedIndex = 2;
            sinhvien = new SinhVien();
            sinhvien.idSinhVien = db.SinhViens.Max(n => n.idSinhVien) + 1;
            sinhvien.nganhhoc = 3;
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            try
            {
                if (db.SinhViens.SingleOrDefault(n => n.idSinhVien == sinhvien.idSinhVien) != null)
                {
                    var a = db.SinhViens.SingleOrDefault(n => n.idSinhVien == sinhvien.idSinhVien);
                    a.hoten = txtName.Text;
                    a.ngaysinh = dtpNgaySinh.Value;
                    a.gioitinh = ckbGioiTinh.Checked;
                    db.SaveChanges();
                    var qthtSv = db.QuaTrinhHocTaps.Where(n => n.idSinhVien == sinhvien.idSinhVien);
                    if (sinhvien.nganhhoc == 1)
                    {
                        if (txtVHCD.Text != "" && txtVHDH.Text != "")
                        {
                            try
                            {
                                var a1 = double.Parse(txtVHCD.Text);
                                var a2 = double.Parse(txtVHDH.Text);

                                if (a1 >= 0 && a1 <= 10 && a2 >= 0 && a2 <= 10)
                                {
                                    qthtSv.First(n => n.idMonHoc == idVanCD).Diem = a1;
                                    db.SaveChanges();
                                    qthtSv.First(n => n.idMonHoc == idVanDH).Diem = a2;
                                    db.SaveChanges();
                                }
                                MessageBox.Show("Cập nhật thành công !");
                            }
                            catch
                            {
                                MessageBox.Show("Vui lòng nhập đầy đủ điểm số.");
                            }
                        }

                    }
                    else if (sinhvien.nganhhoc == 2)
                    {
                        if (txtCoHoc.Text != "" && txtDien.Text != "" && txtQuangHoc.Text != "" && txtVLHN.Text != "")
                        {
                            try
                            {
                                var a1 = double.Parse(txtCoHoc.Text);
                                var a2 = double.Parse(txtQuangHoc.Text);
                                var a3 = double.Parse(txtDien.Text);
                                var a4 = double.Parse(txtVLHN.Text);

                                if (a1 >= 0 && a1 <= 10 && a2 >= 0 && a2 <= 10 && a3 >= 0 && a3 <= 10 && a4 >= 0 && a4 <= 10)
                                {
                                    qthtSv.First(n => n.idMonHoc == idCoHoc).Diem = a1;
                                    db.SaveChanges();
                                    qthtSv.First(n => n.idMonHoc == idQuangHoc).Diem = a2;
                                    db.SaveChanges();
                                    qthtSv.First(n => n.idMonHoc == idDien).Diem = a3;
                                    db.SaveChanges();
                                    qthtSv.First(n => n.idMonHoc == idVlHN).Diem = a4;
                                    db.SaveChanges();
                                }
                                MessageBox.Show("Cập nhật thành công !");
                            }
                            catch
                            {
                                MessageBox.Show("Vui lòng nhập đầy đủ điểm số.");
                            }
                        }
                    }
                    else if (sinhvien.nganhhoc == 3)
                    {
                        if (txtPascal.Text != "" && txtCshap.Text != "" && txtSQL.Text != "")
                        {
                            try
                            {
                                var a1 = double.Parse(txtPascal.Text);
                                var a2 = double.Parse(txtCshap.Text);
                                var a3 = double.Parse(txtSQL.Text);

                                if (a1 >= 0 && a1 <= 10 && a2 >= 0 && a2 <= 10 && a3 >= 0 && a3 <= 10)
                                {
                                    qthtSv.First(n => n.idMonHoc == idPascal).Diem = a1;
                                    db.SaveChanges();
                                    qthtSv.First(n => n.idMonHoc == idCShap).Diem = a2;
                                    db.SaveChanges();
                                    qthtSv.First(n => n.idMonHoc == idSQL).Diem = a3;
                                    db.SaveChanges();
                                }
                                MessageBox.Show("Cập nhật thành công !");
                            }
                            catch
                            {
                                MessageBox.Show("Vui lòng nhập đầy đủ điểm số.");
                            }
                        }
                    }
                    
                    db.SaveChanges();
                }
                else
                {
                    if(txtName.Text != "")
                    {
                        sinhvien.hoten = txtName.Text;
                        sinhvien.gioitinh = ckbGioiTinh.Checked;
                        sinhvien.ngaysinh = dtpNgaySinh.Value;
                        db.SinhViens.Add(sinhvien);

                        if (sinhvien.nganhhoc == 1)
                        {
                            if(txtVHCD.Text != "" && txtVHDH.Text != "")
                            {
                                try
                                {
                                    var a1 = double.Parse(txtVHCD.Text);
                                    var a2 = double.Parse(txtVHDH.Text);

                                    if(a1 >= 0 && a1 <= 10 && a2 >= 0 && a2 <= 10)
                                    {
                                        QuaTrinhHocTap qt = new QuaTrinhHocTap();
                                        qt.idSinhVien = sinhvien.idSinhVien;
                                        qt.idMonHoc = idVanCD;
                                        qt.Diem = a1;
                                        db.QuaTrinhHocTaps.Add(qt);
                                        db.SaveChanges();

                                        QuaTrinhHocTap qt2 = new QuaTrinhHocTap();
                                        qt2.idSinhVien = sinhvien.idSinhVien;
                                        qt2.idMonHoc = idVanDH;
                                        qt2.Diem = a2;
                                        db.QuaTrinhHocTaps.Add(qt2);
                                        db.SaveChanges();
                                    }
                                    MessageBox.Show("Đã thêm vào CSDL thành công !");
                                }
                                catch
                                {
                                    MessageBox.Show("Vui lòng nhập đầy đủ điểm số.");
                                }
                            }
                            
                        }
                        else if (sinhvien.nganhhoc == 2)
                        {
                            if (txtCoHoc.Text != "" && txtDien.Text != "" && txtQuangHoc.Text != "" && txtVLHN.Text != "")
                            {
                                try
                                {
                                    var a1 = double.Parse(txtCoHoc.Text);
                                    var a2 = double.Parse(txtQuangHoc.Text);
                                    var a3 = double.Parse(txtDien.Text);
                                    var a4 = double.Parse(txtVLHN.Text);

                                    if (a1 >= 0 && a1 <= 10 && a2 >= 0 && a2 <= 10 && a3 >= 0 && a3 <= 10 && a4 >= 0 && a4 <= 10)
                                    {
                                        QuaTrinhHocTap qt = new QuaTrinhHocTap();
                                        qt.idSinhVien = sinhvien.idSinhVien;
                                        qt.idMonHoc = idCoHoc;
                                        qt.Diem = a1;
                                        db.QuaTrinhHocTaps.Add(qt);
                                        db.SaveChanges();

                                        QuaTrinhHocTap qt2 = new QuaTrinhHocTap();
                                        qt2.idSinhVien = sinhvien.idSinhVien;
                                        qt2.idMonHoc = idQuangHoc;
                                        qt2.Diem = a2;
                                        db.QuaTrinhHocTaps.Add(qt2);
                                        db.SaveChanges();

                                        QuaTrinhHocTap qt3 = new QuaTrinhHocTap();
                                        qt3.idSinhVien = sinhvien.idSinhVien;
                                        qt3.idMonHoc = idDien;
                                        qt3.Diem = a3;
                                        db.QuaTrinhHocTaps.Add(qt3);
                                        db.SaveChanges();

                                        QuaTrinhHocTap qt4 = new QuaTrinhHocTap();
                                        qt4.idSinhVien = sinhvien.idSinhVien;
                                        qt4.idMonHoc = idVlHN;
                                        qt4.Diem = a4;
                                        db.QuaTrinhHocTaps.Add(qt4);
                                        db.SaveChanges();
                                    }
                                    MessageBox.Show("Đã thêm vào CSDL thành công !");
                                }
                                catch
                                {
                                    MessageBox.Show("Vui lòng nhập đầy đủ điểm số.");
                                }
                            }
                        }
                        else if (sinhvien.nganhhoc == 3)
                        {
                            if (txtPascal.Text != "" && txtCshap.Text != "" && txtSQL.Text != "")
                            {
                                try
                                {
                                    var a1 = double.Parse(txtPascal.Text);
                                    var a2 = double.Parse(txtCshap.Text);
                                    var a3 = double.Parse(txtSQL.Text);

                                    if (a1 >= 0 && a1 <= 10 && a2 >= 0 && a2 <= 10 && a3 >= 0 && a3 <= 10)
                                    {
                                        QuaTrinhHocTap qt = new QuaTrinhHocTap();
                                        qt.idSinhVien = sinhvien.idSinhVien;
                                        qt.idMonHoc = idPascal;
                                        qt.Diem = a1;
                                        db.QuaTrinhHocTaps.Add(qt);
                                        db.SaveChanges();

                                        QuaTrinhHocTap qt2 = new QuaTrinhHocTap();
                                        qt2.idSinhVien = sinhvien.idSinhVien;
                                        qt2.idMonHoc = idCShap;
                                        qt2.Diem = a2;
                                        db.QuaTrinhHocTaps.Add(qt2);
                                        db.SaveChanges();

                                        QuaTrinhHocTap qt3 = new QuaTrinhHocTap();
                                        qt3.idSinhVien = sinhvien.idSinhVien;
                                        qt3.idMonHoc = idSQL;
                                        qt3.Diem = a3;
                                        db.QuaTrinhHocTaps.Add(qt3);
                                        db.SaveChanges();

                                    }
                                    MessageBox.Show("Đã thêm vào CSDL thành công !");
                                }
                                catch
                                {
                                    MessageBox.Show("Vui lòng nhập đầy đủ điểm số.");
                                }
                            }
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập Họ tên !!!");
                    }  
                }
            }
            catch
            {

            }
            loadData();
        }
    }
}
