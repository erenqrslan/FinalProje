using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasoyon.Models.Siniflar;
namespace MvcOnlineTicariOtomasoyon.Controllers
{
    public class İstatistikController : Controller
    {
        // GET: İstatistik
        Context c = new Context();
        public ActionResult Index()
        {
            //Toplam Cari Sayısı
            var deger1 = c.Carilers.Count().ToString();
            ViewBag.d1 = deger1;
            //Toplam Ürün Sayısı
            var deger2 = c.Uruns.Count().ToString();
            ViewBag.d2 = deger2;
            //Toplam Personel Sayısı
            var deger3 = c.Personels.Count().ToString();
            ViewBag.d3 = deger3;
            //Toplam Kategori Sayısı
            var deger4 = c.Kategoris.Count().ToString();
            ViewBag.d4 = deger4;
            //Toplam Stok Sayısı
            var deger5 = c.Uruns.Sum(x => x.Stok).ToString();
            ViewBag.d5 = deger5;
            //Marka Sayısı
            var deger6 = (from x in c.Uruns select x.Marka).Distinct().Count().ToString();
            ViewBag.d6 = deger6;
            //Kritil Seviyedeki Ürün Sayısı
            var deger7 = c.Uruns.Count(x => x.Stok <= 20).ToString();
            ViewBag.d7 = deger7;
            //Max Fiyatlı Ürün
            var deger8 = (from x in c.Uruns orderby x.SatisFiyat descending select x.UrunAd).FirstOrDefault();
            ViewBag.d8 = deger8;
            //Min Fiyatlı Ürün
            var deger9 = (from x in c.Uruns orderby x.SatisFiyat ascending select x.UrunAd).FirstOrDefault();
            ViewBag.d9 = deger9;
            //Kac farklı Buzdolabı olduğu
            var deger10 = c.Uruns.Count(x => x.UrunAd == "Buzdolabı").ToString();
            ViewBag.d10 = deger10;
            //Kac farklı Laptop olduğu
            var deger11 = c.Uruns.Count(x => x.UrunAd == "Laptop").ToString();
            ViewBag.d11 = deger11;
            //Max marka
            var deger12 = c.Uruns.GroupBy(x => x.Marka).OrderByDescending(y => y.Count()).Select(z => z.Key).FirstOrDefault();
            ViewBag.d12 = deger12;
            //En cok Satan Ürün
            var deger13 = c.Uruns.Where(u => u.Urunid == c.SatisHarekets.GroupBy(x => x.Urunid).OrderByDescending(y => y.Count()).Select(z => z.Key).FirstOrDefault()).Select(k => k.UrunAd).FirstOrDefault();
            ViewBag.d13 = deger13;
            //Kasadaki tutar
            var deger14 = c.SatisHarekets.Sum(x => x.ToplamTutar).ToString();
            ViewBag.d14 = deger14;
            //Bugünki satışlar
            //Bugunun tarihini alma
            DateTime Bugun = DateTime.Today;
            var deger15 = c.SatisHarekets.Count(x => x.Tarih == Bugun);
            ViewBag.d15 = deger15;
            //Bugünki Kasa
            if (Convert.ToInt32(deger15) != 0)

            {

                var deger16 = c.SatisHarekets.Where(x => x.Tarih == Bugun).Sum(y => y.ToplamTutar).ToString();

                ViewBag.d16 = deger16;

            }

            else { ViewBag.d16 = deger15; }
            //var deger16 = c.SatisHarekets.Where(x => x.Tarih == Bugun).Sum(y => y.ToplamTutar).ToString();
            //ViewBag.d16 = deger16;
            return View();
        }
        public ActionResult KolayTablolar()
        {
            var sorgu = from x in c.Carilers
                        group x by x.CariSehir into g
                        select new SinifGrup
                        {
                            Sehir = g.Key,
                            Sayi = g.Count()
                        };
            return View(sorgu.ToList());
        }
        //Bir Viewde yeni bir tablo oluşturma ve bu Viewi Ana viewe taşımak için Partial View kullanılır
        //Partial Viewi çağırma; @Html.Action("PartialViewinAdı","Aksiyonunbulunduğu Controllerin ismi") şeklinde çağırılır
        public PartialViewResult Partial1()
        {
            var sorgu2 = from x in c.Personels
                         group x by x.Departman.DepartmanAd into g
                         select new SinifGrup2
                         {
                             Departman = g.Key,
                             Sayi = g.Count()
                         };
            return PartialView(sorgu2.ToList());
        }

        public PartialViewResult Partial2()
        {
            var sorgu = c.Carilers.ToList();
            return PartialView(sorgu);
        }
        public PartialViewResult Partial3()
        {
            var sorgu = c.Uruns.ToList();
            return PartialView(sorgu);
        }
        public PartialViewResult Partial4()
        {
            var sorgu = from x in c.Uruns
                        group x by x.Marka into g
                        select new SinifGrup3
                        {
                            Marka = g.Key,
                            Sayi = g.Count()
                        };
            return PartialView(sorgu.ToList());
        }
    }
}