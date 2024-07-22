using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AracKiralama.Models;
using Humanizer;
using Microsoft.Azure.Documents.Linq;

namespace AracKiralama.Controllers
{
    public class KiralamaIslemiController : Controller
    {
        DataContext _context = new DataContext();

        // GET: KiralamaIslemis
        public async Task<IActionResult> Index()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null) return RedirectToAction("Index", "Home");
            var dataContext = _context.KiralamaIslemleri.Include(k => k.Arac).Include(k => k.Musteri).Include(k => k.Sube);
            return View(await dataContext.ToListAsync());
        }

        // GET: KiralamaIslemis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null) return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            var kiralamaIslemi = await _context.KiralamaIslemleri
                .Include(k => k.Arac)
                .Include(k => k.Musteri)
                .Include(k => k.Sube)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kiralamaIslemi == null)
            {
                return NotFound();
            }

            return View(kiralamaIslemi);
        }

        // GET: KiralamaIslemis/Create
        public IActionResult Create()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            ViewData["AracId"] = new SelectList(_context.Araclar, "Id", "Id");
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "Id", "Id");
            ViewData["SubeId"] = new SelectList(_context.Subeler, "Id", "Id");
            return View();
        }

        // POST: KiralamaIslemis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KiralamaBaslangicTarihi,KiralamaBaslangicSaati,KiralamaBitisTarihi,KiralamaBitisSaati,MusteriId,AracId,SubeId")] KiralamaIslemi kiralamaIslemi)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Add(kiralamaIslemi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AracId"] = new SelectList(_context.Araclar, "Id", "Id", kiralamaIslemi.AracId);
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "Id", "Id", kiralamaIslemi.MusteriId);
            ViewData["SubeId"] = new SelectList(_context.Subeler, "Id", "Id", kiralamaIslemi.SubeId);
            return View(kiralamaIslemi);
        }

        // GET: KiralamaIslemis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var kiralamaIslemi = await _context.KiralamaIslemleri.FindAsync(id);
            if (kiralamaIslemi == null)
            {
                return NotFound();
            }
            ViewData["AracId"] = new SelectList(_context.Araclar, "Id", "Id", kiralamaIslemi.AracId);
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "Id", "Id", kiralamaIslemi.MusteriId);
            ViewData["SubeId"] = new SelectList(_context.Subeler, "Id", "Id", kiralamaIslemi.SubeId);
            return View(kiralamaIslemi);
        }

        // POST: KiralamaIslemis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KiralamaBaslangicTarihi,KiralamaBaslangicSaati,KiralamaBitisTarihi,KiralamaBitisSaati,MusteriId,AracId,SubeId")] KiralamaIslemi kiralamaIslemi)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            if (id != kiralamaIslemi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kiralamaIslemi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KiralamaIslemiExists(kiralamaIslemi.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AracId"] = new SelectList(_context.Araclar, "Id", "Id", kiralamaIslemi.AracId);
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "Id", "Id", kiralamaIslemi.MusteriId);
            ViewData["SubeId"] = new SelectList(_context.Subeler, "Id", "Id", kiralamaIslemi.SubeId);
            return View(kiralamaIslemi);
        }

        // GET: KiralamaIslemis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var kiralamaIslemi = await _context.KiralamaIslemleri
                .Include(k => k.Arac)
                .Include(k => k.Musteri)
                .Include(k => k.Sube)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kiralamaIslemi == null)
            {
                return NotFound();
            }

            return View(kiralamaIslemi);
        }

        // POST: KiralamaIslemis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            var kiralamaIslemi = await _context.KiralamaIslemleri.FindAsync(id);
            if (kiralamaIslemi != null)
            {
                _context.KiralamaIslemleri.Remove(kiralamaIslemi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Kirala(int SehirBilgisi, DateTime KiralamaBaslangicTarihi, int KiralamaBaslangicSaati, DateTime KiralamaBitisTarihi, int KiralamaBitisSaati, double GunlukFiyat)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null) return RedirectToAction("Index", "Home");
            int gunSayisi = 1;
            if (KiralamaBaslangicSaati < KiralamaBitisSaati)
            {
                gunSayisi = (Int32)(KiralamaBitisTarihi - KiralamaBaslangicTarihi).TotalDays + 1;
            }
            else
            {
                gunSayisi = (Int32)(KiralamaBitisTarihi - KiralamaBaslangicTarihi).TotalDays;
            }
            //var AracBilgileri = await _context.Araclar.Where(x => x.AracMusait == true).ToListAsync();
            var bugun = DateTime.Now;
            var AracBilgileri = await _context.KiralamaIslemleri
                 .Include(k => k.Arac) // Arac bilgilerini dahil ediyoruz
                 .Where(k => k.KiralamaBaslangicTarihi.AddDays(k.KiralananGunSayisi) < bugun || k.KiralamaBitisTarihi < KiralamaBaslangicTarihi || k.KiralamaBaslangicTarihi > KiralamaBitisTarihi)
                 .Where(x => x.Arac.AracMusait == true && x.Arac.aracSil == true)
                 .Where(x => x.Arac.KiralamaIslemiId == x.Id || x.Arac.KiralamaIslemiId == 0)
                 .Select(k => new
                 {
                     KiralamaId = k.Id,
                     k.KiralamaBaslangicTarihi,
                     k.KiralamaBitisTarihi,
                     Arac = new
                     {
                         k.Arac.Id,
                         k.Arac.AracMarka,
                         k.Arac.AracModel,
                         k.Arac.AracYil,
                         k.Arac.AracYakit,
                         k.Arac.AracSanziman,
                         k.Arac.MotorHacmi,
                         k.Arac.AracFotograf,
                         k.Arac.GunlukFiyat
                     }
                 })
                 .ToListAsync();
            var araclar = AracBilgileri.Select(k => new Arac
            {
                Id = k.Arac.Id,
                AracMarka = k.Arac.AracMarka,
                AracModel = k.Arac.AracModel,
                AracYil = k.Arac.AracYil,
                AracYakit = k.Arac.AracYakit,
                AracSanziman = k.Arac.AracSanziman,
                MotorHacmi = k.Arac.MotorHacmi,
                AracFotograf = k.Arac.AracFotograf,
                GunlukFiyat = k.Arac.GunlukFiyat

            }).ToList().OrderByDescending(x => x.AracYil);
            var aracSayisi = araclar.Count();


            ViewBag.KiralamaBilgi = new KiralamaIslemi
            {
                SehirId = SehirBilgisi,
                KiralamaBaslangicTarihi = KiralamaBaslangicTarihi,
                KiralamaBaslangicSaati = KiralamaBaslangicSaati,
                KiralamaBitisTarihi = KiralamaBitisTarihi,
                KiralamaBitisSaati = KiralamaBitisSaati,
                KiralananGunSayisi = gunSayisi,
                KiralamaBedeli = gunSayisi * GunlukFiyat

            };
            ViewBag.GunSayisi = gunSayisi;
            if (KiralamaBaslangicSaati == -1 || KiralamaBitisSaati == -1 || SehirBilgisi == -1)
            {
                //ViewBag.KiralamaBilgileri = KiralamaBilgileri;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AracSayisi = "Sizin için " + aracSayisi + " adet araç bulduk!";
            return View(araclar.DistinctBy(x => x.Id).ToList());
        }
        public async Task<IActionResult> KiralamaBilgileri(int AracId, int SehirId, DateTime KiralamaBaslangicTarihi, int KiralamaBaslangicSaati, DateTime KiralamaBitisTarihi, int KiralamaBitisSaati, int KiralananGunSayisi)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null) return RedirectToAction("Index", "Home");
            var aracAdi = await _context.Araclar.FindAsync(AracId);
            var sehirAdi = await _context.Sehirler.FindAsync(SehirId);
            if (aracAdi != null && sehirAdi != null)
            {
                ViewBag.Bilgilendirme1 = "Kiralama başlangıç tarihi ve saati : " + KiralamaBaslangicTarihi.ToShortDateString() + " " + KiralamaBaslangicSaati + ":00";
                ViewBag.Bilgilendirme2 = "Kiralama bitiş tarihi ve saati : " + KiralamaBitisTarihi.ToShortDateString() + " " + KiralamaBitisSaati + ":00";
                ViewBag.Bilgilendirme3 = "Araç Bilgileri : " + aracAdi.AracMarka + " " + aracAdi.AracModel;
                ViewBag.Bilgilendirme4 = "Yakıt : " + aracAdi.AracYakit;
                ViewBag.Bilgilendirme5 = "Şanzıman : " + aracAdi.AracSanziman;
                ViewBag.Bilgilendirme6 = "Toplam tutar : ₺" + KiralananGunSayisi * aracAdi.GunlukFiyat;
                KiralamaIslemi KiralamaSon = new KiralamaIslemi
                {
                    KiralamaBaslangicTarihi = KiralamaBaslangicTarihi,
                    KiralamaBaslangicSaati = KiralamaBaslangicSaati,
                    KiralamaBitisTarihi = KiralamaBitisTarihi,
                    KiralamaBitisSaati = KiralamaBitisSaati,
                    MusteriId = HttpContext.Session.GetInt32("MusteriId") ?? 1,
                    AracId = AracId,
                    SubeId = 1,
                    SehirId = SehirId,
                    KiralananGunSayisi = KiralananGunSayisi,
                    KiralamaBedeli = KiralananGunSayisi * aracAdi.GunlukFiyat

                };

                return View(KiralamaSon);
            }
            return View();
        }
        public async Task<IActionResult> KiralamaSon(KiralamaIslemi sonIslem, KartBilgileri musteriKart)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null) return RedirectToAction("Index", "Home");
            await _context.KiralamaIslemleri.AddAsync(sonIslem);
            var arabaFalse = await _context.Araclar.FindAsync(sonIslem.AracId);
            var musteriTabloGuncelle = await _context.Musteriler.FindAsync(HttpContext.Session.GetInt32("MusteriId"));
            musteriTabloGuncelle.AracId = sonIslem.AracId;
            arabaFalse.MusteriId = sonIslem.MusteriId;
            //arabaFalse.AracMusait = false;
            if (arabaFalse != null)
                arabaFalse.KiralamaIslemiId = await _context.KiralamaIslemleri.MaxAsync(x => x.Id) + 1;
            await _context.SaveChangesAsync();
            ViewBag.SubeAdresi = _context.Subeler.Where(x => x.Id == sonIslem.SubeId).Select(x => x.SubeAdresi).FirstOrDefault();
            return View(sonIslem);

        }
        
        public async Task<IActionResult> AracListele()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null) return RedirectToAction("Index", "Home");
            var arabalar = _context.Araclar.ToListAsync();
            return View(await arabalar);
        }
        [SessionCheckFilterAdmin]
        public async Task<IActionResult> TumKiralamalar()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            var kiralamaIslemleri = await _context.KiralamaIslemleri
                .Include(x => x.Arac)
                .Include(x => x.Musteri)
                .Include(x => x.Sube).OrderByDescending(x=>x.Id)
                .ToListAsync();

            var viewModel = new KiralamaViewModel
            {
                KiralamaIslemleri = kiralamaIslemleri,
                BaslangicTarihi = DateTime.Now.AddDays(-10),
                BitisTarihi = DateTime.Now.AddDays(10),
                Filtre = "0"
            };

            return View(viewModel);
            //return View(await _context.KiralamaIslemleri.Include(x=>x.Arac).Include(x=>x.Musteri).Include(x=>x.Sube).ToListAsync());
        }
        [HttpPost]
        [SessionCheckFilterAdmin]
        public async Task<IActionResult> TumKiralamalar(string? Filtre, DateTime? BaslangicTarihi, DateTime? BitisTarihi)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            if (Filtre != null)
            {
                if (Filtre == "aktif")
                {
                    var aktiff = await _context.KiralamaIslemleri.Include(x => x.Arac).Include(x => x.Musteri).Include(x => x.Sube).Where(x => x.KiralamaBitisTarihi > DateTime.Now && x.KiralamaBaslangicTarihi < DateTime.Now).ToListAsync();
                    var viewModel = new KiralamaViewModel
                    {
                        KiralamaIslemleri = aktiff,
                        BaslangicTarihi = DateTime.Now.AddDays(-10),
                        BitisTarihi = DateTime.Now.AddDays(10),
                        Filtre = "aktif"
                    };
                    return View(viewModel);
                }
                else if (Filtre == "gecmis")
                {
                    var gecmiss = await _context.KiralamaIslemleri.Include(x => x.Arac).Include(x => x.Musteri).Include(x => x.Sube).Where(x => x.KiralamaBitisTarihi < DateTime.Now).ToListAsync();
                    var viewModel = new KiralamaViewModel
                    {
                        KiralamaIslemleri = gecmiss,
                        BaslangicTarihi = DateTime.Now.AddDays(-10),
                        BitisTarihi = DateTime.Now.AddDays(10),
                        Filtre = "gecmis"
                    };
                    return View(viewModel);
                }
                else if (Filtre == "gelecek")
                {
                    var gelecekk = await _context.KiralamaIslemleri.Include(x => x.Arac).Include(x => x.Musteri).Include(x => x.Sube).Where(x => x.KiralamaBaslangicTarihi > DateTime.Now).ToListAsync();
                    var viewModel = new KiralamaViewModel
                    {
                        KiralamaIslemleri = gelecekk,
                        BaslangicTarihi = DateTime.Now.AddDays(-10),
                        BitisTarihi = DateTime.Now.AddDays(10),
                        Filtre = "gelecek"
                    };
                    return View(viewModel);
                }
                else { 
                    var digerr = await _context.KiralamaIslemleri.Include(x => x.Arac).Include(x => x.Musteri).Include(x => x.Sube).ToListAsync();
                    var viewModel = new KiralamaViewModel
                    {
                        KiralamaIslemleri = digerr,
                        BaslangicTarihi = DateTime.Now.AddDays(-10),
                        BitisTarihi = DateTime.Now.AddDays(10),
                        Filtre = "hepsi"
                    };
                    return View(viewModel);
                }
            }
            else
            {
                var kiralamaIslemleri = await _context.KiralamaIslemleri
                    .Include(x => x.Arac)
                    .Include(x => x.Musteri)
                    .Include(x => x.Sube)
                    .Where(x => x.KiralamaBaslangicTarihi >= BaslangicTarihi && x.KiralamaBitisTarihi <= BitisTarihi)
                    .ToListAsync();

                if (BaslangicTarihi != null && BitisTarihi != null)
                {
                    var viewModel = new KiralamaViewModel
                    {
                        KiralamaIslemleri = kiralamaIslemleri,
                        BaslangicTarihi = BaslangicTarihi.Value,
                        BitisTarihi = BitisTarihi.Value
                    };
                    return View(viewModel);
                }
                else
                {
                    var viewModel = new KiralamaViewModel
                    {
                        KiralamaIslemleri = kiralamaIslemleri,
                        BaslangicTarihi = DateTime.Now.AddDays(-10),
                        BitisTarihi = DateTime.Now.AddDays(10),
                        Filtre = "0"
                    };
                    return View(viewModel);
                }
            }
        }

        private bool KiralamaIslemiExists(int id)
        {
            return _context.KiralamaIslemleri.Any(e => e.Id == id);
        }
    }
}
