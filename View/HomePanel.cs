using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Kütüphane_Otomasyon.MyManager;

namespace Kütüphane_Otomasyon.View
{
    public partial class HomePanel : Form
    {
        MyManager manager = new MyManager();
        MyProfile profil = new MyProfile();
        #region Form_Initialize
        public HomePanel()
        {
            InitializeComponent();
        }
        #endregion
        #region Refresh_Database
        void refresh_database()
        {
            try
            {
                DataTable table = new DataTable();
                manager.database_books(LoginScreen.login_user_name).Fill(table);
                dataGridView1.DataSource = table;
                dataGridView2.DataSource = table;
                DataTable table2 = new DataTable();
                manager.database_given_book(LoginScreen.login_user_name).Fill(table2);
                dataGridView3.DataSource = table2;
                DataTable table3 = new DataTable();
                manager.readers_database().Fill(table3);
                dataGridView4.DataSource = table3;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.SuppressFinalize(table);
                GC.SuppressFinalize(table2);
                GC.SuppressFinalize(table3);
                DataGridView3_Colours();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Yaklaşan Tarihleri Renklendirme
        void DataGridView3_Colours()
        {
            for(int i=0;i<=dataGridView3.Rows.Count-1;i++)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                TimeSpan dt = DateTime.Now - Convert.ToDateTime(dataGridView3.Rows[i].Cells[3].Value.ToString());
                if(Convert.ToInt32(dt.TotalDays) >1)
                {
                    style.BackColor = Color.Red;
                }else
                {
                    style.BackColor = Color.Green;
                }
                dataGridView3.Rows[i].DefaultCellStyle = style;
            }
        }
        #endregion
        #region Form Load
        private void HomePanel_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            refresh_database();
            dataGridView1.Columns[0].HeaderText = "Kitap Adı"; //0
            dataGridView1.Columns[1].HeaderText = "Kitap Yazarı"; // 1
            dataGridView1.Columns[2].HeaderText = "Kitap Sayfası";// 2
            dataGridView1.Columns[3].HeaderText = "Kitap Adet"; // 3
            dataGridView1.Columns[4].HeaderText = "Kitap Raf Numarası"; //4
            dataGridView1.Columns[5].HeaderText = "Ekleniş Tarihi";//5
            dataGridView1.Columns[6].HeaderText = "Kitabı Ekleyen";//6
            dataGridView2.Columns[0].HeaderText = "Kitap Adı"; //0
            dataGridView2.Columns[1].HeaderText = "Kitap Yazarı"; // 1
            dataGridView2.Columns[2].HeaderText = "Kitap Sayfası";// 2
            dataGridView2.Columns[3].HeaderText = "Kitap Adet"; // 3
            dataGridView2.Columns[4].HeaderText = "Kitap Raf Numarası"; //4
            dataGridView2.Columns[5].HeaderText = "Ekleniş Tarihi";//5
            dataGridView2.Columns[6].HeaderText = "Kitabı Ekleyen";//6
            dataGridView3.Columns[0].HeaderText = "Kitabı Alan";
            dataGridView3.Columns[1].HeaderText = "Kitap Adı";
            dataGridView3.Columns[2].HeaderText = "Kitap Yazarı";
            dataGridView3.Columns[3].HeaderText = "Son Teslim Tarihi";
            dataGridView3.Columns[4].HeaderText = "Kitabı Veren";
            dataGridView4.Columns[0].HeaderText = "Adı Soyadı";
            dataGridView4.Columns[1].HeaderText = "Kullanıcı Adı";
            dataGridView4.Columns[2].HeaderText = "E-Posta";
            dataGridView4.Columns[3].HeaderText = "Telefon Numarası";
            dataGridView4.Columns[4].HeaderText = "Adres";
            dataGridView4.Columns[5].HeaderText = "Yasak Durumu";
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            tabPane1.SelectedPageIndex = 0;
            textBox15.Text = LoginScreen.login_user_name;
            textBox16.Text = profil.kullanici_sifre(LoginScreen.login_user_name);
            textBox17.Text = profil.kullanici_sec_question(LoginScreen.login_user_name);
            textBox18.Text = profil.kullanici_sec_answer(LoginScreen.login_user_name);
            textBox19.Text = profil.kullanıcı_ip_adres(LoginScreen.login_user_name);
        }
        #endregion
        #region Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            txt_tarih.Caption = DateTime.Now.ToString();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
        }
        #endregion
        #region Form_Closing
        private void HomePanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Çıkış Yapmak İstediğinize Emin Misiniz?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Form x = new LoginScreen();
                x.Show();
            }else
            {
                e.Cancel = true;
            }

        }




        #endregion
        #region Kitap Kaydet
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                errorProvider1.Clear();
                if (manager.new_add_book(textBox1.Text, textBox2.Text, textBox3.Text, Convert.ToInt32(textBox4.Text), textBox5.Text, LoginScreen.login_user_name))
                {
                    MessageBox.Show("Kitabınız Başarıyla Eklendi", "BAŞARILI", MessageBoxButtons.OK);
                    textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                    refresh_database();
                }
                else
                {
                    MessageBox.Show("Kitap Eklenemedi!\nBöyle Bir Kitap Olabilir Zaten Kayıtlı Olabilir!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (textBox11.Text != "" && textBox10.Text != "" && textBox6.Text != "")
            {
                errorProvider1.Clear();

                if (manager.give_book(textBox6.Text, textBox11.Text, textBox10.Text, dateTimePicker1.Value.ToShortDateString().ToString(),LoginScreen.login_user_name))
                {
                    MessageBox.Show("Kitap Kullanıcıya Başarıyla Teslim Edildi!", "BAŞARILI", MessageBoxButtons.OK);
                    refresh_database();
                }
                else
                {
                    MessageBox.Show("Kitap Kullanıcıya Verilemedi!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                textBox11.Text = "";
                textBox10.Text = "";
                textBox9.Text = "";
                textBox8.Text = "";
                textBox7.Text = "";
                textBox6.Text = "";
                dateTimePicker1.Value = DateTime.Now;
            }
            else
            {
                foreach (Control x in groupControl1.Controls)
                {
                    if (x.Text == "")
                    {
                        errorProvider1.SetError(x, "Eksik Veriyi Tamamlayınız!");
                    }
                }
            }
        }

        #endregion
        #region Kitap Seçimi Yapıldığında
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
         
        }
        #endregion
        #region Kitap Silme İşlemi
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(dataGridView1.SelectedRows.Count>0)
            {
                DialogResult result = MessageBox.Show(dataGridView1.CurrentRow.Cells[0].Value.ToString()+" Adlı Kitabı Silmek İstediğinize Emin Misiniz?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    if(manager.delete_book(dataGridView1.CurrentRow.Cells[0].Value.ToString(),LoginScreen.login_user_name))
                    {
                        MessageBox.Show(dataGridView1.CurrentRow.Cells[0].Value.ToString()+" Adlı Kitap Başarıyla Silindi!","BAŞARILI",MessageBoxButtons.OK);
                        textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                        refresh_database();
                    }
                }
            }else if(dataGridView3.SelectedRows.Count>0)
            {
                DialogResult result = MessageBox.Show(dataGridView3.CurrentRow.Cells[0].Value.ToString() + " Adlı Kişiden Kitabı Aldığınıza Emin Misiniz?", "DİKKAT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                   if(manager.delete_given_book(dataGridView3.CurrentRow.Cells[0].Value.ToString(), dataGridView3.CurrentRow.Cells[1].Value.ToString()))
                    {
                        MessageBox.Show("Kitap Silme İşlemi Başarıyla Yapıldı!","BAŞARILI",MessageBoxButtons.OK);refresh_database();
                    }
                }
            }
            else
            {
                MessageBox.Show("Seçili Bir Satır Bulunamadı!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }


        #endregion
        #region Temizleme İşlemi
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
        }
        #endregion
        #region Font Dialog
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fontDialog1.ShowDialog();
            this.Font = fontDialog1.Font;
        }
        #endregion
        #region Kitap Güncelleme İşlemi
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(dataGridView1.SelectedRows.Count>0)
            {
                if(textBox1.Text !=""&& textBox2.Text !=""& textBox3.Text !=""&& textBox4.Text !="" &&textBox5.Text !="")
                {
                    if(manager.update_book(textBox1.Text,textBox2.Text,textBox3.Text,Convert.ToInt32(textBox4.Text),textBox5.Text,dataGridView1.CurrentRow.Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("Güncelleme İşlemi Başarılı","BAŞARILI",MessageBoxButtons.OK);
                        textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                        refresh_database();
                    }else
                    {
                        MessageBox.Show("Güncelleme İşlemi Yapılamadı!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }else
                {
                    MessageBox.Show("Boşlukları Lütfen Doldurunuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Güncellenecek Kitabı Seçiniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Listeyi Yenile
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            refresh_database();
        }
        #endregion
        #region Ara
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(textBox1.Text !="")
            {
                DataTable x = new DataTable();
                manager.book_search(textBox1.Text,"Kitap Adı",LoginScreen.login_user_name).Fill(x);
                dataGridView1.DataSource = x;
                GC.Collect();
                GC.SuppressFinalize(x);
                GC.WaitForPendingFinalizers();
            }else if(textBox2.Text !="")
            {
                DataTable x = new DataTable();
                manager.book_search(textBox2.Text, "Kitap Yazarı", LoginScreen.login_user_name).Fill(x);
                dataGridView1.DataSource = x;
                GC.Collect();
                GC.SuppressFinalize(x);
                GC.WaitForPendingFinalizers();
            }
            else if(textBox3.Text !="")
            {
                DataTable x = new DataTable();
                manager.book_search(textBox3.Text, "Kitap Sayfası", LoginScreen.login_user_name).Fill(x);
                dataGridView1.DataSource = x;
                GC.Collect();
                GC.SuppressFinalize(x);
                GC.WaitForPendingFinalizers();
            }
            else if(textBox4.Text !="")
            {
                //DataTable x = new DataTable();
                //manager.book_search(textBox4.Text,"Kitap Adet",LoginScreen.login_user_name);
                //dataGridView1.DataSource = x;
                //GC.Collect();
                //GC.SuppressFinalize(x);
                //GC.WaitForPendingFinalizers();
                MessageBox.Show("Adete Göre Arama Yapılmamakta!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if(textBox5.Text!="")
            {
                DataTable x = new DataTable();
                manager.book_search(textBox5.Text, "Raf No", LoginScreen.login_user_name).Fill(x);
                dataGridView1.DataSource = x;
                GC.Collect();
                GC.SuppressFinalize(x);
                GC.WaitForPendingFinalizers();
            }
            else if(textBox1.Text!=""&&textBox2.Text!=""&&textBox3.Text!=""&&textBox4.Text!=""&&textBox5.Text!="")
            {
                DataTable x = new DataTable();
                manager.book_search(textBox1.Text, "Kitap Adı", LoginScreen.login_user_name).Fill(x);
                dataGridView1.DataSource = x;
                GC.Collect();
                GC.SuppressFinalize(x);
                GC.WaitForPendingFinalizers();
            }
            else
            {
                MessageBox.Show("Lütfen Aşağıdaki Kutulardan En Az Bir Tanesine Veri Girişi Yapınız!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Kısayollar
        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            if(textBox1.Text!=""&&textBox2.Text!=""&&textBox3.Text!=""&&textBox4.Text!=""&&textBox5.Text!="")
            {
                #region Kitap Ekle
                if (e.Control && e.KeyCode == Keys.S)
                {
                    if(manager.new_add_book(textBox1.Text,textBox2.Text,textBox3.Text,Convert.ToInt32(textBox4.Text),textBox5.Text,LoginScreen.login_user_name))
                    {
                        MessageBox.Show("Kitap Başarıyla Eklendi!","BAŞARILI",MessageBoxButtons.OK);
                        textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                        refresh_database();
                    }else
                    {
                        MessageBox.Show("Kitap Eklenemedi!\nBöyle Bir Kitap Zaten Kayıtlı Olabilir!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                #endregion
                #region Kitap Sil
                else if (e.Control&&e.KeyCode == Keys.X)
                {
                    DialogResult result = MessageBox.Show("Kitabı Silmek İstediğinize Emin Misiniz?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        if (manager.delete_book(textBox1.Text, LoginScreen.login_user_name))
                        {
                            MessageBox.Show("Kitap Başarıyla Silindi!", "BAŞARILI", MessageBoxButtons.OK);
                            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                            refresh_database();
                        }
                        else
                        {
                            MessageBox.Show("Kitap Silinemedi!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                #endregion
                #region Kitap Güncelle
                else if((e.Control && e.KeyCode == Keys.C)&&dataGridView1.SelectedRows.Count>0)
                {
                    if(manager.update_book(textBox1.Text,textBox2.Text,textBox3.Text,Convert.ToInt32(textBox4.Text),textBox5.Text,dataGridView1.CurrentRow.Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("Kitap Başarıyla Güncellendi!","BAŞARILI",MessageBoxButtons.OK);
                        textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                        refresh_database();
                    }
                    else
                    {
                        MessageBox.Show("Kitap Güncellenemedi!","HATA",MessageBoxButtons.OK);
                    }
                }
                #endregion
            }

        }
        #endregion
        #region Emanet Kitap 
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            tabPane1.SelectedPageIndex = 3;
        }
        #endregion
        #region İletişim
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            tabPane1.SelectedPageIndex = 2;

        }
        #endregion
        #region Profilim
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            tabPane1.SelectedPageIndex = 1;
        }
        #endregion
        #region TabPane1 SelectedPageIndexChanged Metodu
        private void tabPane1_SelectedPageIndexChanged(object sender, EventArgs e)
        {
            if(tabPane1.SelectedPageIndex == 3)
            {
                //refresh_database();
                barButtonItem3.Enabled = false;
                barButtonItem8.Enabled = false;
                barButtonItem9.Enabled = false;
                tabPane2.SelectedPageIndex = 0;
            }else
            {
                barButtonItem3.Enabled = true;
                barButtonItem8.Enabled = true;
                barButtonItem9.Enabled = true;
            }
        }
        #endregion
        #region DataGridView2 Kitap Seçimi
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox11.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox10.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox9.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }
        #endregion
        #region Temizle Button2
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            textBox11.Text = "";
            textBox10.Text ="";
            textBox9.Text = "";
            textBox8.Text = "";
            textBox7.Text = "";
        }
        #endregion
        #region Okuyucu Ekle
        private void button1_Click(object sender, EventArgs e)
        {
            bool kontrol = false;
            foreach(Control x in tabNavigationPage6.Controls)
            {
                if(maskedTextBox1.MaskFull==true)
                {
                    if (x.Text != "")
                    {
                        kontrol = true;
                    }
                    else
                    {
                        kontrol = false;
                        break;
                    }
                }else
                {
                    kontrol = false;break;
                }
            }
            if(kontrol == true)
            {
               if(manager.register_readers(textBox12.Text,textBox13.Text,textBox14.Text+comboBox1.Text,maskedTextBox1.Text,richTextBox1.Text))
                {
                    MessageBox.Show("Yeni Okuyucunuz Başarıyla Eklendi!","BAŞARILI",MessageBoxButtons.OK);
                    textBox12.Text = textBox13.Text = textBox14.Text = richTextBox1.Text = maskedTextBox1.Text = "";
                    comboBox1.SelectedIndex = -1;
                    tabPane2.SelectedPageIndex = 0;
                }
                else
                {
                    MessageBox.Show("Okuyucu Kaydedilemedi!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    textBox12.Text = textBox13.Text = textBox14.Text = richTextBox1.Text = maskedTextBox1.Text = "";
                    comboBox1.SelectedIndex = -1;
                }
            }else
            {
                MessageBox.Show("Lütfen İlgili Yerleri Doldurunuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Kullanıcı Engelle
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(dataGridView3.SelectedRows.Count>0)
            {
                DialogResult result = MessageBox.Show(dataGridView3.CurrentRow.Cells[0].Value.ToString()+" Adlı Kullanıcıyı Engellemek İstiyor Musun?","DİKKAT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    if(manager.block_user(dataGridView3.CurrentRow.Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("Kullanıcı Başarıyla Engellendi!\nArtık Kitap Alamayacak!","BAŞARILI",MessageBoxButtons.OK);
                        refresh_database();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen Verilen Kitap Listesinden Bir Kullanıcı Seçiniz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Renklendirme
        private void tabPane2_SelectedPageIndexChanged(object sender, EventArgs e)
        {
            if(tabPane2.SelectedPageIndex == 2)
            {
                refresh_database();
            }
        }
        #endregion
        #region DataGridView3 Kısayol
        private void dataGridView3_KeyUp(object sender, KeyEventArgs e)
        {
            if(dataGridView3.SelectedRows.Count>0)
            {
                if(e.Control&&e.KeyCode == Keys.X)
                {
                    DialogResult result = MessageBox.Show(dataGridView3.CurrentRow.Cells[0].Value.ToString()+" Adlı Kullanıcıyı Engellemek İstediğine Emin Misin?","EMİN MİSİN?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        if (manager.block_user(dataGridView3.CurrentRow.Cells[0].Value.ToString()))
                        {
                            if (manager.delete_given_book(dataGridView3.CurrentRow.Cells[0].Value.ToString(), dataGridView3.CurrentRow.Cells[1].Value.ToString()))
                            {
                                MessageBox.Show("Kullanıcı Başarıyla Engellendi!\nArtık Kitap Alamayacak", "BAŞARILI", MessageBoxButtons.OK);
                                refresh_database();
                            }
                        }
                    }
                }else if(e.Control && e.KeyCode == Keys.D)
                {
                    DialogResult result = MessageBox.Show(dataGridView3.CurrentRow.Cells[1].Value.ToString() + " Adlı Kitabı Aldığına Emin Misin?", "EMİN MİSİN?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {       
                            if (manager.delete_given_book(dataGridView3.CurrentRow.Cells[0].Value.ToString(), dataGridView3.CurrentRow.Cells[1].Value.ToString()))
                            {
                                MessageBox.Show("Kitap Başarıyla Alındı!\n", "BAŞARILI", MessageBoxButtons.OK);
                                refresh_database();
                            }                        
                    }
                }
            }
        }
        #endregion
        #region Kısayol Kayıt Etme
        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            bool kontrol = false;
            foreach(Control x in tabNavigationPage6.Controls)
            {
                if(maskedTextBox1.MaskFull == true)
                {
                    if(x.Text!="")
                    {
                        kontrol = true;
                    }
                    else
                    {
                        kontrol = false;
                        break;
                    }
                }
            }
            if(kontrol == true)
            {
                if(e.Control && e.KeyCode == Keys.S)
                {
                    if(manager.register_readers(textBox12.Text,textBox13.Text,textBox14.Text+comboBox1.Text,maskedTextBox1.Text,richTextBox1.Text))
                    {
                        MessageBox.Show("Kullanıcı Başarıyla Kayıt Edildi!", "BAŞARILI", MessageBoxButtons.OK);
                        textBox12.Text = textBox11.Text = textBox13.Text = richTextBox1.Text = maskedTextBox1.Text = "";
                        comboBox1.SelectedIndex = -1;
                        tabPane2.SelectedPageIndex = 0;
                    }
                }
            }
        }
        #endregion
    }
}
