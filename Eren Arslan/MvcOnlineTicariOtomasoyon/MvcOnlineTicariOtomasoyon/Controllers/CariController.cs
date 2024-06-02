using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasoyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasoyon.Controllers
{
    public class CariController : Controller
    {
        // GET: Cari
        Context c = new Context();
        public ActionResult Index()
        {
            var degerler = c.Carilers.Where(x => x.Durum == true).ToList();
            return View(degerler);
        }
        //Cari Ekleme
        [HttpGet]
        public ActionResult YeniCari()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniCari(Cariler p)
        {
            p.Durum = true;
            c.Carilers.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Cari Silme
        public ActionResult CariSil(int id)
        {
            var cr = c.Carilers.Find(id);
            cr.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Cari Güncelleme ekranını Getirme
        public ActionResult CariGetir(int id)
        {
            var cari = c.Carilers.Find(id);
            return View("CariGetir", cari);
        }
        //Validation Controller ile Cari Güncelleme işlemi
        //Validation Controller? Bu sınıf oluştururken öreneğin [] acıp max 50 karakter olsun dediğimiz kısıtlamaya denir
        //Validation Controller Sınıf-Tablo isimşerinin içinde, Validation Message forlar ise tasarım kısmında ayarlanır
        //Controller kısmında Validation Controlleri yazdıktan sonra bu kontrollerinin sağlanıp sağlanamadığını göre bilmek
        //için Validation Message For lar aracılığıyla kullanıcıya bunu gösterebiliriz
        //@Html.ValidationMessageFor(x=>x.CariAd,"",new {@style="color:red"}) örnek kullanım: "" tırnak olan yer doğrulama
        //mesajı, new kısmıda oluşacak mesajın özniteliklerini ayarlandığı kısım, örneğin: new {@style="color:red"}) olusacak yazı kırmızı renklı olucak
        public ActionResult CariGuncelle(Cariler p)
        {
            if (!ModelState.IsValid)
            {
                return View("CariGetir");
            }
            var cari = c.Carilers.Find(p.Cariid);
            cari.CariAd = p.CariAd;
            cari.CariSoyad = p.CariSoyad;
            cari.CariSehir = p.CariSehir;
            cari.CariMail = p.CariMail;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Carinin yaptığı satışları listeleme
        public ActionResult MusteriSatis(int id)
        {
            var degerler = c.SatisHarekets.Where(x => x.Cariid == id).ToList();
            var cr = c.Carilers.Where(x => x.Cariid == id).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.cari = cr;
            return View(degerler);
        }
    }
}