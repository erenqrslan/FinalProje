using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcOnlineTicariOtomasoyon.Models.Siniflar;
namespace MvcOnlineTicariOtomasoyon.Controllers
{
    [Authorize]
    public class DepartmanController : Controller
    {
        // GET: Departman
        Context c = new Context();
        public ActionResult Index()
        {
            var degerler = c.Departmans.Where(x => x.Durum == true).ToList();
            return View(degerler);
        }
        [Authorize(Roles = "A")]
        [HttpGet]
        public ActionResult DepartmanEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DepartmanEkle(Departman d)
        {
            c.Departmans.Add(d);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Departman Silme:
        public ActionResult DepartmanSil(int id)
        {
            var dep = c.Departmans.Find(id);
            dep.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        //Departman Güncelleme:
        public ActionResult DepartmanGetir(int id)
        {
            var dpt = c.Departmans.Find(id);
            return View("DepartmanGetir", dpt);
        }
        //Güncelleme İşlemei:
        public ActionResult DepartmanGuncelle(Departman p)
        {
            var dept = c.Departmans.Find(p.Departmanid);
            dept.DepartmanAd = p.DepartmanAd;
            c.SaveChanges();
            return RedirectToAction("Index");
        }

        //Departman Detay Sayfası Oluşturma
        //Departmana ait Personelleri Listeleme
        public ActionResult DepartmanDetay(int id)
        {
            var degerler = c.Personels.Where(x => x.Departmanid == id).ToList();
            var dpt = c.Departmans.Where(x => x.Departmanid == id).Select(y => y.DepartmanAd).FirstOrDefault();//FirstOrDefault kullanmamızın nedeni sadece departman adını cekmemizden kaynaklı,Tablolama şeklinde bir listeleme olsaydı Yine ToList kullanıcaktır
            //Yazılan bu sorguyu Viewe taşımak için ViewBag kullanılabilir.
            ViewBag.d = dpt;
            return View(degerler);
        }
        //
        public ActionResult DepartmanPersonelSatis(int id)
        {
            var degerler = c.SatisHarekets.Where(x => x.Personelid == id).ToList();
            var per = c.Personels.Where(x => x.Personelid == id).Select(y => y.PersonelAd + " " + y.PersonelSoyad).FirstOrDefault();
            ViewBag.dpers = per;
            return View(degerler);
        }
        //İlgili Personelin Çıkış İşlemini Yaptırma
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();//SignOut Giriş işleminden çık anlamı taşır
            Session.Abandon();// Abandon istekleri terk et anlamı taşır
            return RedirectToAction("Index", "Login");
        }
    }
}