using System;
using System.Windows.Forms;

namespace Kütüphane_Otomasyon.View
{
    public partial class LoginScreen : Form
    {
        MyManager manager = new MyManager();
        #region Form_Initialize
        public static string login_user_name = "";
        short x = 0;
        public LoginScreen()
        {
            InitializeComponent();
        }
        #endregion
        #region Form_Load
        private void LoginScreen_Load(object sender, EventArgs e)
        {
            progressPanel1.Caption = "Giriş Bekleniyor";
            progressPanel1.Description = "Bekleniyor...";
            timer1.Interval = 1000;
            timer1.Start();
        }
        #endregion
        #region Tarih Timer 
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_tarih.Text = DateTime.Now.ToString();
        }
        #endregion
        #region Form Kapanma
        private void LoginScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }






        #endregion
        #region Giriş Butonu
        private void button_login_Click(object sender, EventArgs e)
        {
            if(txtusername.Text!=""&&txtpassword.Text!="")
            {
                progressPanel1.Caption = "Bağlanılıyor!";
                progressPanel1.Description = "Lütfen Bekleyiniz!";
                if (manager.Login_User(txtusername.Text,manager.md5_password(txtpassword.Text)))
                {
                    login_user_name = txtusername.Text;
                    timer2.Interval = 1000;
                    timer2.Start();
                }
                else
                {
                    progressPanel1.Caption = "Giriş Yapılamadı!";
                    progressPanel1.Description = "Bekleniyor...";
                    MessageBox.Show("Kullanıcı Adı veya Şifre Hatalı!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    txtusername.Text = txtpassword.Text = "";
                }
            }else
            {
                MessageBox.Show("Lütfen Eksik Yerleri Doldurunuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                progressPanel1.Visible = false;
            }

        }
        #endregion
        #region Giriş Paneli Şifremi Göster
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(txtpassword.UseSystemPasswordChar == true)
            {
                txtpassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtpassword.UseSystemPasswordChar = true;
            }
        }



        #endregion
        #region Destek Bölümü İnternet Bağlantı Kontrol
        private void tabPane1_SelectedPageIndexChanged(object sender, EventArgs e)
        {
            if(tabPane1.SelectedPage.Name =="tabNavigationPage4")
            {
                if(!manager.network_connection())
                {
                    MessageBox.Show("Destek Bölümü İçin Aktif Bir İnternet Bağlantısı Gerekmektedir!","BAĞLANTI HATASI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    tabPane1.SelectedPageIndex = 0;
                }
            }if(tabPane1.SelectedPageIndex == 0)
            {
                txtusername.Focus();
            }if(tabPane1.SelectedPageIndex ==1)
            {
                textBox1.Focus();
            }if(tabPane1.SelectedPageIndex ==2)
            {
                textBox4.Focus();
            }
        }
        #endregion
        #region Kayıt Ol Button
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!=""&&textBox2.Text!=""&&textBox3.Text!=""&&comboBox1.Text!="")
            {
                if(manager.register_user(textBox1.Text,manager.md5_password(textBox2.Text),comboBox1.Text,textBox3.Text,manager.ip_address()))
                {
                    MessageBox.Show(textBox1.Text + " Adlı Kullanıcı Başarıyla Kayıt Edildi!", "HATA", MessageBoxButtons.OK);
                    textBox1.Text = textBox2.Text = textBox3.Text = ""; comboBox1.SelectedIndex = -1;
                    tabPane1.SelectedPageIndex = 0;
                }else
                {
                    MessageBox.Show("Kullanıcı Kaydı Yapılamadı!\nBöyle Bir Kullanıcı Olabilir!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("Lütfen Eksiklikleri Doldurunuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Kayıt Ol Paneli Şifremi Göster
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(textBox2.UseSystemPasswordChar == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
        #endregion
        #region ProgressBar Timer
        private void timer2_Tick(object sender, EventArgs e)
        {
            x++;
            if(x==3)
            {
                timer2.Stop();              
                Form x = new HomePanel();
                this.Hide();
                x.Show();
            }
        }
        #endregion
        #region Şifremi Unuttum
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if((textBox5.Text!=""&&textBox4.Text!="")&&(textBox6.Text==textBox7.Text))
            {
                if(manager.update_my_password(textBox4.Text,textBox5.Text,manager.md5_password(textBox6.Text)))
                {
                    MessageBox.Show("Şifreniz Başarıyla Güncellendi!","BAŞARILI",MessageBoxButtons.OK);
                    textBox5.Text = textBox4.Text = textBox6.Text = textBox7.Text = "";
                    tabPane1.SelectedPageIndex = 0;
                }
                else
                {
                    MessageBox.Show("Şifreniz Güncellenemedi!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("İlgili Yerleri Lütfen Doldurunuz!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Şifremi Göster
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(textBox6.UseSystemPasswordChar == true&&textBox7.UseSystemPasswordChar == true)
            {
                textBox6.UseSystemPasswordChar = textBox7.UseSystemPasswordChar = false;
            }
            else
            {
                textBox6.UseSystemPasswordChar = textBox7.UseSystemPasswordChar = true;

            }
        }
        #endregion
    }
}
