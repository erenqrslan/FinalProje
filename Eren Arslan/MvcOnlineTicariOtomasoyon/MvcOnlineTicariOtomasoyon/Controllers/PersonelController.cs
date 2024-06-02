using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasoyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasoyon.Controllers
{
    public class PersonelController : Controller
    {
        // GET: Personel
        Context c = new Context();
        public ActionResult Index()
        {
            //var degerler = c.Personels.ToList();
            var degerler = c.Personels.Where(x => x.Durum == true).ToList();
            return View(degerler);
        }
        //Personel Ekleme 
        [HttpGet]
        public ActionResult PersonelEkle()
        {
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd,
                                               Value = x.Departmanid.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View();
        }
        [HttpPost]
        public ActionResult PersonelEkle(Personel p)
        {
            //File upload ile resim yüklemek
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);//dosyanın adını alma
                string uzanti = Path.GetExtension(Request.Files[0].FileName);//dosyanın uzantısını alma
                string yol = "~/Image/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.PersonelGorsel = "/Image/" + dosyaadi + uzanti;//Veri tabanına personel görseli kaydetmek için
            }
            p.Durum = true;
            c.Personels.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Personel Güncelleme işlemi
        //Güncellemeden önce personeli getirmemiz gerekiyor
        public ActionResult PersonelGetir(int id)
        {
            //DropdownListForun doldurulması için
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd,
                                               Value = x.Departmanid.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            var prs = c.Personels.Find(id);
            return View("PersonelGetir", prs);
        }
        //Personel Güncelle ActionResult
        public ActionResult PersonelGuncelle(Personel p)
        {
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);//dosyanın adını alma
                string uzanti = Path.GetExtension(Request.Files[0].FileName);//dosyanın uzantısını alma
                string yol = "~/Image/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.PersonelGorsel = "/Image/" + dosyaadi + uzanti;//Veri tabanına personel görseli kaydetmek için
            }
            var prsn = c.Personels.Find(p.Personelid);
            prsn.PersonelAd = p.PersonelAd;
            prsn.PersonelSoyad = p.PersonelSoyad;
            prsn.PersonelGorsel = p.PersonelGorsel;
            prsn.Departmanid = p.Departmanid;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Personel Silme - (Bu kısımdada ilişkilir bir tablo olduğu için tamamen silmek yerine durumunu False yaptım)
        public ActionResult PersonelSil(int id)
        {
            var per = c.Personels.Find(id);
            per.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelListe()
        {
            var sorgu = c.Personels.ToList();
            return View(sorgu);
        }

    }
}