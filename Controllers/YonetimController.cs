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
    public class YonetimController : Controller
    {
        MvcContext db = new MvcContext();

        // GET: Yonetim
        [HttpGet]
        public ActionResult Index()
        {
            
            ViewBag.profil = Session["Yonetici"];
            if (ViewBag.profil == null)
            {
                //kullanıcı bulunamadı 
                return RedirectToAction("Index","GirisYap");
            }           
            return View();          
        }

        [HttpPost]
        public ActionResult Index(Yonetici kull)
        {

            //int id= ViewBag.KullaniciID;

            //kull.KullaniciID = id;
            Yonetici profilSahibi = Session["Yonetici"] as Yonetici;
            kull.YoneticiID = profilSahibi.YoneticiID;
            Yonetici kulgun = db.Yonetici.Find(kull.YoneticiID);

            
            kulgun.YoneticiAd = kull.YoneticiAd;                    
            kulgun.YoneticiSifre = kull.YoneticiSifre;
       

            Session["Yonetici"] = kulgun;
            ViewBag.profil = Session["Yonetici"];


            db.SaveChanges();

          
            return RedirectToAction("Index","Yonetim");
        }

        //TARİFLER

        public ActionResult Tarifler(string p)
        {
            var tarifler = from t in db.Tarif select t;
            if (!string.IsNullOrEmpty(p))
            {
                tarifler = tarifler.Where(x => x.Baslik.Contains(p));
            }
            return View(tarifler.ToList());

            // var tarifler = db.Tarif.ToList();
            // return View(tarifler);
        }
        [HttpGet]
        public ActionResult TarifYaz()
        {

            ViewBag.Kategori = db.Kategori.ToList();
            ViewBag.Etiket = db.Etiket.ToList();
            return View();



        }

        [HttpPost]
        public  ActionResult TarifYaz(Tarif tarif,HttpPostedFileBase Resim)
        {

            
            if (tarif != null)
            {


                Yonetici aktif = Session["Yonetici"] as Yonetici;
                tarif.YayınTarihi= DateTime.Now;                
                tarif.YoneticiID = aktif.YoneticiID;
                tarif.KapakResimID = ResimKaydet(Resim, HttpContext);
                db.Tarif.Add(tarif);
                db.SaveChanges();

               
            
            }

           
            db.SaveChanges();



            return RedirectToAction("Tarifler");
        }


        public static int ResimKaydet(HttpPostedFileBase Resim, HttpContextBase ctx)
        {
            MvcContext db = new MvcContext();


            Yonetici k = (Yonetici)ctx.Session["Yonetici"];

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
            dbRes.YoneticiID = k.YoneticiID;

            db.Resim.Add(dbRes);
            db.SaveChanges();

            return dbRes.ResimID;

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
            Yonetici aktif = Session["Yonetici"] as Yonetici;
           
            tf.YoneticiID = aktif.YoneticiID;
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


        //KULLANICILAR
        public ActionResult Kullanicilar(string p)
        {
            var kullanicilar = from kl in db.Kullanici select kl;
            if (!string.IsNullOrEmpty(p))
            {
                kullanicilar = kullanicilar.Where(x => x.KullaniciAdi.Contains(p));
            }
            return View(kullanicilar.ToList());

            //  var kullanici = db.Kullanici.ToList();
            // return View(kullanici);
        }

        public ActionResult KullaniciSil(int id)
        {        
            
            Kullanici kl = db.Kullanici.Where(x => x.KullaniciID == id).SingleOrDefault();
            List<Tarif> trfkl = db.Tarif.Where(z => z.KullaniciID == id).ToList();
            if (kl == null)
            {
                //kategori yok
            }
            else
            {
                if (trfkl != null)
                {
                    foreach (var trf in trfkl)
                    {
                        db.Tarif.Remove(trf);
                    }

                }
                else
                {
                        //kullanıcı tarif eklemediyse null ise döngüden çık
                }
              
                
                db.Kullanici.Remove(kl);
                db.SaveChanges();
            }

            return RedirectToAction("Kullanicilar");
        }

        [HttpGet]
        public ActionResult KullaniciEkle()
        {


            return View();
        }

        [HttpPost]
        public ActionResult KullaniciEkle(Kullanici kl)// HttpPostedFileBase Resim
        {
            kl.ResimID = 1010;
            kl.KayıtTarihi = DateTime.Now;
            db.Kullanici.Add(kl);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }


        [HttpGet]
        public ActionResult KullaniciGuncelle(int id)
        {

            Kullanici kl = db.Kullanici.Find(id);
            if (kl == null)
            {
                //kullanıcı bulunamadı 
                return RedirectToAction("Kullanicilar");
            }

            TempData["KullaniciID"] = id;
            return View(kl);
        }
        [HttpPost]
        public ActionResult KullaniciGuncelle(Kullanici kl, HttpPostedFileBase ResimID)
        {
            int KullaniciID = (int)TempData["KullaniciID"];
            Kullanici kul = db.Kullanici.Find(KullaniciID);

           
            kl.KullaniciID = KullaniciID;
            if (ResimID != null)
            {
                kl.ResimID = ResimKaydet(ResimID, HttpContext);
            }
            else
            {
                //resim yüklemeden devam edecek.
                kl.ResimID = 1015;
            }
          

            kul.Ad = kl.Ad;
            kul.Soyad = kl.Soyad;
            kul.KullaniciAdi = kl.KullaniciAdi;
            kul.Eposta = kl.Eposta;
            kul.ResimID = kl.ResimID;
            kul.Sifre = kl.Sifre;


            db.SaveChanges();


            return RedirectToAction("Kullanicilar");
        }







        //KATEGORİLER

        public ActionResult Kategoriler(string p)
        {
            var kategoriler = from k in db.Kategori select k;
            if (!string.IsNullOrEmpty(p))
            {
                kategoriler = kategoriler.Where(x => x.KategoriAd.Contains(p));
            }
            //var kategoriler = db.Kategori.ToList();
            return View(kategoriler.ToList());
        }

        [HttpGet]
        public ActionResult KategoriEkle()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategori kat, HttpPostedFileBase Resimk)// HttpPostedFileBase Resim
        {
            Yonetici katSahip = Session["Yonetici"] as Yonetici;
            //katSahip.KullaniciID = katSahip.KullaniciID;
            //var KatEkId = katSahip.KullaniciID;
            kat.KategoriResim = ResimKaydet(Resimk, HttpContext);

            db.Kategori.Add(kat);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public ActionResult KategoriSil(int id)
        {
            Kategori k = db.Kategori.Where(x => x.KategoriID == id).SingleOrDefault();
            if (k ==null)
            {
                //kategori yok
            }
            else
            {
                db.Kategori.Remove(k);
                db.SaveChanges();
            }

            return RedirectToAction("Kategoriler");
        }

        [HttpGet]
        public ActionResult KategoriGuncelle(int id)
        {

            Kategori k = db.Kategori.Find(id);
            if (k == null)
            {
                //kullanıcı bulunamadı 
                return RedirectToAction("Kategoriler");
            }

            TempData["KategoriID"] = id;
            return View(k);
        }
        [HttpPost]
        public ActionResult KategoriGuncelle(Kategori kt, HttpPostedFileBase Resimkg)
        {
            Yonetici katSahipg = Session["Yonetici"] as Yonetici;
            int KategoriID = (int)TempData["KategoriID"];

            Kategori kat = db.Kategori.Find(KategoriID);

            kat.KategoriAd = kt.KategoriAd;
            if (Resimkg != null)
            {
                kat.KategoriResim = ResimKaydet(Resimkg, HttpContext);
            }
            else
            {
                //resim yüklemeden devam edecek.
            }
           
            
            db.SaveChanges();


            return RedirectToAction("Kategoriler");
        }







        //ETİKETLER

        public ActionResult Etiketler(string p)
        {
            var etiketler = from e in db.Etiket select e;
            if (!string.IsNullOrEmpty(p))
            {
                etiketler = etiketler.Where(x => x.Ad.Contains(p));
            }          
            return View(etiketler.ToList());

            //var Etiketler = db.Etiket.ToList();
            // return View(Etiketler);
        }

        [HttpGet]
        public ActionResult EtiketEkle()
        {


            return View();
        }

        [HttpPost]
        public ActionResult EtiketEkle(Etiket et)// HttpPostedFileBase Resim
        {

            db.Etiket.Add(et);
            db.SaveChanges();
            return RedirectToAction("Etiketler");
        }

        public ActionResult EtiketSil(int id)
        {
            Etiket et = db.Etiket.Where(x => x.EtiketID == id).SingleOrDefault();
            if (et == null)
            {
                //kategori yok
            }
            else
            {
                db.Etiket.Remove(et);
                db.SaveChanges();
            }

            return RedirectToAction("Etiketler");
        }

        [HttpGet]
        public ActionResult EtiketGuncelle(int id)
        {

            Etiket etk = db.Etiket.Find(id);
            if (etk == null)
            {
                //kullanıcı bulunamadı 
                return RedirectToAction("Etiketler");
            }

            TempData["EtiketID"] = id;
            return View(etk);
        }
        [HttpPost]
        public ActionResult EtiketGuncelle(Etiket etkt)
        {
            int EtiketID = (int)TempData["EtiketID"];

            Etiket et = db.Etiket.Find(EtiketID);
            et.Ad = etkt.Ad;
            
            db.SaveChanges();


            return RedirectToAction("Etiketler");
        }








    }
}