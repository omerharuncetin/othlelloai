using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace OthelloGameClever
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tahtaların oluşturulması ve başlangıç taşlarının konulması
            char[,] tahta = new char[8, 8];
            char[,] sanalTahta = new char[8, 8];
            char tas = 'B';
            int satir = 0, sutun = 0;
            byte[,] tasAdedi = new byte[8,8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tasAdedi[i, j] = 0;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tahta[i, j] = '-';
                    sanalTahta[i, j] = '-';

                }
            }

            BaslangicTaslari(ref tahta);
            BaslangicTaslari(ref sanalTahta);

            // sira değişkeni sıranın bilgisayarda mı yoksa insanda mı olduğuna karar vermek için kullanılır
            int sira = 0;
            string kim = "";
            while (true)
            {
                if (TahtaDoluMu(tahta))
                {
                    break;
                }

                Clear();
                TahtayiYazdir(ref tahta);
                if (tas == 'B')
                {
                    kim = "Sizin sıranız";
                }
                else
                {
                    kim = "Bilgisayar oynuyor...";
                }
                WriteLine("\n\n" + kim);
                if (sira % 2 == 0)
                {
                    KullaniciGiris(ref satir, ref sutun);
                    if (satir == -1 && sutun == -1)
                    {
                        sira++;
                        if (tas == 'B')
                        {
                            tas = 'S';
                        }
                        else
                        {
                            tas = 'B';
                        }
                        continue;
                    }
                    HamleBul(ref tahta, ref satir, ref sutun, ref tas);

                }
                else
                {
                    // Önce Bilgisayar için taş konulabilecek olası yerleri buluyor ardından en fazla taş yenebilecek yeri belirliyor ve taşı koyuyor
                    Bilgisayar(ref tahta, ref sanalTahta, ref tasAdedi, ref tas);
                    EnFazlaTas(tasAdedi, ref satir, ref sutun);
                    HamleBul(ref tahta, ref satir, ref sutun, ref tas);
                    System.Threading.Thread.Sleep(820);
                }
                if (tas == 'B')
                {
                    tas = 'S';
                }
                else
                {
                    tas = 'B';
                }

                sira++;
            }

            KazananiBul(tahta);
            ReadKey();

        }

        // Tahtayı ekrana yazdırmak için
        static void TahtayiYazdir(ref char[,] tahta)
        {
            Write("   ");
            for (int i = 0; i < 8; i++)
            {

                Write(i + "  ");
            }

            for (int i = 0; i < 8; i++)
            {

                WriteLine();
                Write(i + "  ");
                for (int j = 0; j < 8; j++)
                {
                    // Oynanan taşlara göre renklerin ayarlanması
                    if (tahta[i, j] == 'B')
                    {
                        ForegroundColor = ConsoleColor.White;

                    }

                    else if (tahta[i, j] == 'S')
                    {
                        ForegroundColor = ConsoleColor.Red;

                    }

                    else
                        ForegroundColor = ConsoleColor.Gray;

                    Write(tahta[i, j] + "  ");
                }

            }

        }

        // Kullanıcıdan satır ve sütun bilgilerini almak için
        static void KullaniciGiris(ref int satir, ref int sutun)
        {
            try
            {
                while (true)
                {
                    satir = int.Parse(ReadLine());
                    sutun = int.Parse(ReadLine());
                    if (satir == -1 && sutun == -1)
                    {
                        break;
                    }
                    if (satir < 8 && satir >= 0 && sutun < 8 && sutun >= 0)
                    {
                        break;
                    }
                    else
                    {
                        WriteLine("Lütfen 0 ile 8 arasında bir rakam giriniz.");
                        continue;
                    }
                }



            }
            catch (Exception)
            {
                WriteLine("Lütfen 0 ile 8 arasında bir tam sayı giriniz");
            }

        }

        // Kullanıcı için taşın konulabilecek yerlerinin bulunması ve taşın yerleştirilmesi
        static void HamleBul(ref char[,] tahta, ref int satir, ref int sutun, ref char tas)

        {
            bool dogruluk = true;
            byte kontrol = 1;
            while (tahta[satir, sutun] != tas)
            {
                kontrol = 1;
                if (tahta[satir, sutun] == '-')
                {
                    for (int i = 2; i < 8; i++)
                    {
                        kontrol++;
                        dogruluk = true;
                        if (satir + i < 8)
                        {
                            if (tahta[satir + i, sutun] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir + j, sutun] == tas || tahta[satir + j, sutun] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        tahta[satir + j, sutun] = tas;
                                    }

                                    tahta[satir, sutun] = tas;
                                }


                            }

                        }

                        if (satir - i >= 0)
                        {
                            dogruluk = true;
                            if (tahta[satir - i, sutun] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir - j, sutun] == tas || tahta[satir - j, sutun] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        tahta[satir - j, sutun] = tas;
                                    }
                                    tahta[satir, sutun] = tas;
                                }


                            }
                        }

                        if (sutun + i < 8)
                        {
                            dogruluk = true;
                            if (tahta[satir, sutun + i] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir, sutun + j] == tas || tahta[satir, sutun + j] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        tahta[satir, sutun + j] = tas;
                                    }
                                    tahta[satir, sutun] = tas;
                                }


                            }
                        }

                        if (sutun - i >= 0)
                        {
                            dogruluk = true;
                            if (tahta[satir, sutun - i] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir, sutun - j] == tas || tahta[satir, sutun - j] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        tahta[satir, sutun - j] = tas;
                                    }
                                    tahta[satir, sutun] = tas;
                                }


                            }
                        }

                        if (satir + i < 8 && sutun + i < 8)
                        {
                            dogruluk = true;
                            if (tahta[satir + i, sutun + i] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir + j, sutun + j] == tas || tahta[satir + j, sutun + j] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j <= i; j++)
                                    {
                                        tahta[satir + j, sutun + j] = tas;
                                    }
                                    tahta[satir, sutun] = tas;
                                }


                            }
                        }

                        if (satir + i < 8 && sutun - i >= 0)
                        {
                            dogruluk = true;
                            if (tahta[satir + i, sutun - i] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir + j, sutun - j] == tas || tahta[satir + j, sutun - j] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        tahta[satir + j, sutun - j] = tas;
                                    }
                                    tahta[satir, sutun] = tas;
                                }


                            }
                        }

                        if (satir - i >= 0 && sutun + i < 8)
                        {
                            dogruluk = true;
                            if (tahta[satir - i, sutun + i] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir - j, sutun + j] == tas || tahta[satir - j, sutun + j] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        tahta[satir - j, sutun + j] = tas;
                                    }
                                    tahta[satir, sutun] = tas;
                                }


                            }
                        }

                        if (satir - i >= 0 && sutun - i >= 0)
                        {
                            dogruluk = true;
                            if (tahta[satir - i, sutun - i] == tas)
                            {
                                for (int j = 1; j < i; j++)
                                {
                                    if (tahta[satir - j, sutun - j] == tas || tahta[satir - j, sutun - j] == '-')
                                    {
                                        dogruluk = false;
                                    }

                                }

                                if (dogruluk)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        tahta[satir - j, sutun - j] = tas;
                                    }
                                    tahta[satir, sutun] = tas;
                                }


                            }
                        }
                        if (kontrol == 7)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    WriteLine("Tekrar Deneyiniz");
                    KullaniciGiris(ref satir, ref sutun);
                }
                if (kontrol == 7 && tahta[satir,sutun] != tas)
                {
                    WriteLine("Tekrar Deneyiniz");
                    KullaniciGiris(ref satir, ref sutun);
                    kontrol = 1;
                    continue;
                }
                
            }

        }

        // Bilgisayarın hamle bulması için
        static void Bilgisayar(ref char[,] tahta, ref char[,] sanalTahta, ref byte[,] tasAdedi, ref char tas)
        {
            bool dogruluk = false;
            int kontrol = -5;

            TahtayiKopyala(ref tahta, ref sanalTahta);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tasAdedi[i, j] = 0;
                }
            }

            for (int satir = 0; satir < 8; satir++)
            {
                dogruluk = false;
                for (int sutun = 0; sutun < 8; sutun++)
                {
                    dogruluk = false;
                    if (sanalTahta[satir, sutun] == '-' && sanalTahta[satir, sutun] != tas)
                    {
                        for (int i = 2; i < 8; i++)
                        {
                            dogruluk = true;

                            if (satir + i < 8)
                            {
                                if (sanalTahta[satir + i, sutun] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir + j, sutun] == tas || sanalTahta[satir + j, sutun] == '-')
                                        {
                                            dogruluk = false;
                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j < i; j++)
                                        {
                                            sanalTahta[satir + j, sutun] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }

                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }

                            if (satir - i >= 0)
                            {
                                dogruluk = true;
                                if (sanalTahta[satir - i, sutun] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir - j, sutun] == tas || sanalTahta[satir - j, sutun] == '-')
                                        {
                                            dogruluk = false;
                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j < i; j++)
                                        {
                                            sanalTahta[satir - j, sutun] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }
                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }

                            if (sutun + i < 8)
                            {
                                dogruluk = true;
                                if (sanalTahta[satir, sutun + i] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir, sutun + j] == tas || sanalTahta[satir, sutun + j] == '-')
                                        {
                                            dogruluk = false;
                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j < i; j++)
                                        {
                                            sanalTahta[satir, sutun + j] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }
                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }

                            if (sutun - i >= 0)
                            {
                                dogruluk = true;
                                if (sanalTahta[satir, sutun - i] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir, sutun - j] == tas || sanalTahta[satir, sutun - j] == '-')
                                        {
                                            dogruluk = false;
                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j < i; j++)
                                        {
                                            sanalTahta[satir, sutun - j] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }
                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }

                            if (satir + i < 8 && sutun + i < 8)
                            {
                                dogruluk = true;
                                if (sanalTahta[satir + i, sutun + i] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir + j, sutun + j] == tas || sanalTahta[satir + j, sutun + j] == '-')
                                        {
                                            dogruluk = false;

                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j <= i; j++)
                                        {
                                            sanalTahta[satir + j, sutun + j] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }
                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }

                            if (satir + i < 8 && sutun - i >= 0)
                            {
                                dogruluk = true;
                                if (sanalTahta[satir + i, sutun - i] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir + j, sutun - j] == tas || sanalTahta[satir + j, sutun - j] == '-')
                                        {
                                            dogruluk = false;
                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j < i; j++)
                                        {
                                            sanalTahta[satir + j, sutun - j] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }
                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }

                            if (satir - i >= 0 && sutun + i < 8)
                            {
                                dogruluk = true;
                                if (sanalTahta[satir - i, sutun + i] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir - j, sutun + j] == tas || sanalTahta[satir - j, sutun + j] == '-')
                                        {
                                            dogruluk = false;
                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j < i; j++)
                                        {
                                            sanalTahta[satir - j, sutun + j] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }
                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }

                            if (satir - i >= 0 && sutun - i >= 0)
                            {
                                dogruluk = true;
                                if (sanalTahta[satir - i, sutun - i] == tas)
                                {
                                    for (int j = 1; j < i; j++)
                                    {
                                        if (sanalTahta[satir - j, sutun - j] == tas || sanalTahta[satir - j, sutun - j] == '-')
                                        {
                                            dogruluk = false;
                                        }

                                    }

                                    if (dogruluk)
                                    {
                                        for (int j = 1; j < i; j++)
                                        {
                                            sanalTahta[satir - j, sutun - j] = tas;
                                            tasAdedi[satir, sutun]++;
                                        }

                                        sanalTahta[satir, sutun] = tas;
                                        tasAdedi[satir, sutun]++;
                                        TahtayiKopyala(ref tahta, ref sanalTahta);
                                    }


                                }
                            }
                            if (sanalTahta[satir, sutun] == tas)
                            {
                                kontrol = sutun;
                                break;

                            }
                        }


                    }

                    if (sanalTahta[satir, sutun] == tas && dogruluk)
                    {
                        break;
                    }
                }

                if (kontrol != -5)
                {
                    break;
                }
            }

        }

        // Kazananın belirlenmesi için
        static void KazananiBul(char[,] tahta)
        {
            int beyaz = 0, siyah = 0;
            string sonuc;

            Clear();
            TahtayiYazdir(ref tahta);
            WriteLine("\n\n\n");

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tahta[i, j] == 'B')
                    {
                        beyaz++;
                    }
                    else
                    {
                        siyah++;
                    }
                }
            }

            if (beyaz > siyah)
            {
                sonuc = $"Beyaz taş sayısı: {beyaz} Siyah taş sayısı: {siyah}. Kazanan Beyaz!";
                WriteLine("\n" + sonuc);
            }

            else if (beyaz == siyah)
            {
                sonuc = $"Beyaz taş sayısı: {beyaz} Siyah taş sayısı: {siyah}. Berabere!";
                WriteLine("\n" + sonuc);
            }

            else
            {
                sonuc = $"Beyaz taş sayısı: {beyaz} Siyah taş sayısı: {siyah}. Kazanan Siyah!";
                WriteLine("\n" + sonuc);
            }

        }

        // Tahtanın dolu olup olmadığını kontrol edip oyunu devam ettirmek için
        static bool TahtaDoluMu(char[,] tahta)
        {
            bool kontrol = true;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tahta[i, j] == '-')
                    {
                        kontrol = false;
                    }
                }
            }

            return kontrol;
        }

        // Başlangıç taşlarını tahtalara yerleştirmek için
        static void BaslangicTaslari(ref char[,] tahta)
        {

            tahta[4, 3] = 'B';
            tahta[3, 4] = 'B';
            tahta[3, 3] = 'S';
            tahta[4, 4] = 'S';
        }

        // Bilgisayar kısmında yenebilecek en fazla taşı bulmak için
        static void EnFazlaTas(byte[,] tasAdedi,ref int satir, ref int sutun)
        {
            byte max;
            max = tasAdedi[0, 0];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tasAdedi[i,j] >= max)
                    {
                        max = tasAdedi[i, j];
                        satir = i;
                        sutun = j;
                    }
                }
            }
        }

        // Bilgisayar kısmında normal tahtayı sanal tahtaya kopyalayan bir fonksiyon
        static void TahtayiKopyala(ref char[,] tahta, ref char[,] sanalTahta)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sanalTahta[i, j] = tahta[i, j];
                }
            }
        }
    }
}

