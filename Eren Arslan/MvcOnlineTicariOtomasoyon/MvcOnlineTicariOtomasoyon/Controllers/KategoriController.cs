using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasoyon.Models.Siniflar;
using PagedList;
using PagedList.Mvc;

namespace MvcOnlineTicariOtomasoyon.Controllers
{
    public class KategoriController : Controller
    {
        // GET: Kategori

        //Listeleme işlemi
        Context c = new Context();
        public ActionResult Index(int sayfa = 1)
        {
            var degerler = c.Kategoris.ToList().ToPagedList(sayfa, 4);
            return View(degerler);
        }
        //Kategori Ekleme işlemi
        [HttpGet] //view çalıştığında bu metodu çalıştır demek
        public ActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost] //Bir buttona tıklandığı zaman bu kısım çalşır
        public ActionResult KategoriEkle(Kategori k)
        {
            c.Kategoris.Add(k);//k nesnesi view tarafında gonderilecek paremetreleri tutucak
            c.SaveChanges();//tutulan değişiklikleri kaydetmemize yarar (veri tabanı kısmında)
            return RedirectToAction("Index");// bu işlemler tamladıktanda sonra beni bir yere yöneldirmeye yarıyor
        }

        //Kategori Silme işlemi
        public ActionResult KetegoriSil(int id)
        {
            var ktg = c.Kategoris.Find(id);//kategoris sınıfın içinden dışarıdan gonderılen id yi bul anlamına gelir
            c.Kategoris.Remove(ktg);//Remove kaldır anlamı tasır
            c.SaveChanges();
            return RedirectToAction("Index");
        }

        //Kategori Güncellem İşlemi
        public ActionResult KategoriGetir(int id)
        {
            var kategori = c.Kategoris.Find(id);
            return View("KategoriGetir", kategori);
        }
        public ActionResult KategoriGuncelle(Kategori k)
        {
            var ktgr = c.Kategoris.Find(k.KategoriİD);
            ktgr.KategoriAd = k.KategoriAd;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}