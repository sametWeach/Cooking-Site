using MvcYemek.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcYemek.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        MvcContext db = new MvcContext();

        // GET: Yonetim
        [HttpGet]
        public ActionResult Index()
        {

            ViewBag.profil = Session["Kullanici"];
            if (ViewBag.profil == null)
            {
                //kullanıcı bulunamadı 
                return RedirectToAction("Index", "GirisYap");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(Kullanici kull, HttpPostedFileBase Resimg)
        {

            //int id= ViewBag.KullaniciID;

            //kull.KullaniciID = id;
            Kullanici profilSahibi = Session["Kullanici"] as Kullanici;
            kull.KullaniciID = profilSahibi.KullaniciID;
            Kullanici kulgun = db.Kullanici.Find(kull.KullaniciID);

            if (Resimg != null)
            {
                kulgun.ResimID = ResimKaydet(Resimg, HttpContext);
            }
            else
            {
                //resim yüklemeden devam edecek.
            }
            kulgun.Ad = kull.Ad;
            kulgun.Soyad = kull.Soyad;
            kulgun.Eposta = kull.Eposta;
            kulgun.Sifre = kull.Sifre;
            kulgun.KullaniciAdi = kull.KullaniciAdi;

            Session["Kullanici"] = kulgun;
            ViewBag.profil = Session["Kullanici"];


            db.SaveChanges();


            return RedirectToAction("Index", "Kullanici");
        }

        public static int ResimKaydet(HttpPostedFileBase Resim, HttpContextBase ctx)
        {
            MvcContext db = new MvcContext();


            Kullanici k = (Kullanici)ctx.Session["Kullanici"];

            int kucukWidth = Convert.ToInt32(ConfigurationManager.AppSettings["kw"]);
            int kucukHeight = Convert.ToInt32(ConfigurationManager.AppSettings["kh"]);
            int ortaWidth = Convert.ToInt32(ConfigurationManager.AppSettings["ow"]);
            int ortaHeight = Convert.ToInt32(ConfigurationManager.AppSettings["oh"]);
            int buyukWidth = Convert.ToInt32(ConfigurationManager.AppSettings["bw"]);
            int buyukHeight = Convert.ToInt32(ConfigurationManager.AppSettings["bh"]);

            string newName = Path.GetFileNameWithoutExtension(Resim.FileName) + "-" + Guid.NewGuid() + Path.GetExtension(Resim.FileName);

            Image orjRes = Image.FromStream(Resim.InputStream);
            Bitmap kucukRes = new Bitmap(orjRes, kucukWidth, kucukHeight);
            Bitmap ortaRes = new Bitmap(orjRes, ortaWidth, ortaHeight);
            Bitmap buyukRes = new Bitmap(orjRes, buyukWidth, buyukHeight);

            kucukRes.Save(ctx.Server.MapPath("~/Content/Resimler/Kucuk/" + newName));
            ortaRes.Save(ctx.Server.MapPath("~/Content/Resimler/Orta/" + newName));
            buyukRes.Save(ctx.Server.MapPath("~/Content/Resimler/Buyuk/" + newName));


            Resim dbRes = new Resim();
            dbRes.Ad = Resim.FileName;
            dbRes.Buyukresimyol = "/Content/Resimler/Buyuk/" + newName;
            dbRes.Ortaresimyol = "/Content/Resimler/Orta/" + newName;
            dbRes.Kucukresimyol = "/Content/Resimler/Kucuk/" + newName;

            dbRes.EklenmeTarihi = DateTime.Now;
            dbRes.KullanıcıID = k.KullaniciID;

            db.Resim.Add(dbRes);
            db.SaveChanges();

            return dbRes.ResimID;

        }


        public ActionResult Tarifler()
        {
            Kullanici profilSahibi = Session["Kullanici"] as Kullanici;


            var tarifler = db.Tarif.Where(x => x.KullaniciID == profilSahibi.KullaniciID).ToList();
            return View(tarifler);
        }
        [HttpGet]
        public ActionResult TarifYaz()
        {

            ViewBag.Kategori = db.Kategori.ToList();
            ViewBag.Etiket = db.Etiket.ToList();
            return View();



        }

        [HttpPost]
        public ActionResult TarifYaz(Tarif tarif, HttpPostedFileBase Resim)
        {


            if (tarif != null)
            {


                Kullanici aktif = Session["Kullanici"] as Kullanici;
                tarif.YayınTarihi = DateTime.Now;
                tarif.KullaniciID = aktif.KullaniciID;
                tarif.KapakResimID = ResimKaydet(Resim, HttpContext);
                db.Tarif.Add(tarif);
                db.SaveChanges();



            }


            db.SaveChanges();



            return RedirectToAction("Tarifler");
        }


        public ActionResult TarifSil(int id)
        {
            Tarif tf = db.Tarif.Where(x => x.TarifID == id).SingleOrDefault();
            if (tf == null)
            {
                //kategori yok
            }
            else
            {
                db.Tarif.Remove(tf);
                db.SaveChanges();
            }

            return RedirectToAction("Tarifler");
        }


        [HttpGet]
        public ActionResult TarifGuncelle(int id)
        {

            ViewBag.Kategori = db.Kategori.ToList();
            ViewBag.Etiket = db.Etiket.ToList();

            Tarif tr = db.Tarif.Find(id);
            if (tr == null)
            {
                //kullanıcı bulunamadı 
                return RedirectToAction("Tarifler");
            }

            TempData["TarifID"] = id;
            return View(tr);
        }
        [HttpPost]
        public ActionResult TarifGuncelle(Tarif trf, HttpPostedFileBase Resim)
        {
            int TarifID = (int)TempData["TarifID"];


            Tarif tf = db.Tarif.Find(TarifID);
            Kullanici aktif = Session["Kullanici"] as Kullanici;

            tf.KullaniciID = aktif.KullaniciID;
            if (Resim != null)
            {
                tf.KapakResimID = ResimKaydet(Resim, HttpContext);
            }
            else
            {
                //resim yüklemeden devam edecek.
            }



            tf.Baslik = trf.Baslik;
            tf.Aciklama = trf.Aciklama;
            tf.KategoriID = trf.KategoriID;
            tf.EtiketID = trf.EtiketID;
            tf.Malzemeler = trf.Malzemeler;
            tf.Yapilis = trf.Yapilis;
            tf.Hazirlanma = trf.Hazirlanma;
            tf.Pisirme = trf.Pisirme;
            tf.KacKisi = trf.KacKisi;
            tf.YayınTarihi = DateTime.Now;


            db.SaveChanges();





            return RedirectToAction("Tarifler");
        }









    }
}