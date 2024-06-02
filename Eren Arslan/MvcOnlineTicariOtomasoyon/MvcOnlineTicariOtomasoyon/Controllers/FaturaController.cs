using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasoyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasoyon.Controllers
{
    public class FaturaController : Controller
    {
        // GET: Fatura
        Context c = new Context();
        //View kısmına faturaları taşıma
        public ActionResult Index()
        {
            var liste = c.Faturalars.ToList();
            return View(liste);
        }
        //Fatura ekle
        [HttpGet]
        public ActionResult FaturaEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FaturaEkle(Faturalar f)
        {
            c.Faturalars.Add(f);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Fatura Günceleme için Güncelleme Sayfasına İlgili faturayı getirmek
        public ActionResult FaturaGetir(int id)
        {
            var fatura = c.Faturalars.Find(id);
            return View("FaturaGetir", fatura);
        }
        //Fatura Güncelleme
        public ActionResult FaturaGuncelle(Faturalar f)
        {
            var fatura = c.Faturalars.Find(f.Faturaid);
            fatura.FaturaSerino = f.FaturaSerino;
            fatura.FaturaSiraNo = f.FaturaSiraNo;
            fatura.Saat = f.Saat;
            fatura.Tarih = f.Tarih;
            fatura.TeslimAlan = f.TeslimAlan;
            fatura.TeslimEden = f.TeslimEden;
            fatura.VergiDairesi = f.VergiDairesi;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Faturaların detayları
        public ActionResult FaturaDetay(int id)
        {
            var degerler = c.FaturaKalems.Where(x => x.Faturaid == id).ToList();
            var dpt = c.Faturalars.Where(x => x.Faturaid == id).Select(y => y.FaturaSerino).FirstOrDefault();//FirstOrDefault kullanmamızın nedeni sadece departman adını cekmemizden kaynaklı,Tablolama şeklinde bir listeleme olsaydı Yine ToList kullanıcaktır
            //Yazılan bu sorguyu Viewe taşımak için ViewBag kullanılabilir.
            ViewBag.d = dpt;
            return View(degerler);
        }
        //Faturaya ait kalem girişi
        [HttpGet]
        public ActionResult YeniKalem()
        {
            return View();
        }
        public ActionResult YeniKalem(FaturaKalem p)
        {
            c.FaturaKalems.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}