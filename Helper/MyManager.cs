using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.OleDb;
using System.Net;
using System.Windows.Forms;
using System.Net.Mail;

namespace Kütüphane_Otomasyon
{  
    public class MyManager
    {
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=MyLocalDatabase.accdb");
        OleDbDataReader reader = null;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        #region MD5 Kripto
        public string md5_password(string pass)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] byt = Encoding.UTF8.GetBytes(pass);
            byt = md5.ComputeHash(byt);
            StringBuilder sb = new StringBuilder();
            foreach(byte md in byt)
            {
                sb.Append(md.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
        #endregion
        #region Network_Control
        public bool network_connection()
        {
            try
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("www.google.com",80);
                client.Close();
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
        #endregion
        #region Kullanıcı Giriş
        public bool Login_User(string u_name,string u_pass)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT * FROM users WHERE u_name='"+u_name+"' and u_pass='"+u_pass+"'",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }
            finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Kullanıcı Kayıt
        public bool register_user(string u_name,string u_pass,string sec_question,string sec_answer,string ip_adress)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT * FROM users WHERE u_name='"+u_name+"'",con);
                reader = cmd.ExecuteReader();
                if(!reader.Read())
                {
                    reader.Close();
                    cmd = new OleDbCommand("INSERT INTO users VALUES('" + u_name + "','" + u_pass + "','" + sec_question + "','" + sec_answer + "','" + ip_adress + "')", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return true;
                }else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Ip Adresi
        public string ip_address()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();          
        }
        #endregion
        #region Kitapları Çek
        public OleDbDataAdapter database_books(string ekleyen)
        {
            con.Open();
            cmd = new OleDbCommand("SELECT * FROM books WHERE ekleyen='"+ekleyen+"' ORDER BY eklenme_tarih DESC",con);
            adapter = new OleDbDataAdapter(cmd);
            con.Close();
            return adapter;
        }
        #endregion
        #region Kitap Ekle
        public bool new_add_book(string kitap_adi,string kitap_yazari,string kitap_sayfasi,int adet,string raf_no,string ekleyen)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT * FROM books WHERE kitap_adı='"+kitap_adi+"'",con);
                reader = cmd.ExecuteReader();
                if(!reader.Read())
                {
                    reader.Close();
                    cmd = new OleDbCommand("INSERT INTO books VALUES('" + kitap_adi + "','" + kitap_yazari + "','" + kitap_sayfasi + "','" + adet + "','" + raf_no + "','" + DateTime.Now.ToString() + "','" + ekleyen + "')", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return true;
                }else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Kitap Sil
        public bool delete_book(string kitap_adi,string ekleyen)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("DELETE * FROM books WHERE kitap_adı='"+kitap_adi+"' and ekleyen='"+ekleyen+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Kitap Güncelle
        public bool update_book(string kitap_adi,string yazar_adi,string kitap_sayfasi,int adet,string raf_no,string old_name)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("UPDATE books SET kitap_adı='"+kitap_adi+"',kitap_yazari='"+yazar_adi+"',kitap_sayfasi='"+kitap_sayfasi+"',kitap_adet='"+adet+"',raf_no='"+raf_no+"' WHERE kitap_adı='"+old_name+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }
            finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Kitap Ara
        public OleDbDataAdapter book_search(string data,string process,string ekleyen)
        {
            try
            {
                con.Open();
                if (process == "Kitap Adı")
                {
                    cmd = new OleDbCommand("SELECT * FROM books WHERE kitap_adı='" + data + "' and ekleyen='" + ekleyen + "'", con);
                    adapter = new OleDbDataAdapter(cmd);
                    return adapter;
                }
                else if (process == "Kitap Yazarı")
                {
                    cmd = new OleDbCommand("SELECT * FROM books WHERE kitap_yazari='" + data + "' and ekleyen='" + ekleyen + "'", con);
                    adapter = new OleDbDataAdapter(cmd);
                    return adapter;
                }
                else if (process == "Kitap Sayfası")
                {
                    cmd = new OleDbCommand("SELECT * FROM books WHERE kitap_sayfasi='" + data + "' and ekleyen='" + ekleyen + "'", con);
                    adapter = new OleDbDataAdapter(cmd);
                    return adapter;
                }
                //else if (process == "Kitap Adet")
                //{
                //    int adet = Convert.ToInt32(data);
                //    cmd = new OleDbCommand("SELECT * FROM books WHERE kitap_adet='" + adet + "' and ekleyen='" + ekleyen + "'", con);
                //    adapter = new OleDbDataAdapter(cmd);
                //    return adapter;
                //}
                else if (process == "Raf No")
                {
                    cmd = new OleDbCommand("SELECT * FROM books WHERE raf_no='" + data + "' and ekleyen='" + ekleyen + "'", con);
                    adapter = new OleDbDataAdapter(cmd);
                    return adapter;
                }
                else
                {
                    adapter = new OleDbDataAdapter();
                    return adapter;
                }
            }
            catch (Exception)
            {
                adapter = new OleDbDataAdapter();
                return adapter;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }           
        }
        #endregion
        #region Okuyucu Kaydet
        public bool register_readers(string name_surname,string u_name,string e_mail,string phone_number,string address)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT * FROM readers WHERE kullanıcı_adı='"+u_name+"'",con);
                reader = cmd.ExecuteReader();
                if(!reader.Read())
                {
                    reader.Close();
                    cmd = new OleDbCommand("INSERT INTO readers VALUES('" + name_surname + "','" + u_name + "','" + e_mail + "','" + phone_number + "','" + address + "','Yok')", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return true;
                }else
                {
                    reader.Close();
                    return false;
                }
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Okuyucuya Kitap Ver
       public bool give_book(string user_name,string book_name,string book_writer,string date,string given_name)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("SELECT * FROM readers WHERE kullanıcı_adı='"+user_name+"' and engel_durumu='Yok'",con);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    reader.Close();
                    cmd = new OleDbCommand("INSERT INTO readers_book VALUES('" + user_name + "','" + book_name + "','" + book_writer + "','" + date + "','"+given_name+"')", con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return true;
                } else
                {
                    reader.Close();
                    return false;
                }
                
            }catch(Exception)
            {
                return false;
            }finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region  Verilen Kitapları Çek
        public OleDbDataAdapter database_given_book(string user_name)
        {
            con.Open();
            cmd = new OleDbCommand("SELECT * FROM readers_book WHERE kitabi_veren='"+user_name+"'",con);
            adapter = new OleDbDataAdapter(cmd);
            con.Close();
            return adapter;
        }
        #endregion
        #region Kitap Alındı
        public bool delete_given_book(string user_name,string book_name)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("DELETE * FROM readers_book WHERE u_name='"+user_name+"' and kitap_adı='"+book_name+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }
            finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Kullanıcı Engelle
        public bool block_user(string u_name)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("UPDATE readers SET engel_durumu='Var' WHERE kullanıcı_adı='" + u_name + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }catch(Exception)
            {
                return false;
            }
            finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        # endregion
        #region Şifremi Değiştir
        public bool update_my_password(string u_name,string sec_answer,string new_pass)
        {
            try
            {
                con.Open();
                cmd = new OleDbCommand("UPDATE users SET u_pass='"+new_pass+"' WHERE u_name='"+u_name+"' and sec_answer='"+sec_answer+"'",con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion
        #region Okuyucu_Cek
        public OleDbDataAdapter readers_database()
        {
            con.Open();
            cmd = new OleDbCommand("SELECT * FROM readers WHERE engel_durumu='Yok'",con);
            adapter = new OleDbDataAdapter(cmd);
            con.Close();
            return adapter;
        }
        #endregion
        #region Profilim
        public class MyProfile
        {
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=MyLocalDatabase.accdb");
            OleDbDataReader reader = null;
            OleDbCommand cmd;
            OleDbDataAdapter adapter;
            #region Kullanici Sifre
            public string kullanici_sifre(string username)
            {
                string sifre = "";
                try
                {
                    con.Open();
                    cmd = new OleDbCommand("SELECT * FROM users WHERE u_name='"+username+"'",con);
                    reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        sifre = reader[1].ToString();
                    }
                    reader.Close();
                    return sifre;
                }catch(Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if(con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
                
            }
            #endregion
            #region Kullanıcı Güvenlik Sorusu
            public string kullanici_sec_question(string username)
            {
                string sec = "";
                try
                {
                    con.Open();
                    cmd = new OleDbCommand("SELECT * FROM users WHERE u_name='" + username + "'", con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        sec = reader[2].ToString();
                    }
                    reader.Close();
                    return sec;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            #endregion
            #region Kullanıcı Güvenlik Sorusu Cevabı
            public string kullanici_sec_answer(string username)
            {
                string answer = "";
                try
                {
                    con.Open();
                    cmd = new OleDbCommand("SELECT * FROM users WHERE u_name='" + username + "'", con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        answer = reader[3].ToString();
                    }
                    reader.Close();
                    return answer;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            #endregion
            #region Kullanıcı İp Adresi
            public string kullanıcı_ip_adres(string username)
            {
                string ip = "";
                try
                {
                    con.Open();
                    cmd = new OleDbCommand("SELECT * FROM users WHERE u_name='" + username + "'", con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ip = reader[4].ToString();
                    }
                    reader.Close();
                    return ip;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
