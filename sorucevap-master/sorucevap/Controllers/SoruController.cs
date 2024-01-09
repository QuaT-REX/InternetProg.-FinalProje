using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sorucevap.Models;
using sorucevap.ViewModel;

namespace sorucevap.Controllers
{
    public class SoruController : Controller
    {
        private readonly Context _c;

        public SoruController(Context c)
        {
            _c = c;
        }

        public IActionResult Index()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var values = _c.sorus.Where(x=>x.userId == id).Select(x => new SoruListe
            {
                id = x.Id,
                baslik = x.Baslik,
                icerik = x.Icerik,
                Durum = x.Durum,
                SoruSoranBilgisi = x.SoruSoranBilgi,
            }).ToList();
            return View(values);
        }

        public IActionResult Sil(int id)
        {
            var values = _c.sorus.Find(id);
            _c.sorus.Remove(values);
            _c.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Duzenle(int id)
        {
            var values = _c.sorus.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult Duzenle(int id , Soru soru)
        {
            var values = _c.sorus.Find(id);
            values.Baslik = soru.Baslik;
            values.Icerik = soru.Icerik;
            values.Durum = values.Durum;
            values.userId = values.userId;
            values.SoruSoranBilgi = values.SoruSoranBilgi;
            _c.sorus.Update(values);
            _c.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Ekle()
        {            
            return View();
        }
        [HttpPost]
        public IActionResult Ekle(Soru soru)
        {
            int id =Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var name = HttpContext.Session.GetString("name");
            var surname = HttpContext.Session.GetString("surname");
            Soru values = new Soru();
            values.Baslik = soru.Baslik;
            values.Icerik = soru.Icerik;
            values.Durum = false;
            values.SoruSoranBilgi = name + " " + surname;
            values.userId = id;
            _c.sorus.Add(values);
            _c.SaveChanges();
            return RedirectToAction("Index");

        }

        //---------------------------------------------

        [Authorize(Roles = "Admin")]
        public IActionResult GelenSorular()
        {
            var values = _c.sorus.Select(x => new SoruListe
            {
                id = x.Id,
                baslik = x.Baslik,
                icerik = x.Icerik,
                SoruSoranBilgisi = x.SoruSoranBilgi
            }).ToList();
            return View(values);
        }



        public IActionResult CevapVer(int id , Cevap cevap)
        {
            Cevap cvp = new Cevap();
            cvp.CevapIcerik = cevap.CevapIcerik;
            cvp.soruId = id;
            var soruBul =  _c.sorus.Find(id);
            soruBul.Durum = true;
            _c.cevaps.Add(cvp);
            _c.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult CevapGetir(int id)
        {
            var soru = _c.cevaps.Where(x=>x.soruId == id);
            return Json(soru);
        }
    }
}
