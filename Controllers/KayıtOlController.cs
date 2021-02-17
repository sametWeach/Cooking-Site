using MvcYemek.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcYemek.Controllers
{
    public class KayıtOlController : Controller
    {
        MvcContext db = new MvcContext();
        // GET: KayıtOl
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Kullanici k)//HttpPostedFileBase ResimKayıt
        {


            k.ResimID = 1010;
            k.KayıtTarihi = DateTime.Now;         
            db.Kullanici.Add(k);
            db.SaveChanges();
            return RedirectToAction("Index", "GirisYap");
        }
       
    }
}