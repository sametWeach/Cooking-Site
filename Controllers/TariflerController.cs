using MvcYemek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcYemek.Controllers
{
    public class TariflerController : Controller
    {
        MvcContext db = new MvcContext();
        // GET: Tarifler
        public ActionResult Index(int id)
        {
            //detay

            Tarif tr = db.Tarif.SingleOrDefault(x => x.TarifID == id);
            return View(tr);
        }
    }
}