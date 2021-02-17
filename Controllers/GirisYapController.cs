using MvcYemek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcYemek.Controllers
{
    public class GirisYapController : Controller
    {
        MvcContext db = new MvcContext();
        // GET: GirisYap
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string KullaniciAdi,string Sifre)
        {
            Kullanici k = db.Kullanici.Where(x => x.KullaniciAdi == KullaniciAdi && x.Sifre == Sifre).SingleOrDefault();
            Yonetici yn = db.Yonetici.Where(x => x.YoneticiAd == KullaniciAdi && x.YoneticiSifre == Sifre).SingleOrDefault();
            if (yn != null)
            {
                Session["Yonetici"] = yn;
                return RedirectToAction("Index", "Yonetim");
            }
            if (k ==null )
            {
                ViewBag.Sonuc = "Kullanıcı adınız veya şifreniz hatalıdır !";
                return View();
            }        
            else
            {
                Session["Kullanici"] = k;
                return RedirectToAction("Index", "Home");
            }


          
        }
    }
}