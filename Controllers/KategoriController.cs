using MvcYemek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcYemek.Controllers
{
    public class KategoriController : Controller
    {
        MvcContext db = new MvcContext();
        // GET: Kategori
        public ActionResult Index()
        {
            
            return View(db.Kategori.ToList());
        }

        public ActionResult KategoriTarifListele(int id)
        {

            return View(id);
        }

        public ActionResult TarifListele(int id)
        {
            var data = db.Tarif.Where(x => x.KategoriID == id);
            return View("TarifListele",data);
        }


    }
}