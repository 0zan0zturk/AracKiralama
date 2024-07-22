using AracKiralama.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Controllers
{
[SessionCheckFilterAdmin]
    public class AdminController : Controller
    {
        DataContext _context = new DataContext();

        public async Task<IActionResult> Rapor()
        {
            
            var bugun = DateTime.Now;
            ViewBag.KiradakiAracSayisi = _context.KiralamaIslemleri.AsNoTracking().Include(k => k.Arac) // Arac bilgilerini dahil ediyoruz
                 .Where(k => k.KiralamaBaslangicTarihi <= bugun && k.KiralamaBitisTarihi >= bugun).Count();
            ViewBag.ToplamCiro = _context.KiralamaIslemleri.Sum(x => x.KiralamaBedeli);
            ViewBag.HasarliAracSayisi = _context.Araclar.Where(x => x.AracMusait == false).Count();
            ViewBag.ToplamIslemSayisi = _context.KiralamaIslemleri.Count() - _context.Araclar.Count();
            ViewBag.ToplamKiralananGunSayisi = _context.KiralamaIslemleri.Sum(x => x.KiralananGunSayisi) - _context.Araclar.Count();
            var enSikKiralanan = _context.KiralamaIslemleri
                .GroupBy(k => k.AracId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    AracId = g.Key,
                    KiralamaSayisi = g.Count()
                })
                .FirstOrDefault();
            if (enSikKiralanan != null)
            {
                var arac = _context.Araclar.FirstOrDefault(x => x.Id == enSikKiralanan.AracId);
                ViewBag.EnSikKiralanan = arac;
            }
            else
            {
                ViewBag.EnSikKiralanan = null;
            }
            var enSikKiralayan = _context.KiralamaIslemleri
                .GroupBy(k => k.MusteriId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    MusteriId = g.Key,
                    KiralamaSayisi = g.Count()

                })
                .FirstOrDefault();
            if (enSikKiralayan != null)
            {
                var musteri = _context.Musteriler.FirstOrDefault(x => x.Id == enSikKiralayan.MusteriId);
                ViewBag.EnSikKiralayan = musteri;
            }
            else
            {
                ViewBag.EnSikKiralayan = null;
            }
            ViewBag.GirisSayisi = _context.MusteriGirisleri.Where(x => x.GirisZamani > DateTime.Now.AddDays(-365)).Count();
            ViewBag.GirisSayisiTumZamanlar = _context.MusteriGirisleri.Count();
            return View();

        }
    }
}
