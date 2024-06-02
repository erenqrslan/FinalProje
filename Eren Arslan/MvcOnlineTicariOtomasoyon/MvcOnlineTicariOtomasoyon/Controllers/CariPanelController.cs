using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcOnlineTicariOtomasoyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasoyon.Controllers
{
    public class CariPanelController : Controller
    {
        // GET: CariPanel
        Context c = new Context();
        [Authorize]
        public ActionResult Index()
        {
            /*Cari Maili Session olarak almamızın nedeni: giriş yapan kullanıcının bilgisini tutmak
            ve o bilgi üzerinden ilgili kullanıcının diğer verilerine ulaşmak*/
            var mail = (string)Session["CariMail"];
            var degerler = c.mesajlars.Where(x => x.Alici == mail).ToList();
            ViewBag.m = mail;
            var mailid = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.Cariid).FirstOrDefault();
            ViewBag.mid = mailid;
            var toplamsatis = c.SatisHarekets.Where(x => x.Cariid == mailid).Count();
            ViewBag.toplamsatis = toplamsatis;
            //Carinin bu güne kadarki harcamalarını hesaplama
            //var toplamtutar = c.SatisHarekets.Where(x => x.Cariid == mailid).Sum(y => y.ToplamTutar);
            var toplamtutar = c.SatisHarekets
                   .Where(x => x.Cariid == mailid)
                   .Sum(y => (decimal?)y.ToplamTutar) ?? 0;
            ViewBag.toplamtutar = toplamtutar;
            //Carinin bu güne kadarki aldığı toplam ürün adedi
            var toplamurunsayisi = c.SatisHarekets.Where(x => x.Cariid == mailid).Sum(y => (decimal?)y.Adet) ?? 0;
            ViewBag.toplamurunsayisi = toplamurunsayisi;
            //ViewBag ile View tarafına ad-soyad taşıma
            var adsoyad = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.adsoyad = adsoyad;
            var sehir = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.CariSehir).FirstOrDefault();
            ViewBag.sehir = sehir;
            return View(degerler);
        }
        [Authorize]
        public ActionResult Siparislerim()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(x => x.CariMail == mail.ToString()).Select(y => y.Cariid).FirstOrDefault();
            var degerler = c.SatisHarekets.Where(x => x.Cariid == id).ToList();
            return View(degerler);
        }
        [Authorize]
        public ActionResult GelenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.mesajlars.Where(x => x.Alici == mail).OrderByDescending(x => x.MesajID).ToList();
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Gönderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult GidenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.mesajlars.Where(x => x.Gönderici == mail).OrderByDescending(x => x.MesajID).ToList();
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Gönderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult MesajDetay(int id)
        {
            var degerler = c.mesajlars.Where(x => x.MesajID == id).ToList();
            var mail = (string)Session["CariMail"];
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Gönderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(degerler);
        }
        [Authorize]
        [HttpGet]
        public ActionResult YeniMesaj()
        {
            //ViewBagler ile değerleri taşıma
            var mail = (string)Session["CariMail"];
            var gelensayisi = c.mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Gönderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View();
        }
        [HttpPost]
        public ActionResult YeniMesaj(mesajlar m)
        {
            var mail = (string)Session["CariMail"];
            m.Gönderici = mail;
            //Yeni mesaj gönderirken;tarihi Controllerden otomatik şuanki zamanı alma          
            m.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            //Yeni Mesaj Ekleme işlemi
            c.mesajlars.Add(m);
            c.SaveChanges();
            return View();
        }
        [Authorize]
        public ActionResult KargoTakip(string p)
        {
            var k = from x in c.KargoDetays select x;
            k = k.Where(y => y.TakipKodu.Contains(p));//Textboxun içindeki varsa,texboxun içindeki değere göre sıralanır
            return View(k.ToList());
        }
        [Authorize]
        public ActionResult CariKargoTakip(string id)
        {
            var degerler = c.KargoTakips.Where(x => x.TakipKodu == id).ToList();
            return View(degerler);
        }
        //İlgili Çarinin Çıkış İşlemini Yaptırma
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();//SignOut Giriş işleminden çık anlamı taşır
            Session.Abandon();// Abandon istekleri terk et anlamı taşır
            return RedirectToAction("Index", "Login");
        }
        //Profil bilgisini Partiala Taşımak
        public PartialViewResult Partial1()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.Cariid).FirstOrDefault();
            var caribul = c.Carilers.Find(id);
            return PartialView("Partial1", caribul);
        }
        public PartialViewResult Partial2()
        {
            //Son atılan mesajı en üste göstermek
            var veriler = c.mesajlars.Where(x => x.Gönderici == "admin").OrderByDescending(x => x.Tarih).ToList();
            ////İlk mesajı en üste gösteriyor
            //var veriler = c.mesajlars.Where(x => x.Gönderici == "admin").ToList();
            return PartialView(veriler);
        }
        public ActionResult CariBilgiGuncelle(Cariler cr)
        {
            var cari = c.Carilers.Find(cr.Cariid);
            cari.CariAd = cr.CariAd;
            cari.CariSoyad = cr.CariSoyad;
            cari.CariSifre = cr.CariSifre;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}