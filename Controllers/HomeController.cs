using MvcYemek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcYemek.Controllers
{
    public class HomeController : Controller
    {
        MvcContext db = new MvcContext();
        // GET: Home
        public ActionResult Index(string p)
        {
            var malzemeAra = from ma in db.Tarif select ma;
            if (!string.IsNullOrEmpty(p))
            {
                malzemeAra = malzemeAra.Where(x => x.Malzemeler.Contains(p));
            }



            return View(malzemeAra.ToList());

           // var tarifListesi = db.Tarif.OrderByDescending(x =>x.YayınTarihi).ToList(); 
          // return View(tarifListesi);
        }

        public ActionResult Arama(string p)
        {
            var malzemeAra = from ma in db.Tarif select ma;
            if (!string.IsNullOrEmpty(p))
            {
                malzemeAra = malzemeAra.Where(x => x.Malzemeler.Contains(p));
               
            }
            return View(malzemeAra.ToList());
        }

      


        public ActionResult KategoriListele()
        {
          
            var KategoriListesi = db.Kategori.OrderBy(x => x.KategoriAd).ToList();
            return View(KategoriListesi);
        }

        public ActionResult KategoriTarifListele(int id)
        {

           
            return View(id);
        }
        public ActionResult TarifListele(int id)
        {

            var data = db.Tarif.Where(x => x.KategoriID == id).ToList();
            return View("Index",data);
        }


        public ActionResult EtiketListele(string p)
        {

            var etiketListele = from et in db.Etiket select et;
            if (!string.IsNullOrEmpty(p))
            {
                etiketListele = etiketListele.Where(x => x.Ad.Contains(p));
            }
            return View(etiketListele.ToList());

            //var EtiketListesi = db.Etiket.OrderBy(x => x.Ad).ToList();
           //return View(EtiketListesi);
        }

        public ActionResult EtiketTarifListele(int id)
        {


            return View(id);
        }
        public ActionResult EtktTarifListele(int id)
        {

            var etktdata = db.Tarif.Where(x => x.EtiketID == id).ToList();
            return View("Index", etktdata);
        }



        public ActionResult Iletisim()
        {

            return View();
        }

        public ActionResult CikisYap()
        {
            Session["Kullanici"] = null;

            return RedirectToAction("Index");

        }

    }


}