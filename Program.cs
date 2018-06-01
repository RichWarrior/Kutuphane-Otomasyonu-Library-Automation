using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using Kütüphane_Otomasyon.View;
using System;
using System.Windows.Forms;

namespace Kütüphane_Otomasyon
{
    static class Program
    {
        /*Kısayollar
         * CTRL+S = KİTAP EKLE
         * CTRL+X = KİTAP SİL && KULLANICI ENGELLE
         * CTRL+C = KİTAP GÜNCELLE
         * CTRL+D = KİTAP ALINDI
         * */
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
            Application.Run(new LoginScreen());
        }
    }
}
