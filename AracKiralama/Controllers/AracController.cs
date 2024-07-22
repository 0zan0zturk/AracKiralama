using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AracKiralama.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Identity.Client;

namespace AracKiralama.Controllers
{
    //[SessionCheckFilterAdmin]
    public class AracController : Controller
    {
        public class PhotoUploadViewModel
        {
            public IFormFile Photo { get; set; }
        }
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 2MB
        DataContext _context = new DataContext();

        // GET: Arac
        public async Task<IActionResult> List()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            return View(await _context.Araclar.Where(x => x.aracSil == true).OrderByDescending(x => x.AracMusait).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> List(string aracFiltre)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            ViewBag.filtre = aracFiltre;
            if (aracFiltre == "silinmis")
            {
                return View(await _context.Araclar.Where(x => x.aracSil == false).ToListAsync());
            }
            else if(aracFiltre == "hepsi")
            {
                return View(await _context.Araclar.OrderByDescending(x=>x.AracMusait).ToListAsync());
            } else if(aracFiltre == "musait")
            {
                return View(await _context.Araclar.Where(x => x.AracMusait == true && x.aracSil == true).ToListAsync());
            }
            else
            {
                return View(await _context.Araclar.Where(x => x.AracMusait == false).ToListAsync());
            }
        }

        // GET: Arac/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var arac = await _context.Araclar.FirstOrDefaultAsync(m => m.Id == id);
            if (arac == null)
            {
                return NotFound();
            }
            ViewBag.ToplamKiralanma = _context.KiralamaIslemleri.Count(x => x.AracId == id);
            ViewBag.ToplamCiro = _context.KiralamaIslemleri.Where(x => x.AracId == id).Sum(x => x.KiralamaBedeli);
            var sonKira = await _context.KiralamaIslemleri.Where(m => m.AracId == id).OrderByDescending(m => m.KiralamaBitisTarihi).Select(m => m.MusteriId).FirstOrDefaultAsync();
            ViewBag.SonKiralayan = await _context.Musteriler.Where(x => x.Id == sonKira).Select(x => x.AdSoyad).FirstOrDefaultAsync();

            return View(arac);
        }

        // GET: Arac/Create
        public async Task<IActionResult> Create()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            var yakitlar = await _context.Yakitlar.ToListAsync();
            var sanzimanlar = await _context.Sanzimanlar.ToListAsync();

            ViewBag.yakitlar = new SelectList(yakitlar, "YakitTipi", "YakitTipi");
            ViewBag.sanzimanlar = new SelectList(sanzimanlar, "SanzimanTipi", "SanzimanTipi");
            return View();
        }

        // POST: Arac/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AracPlaka,AracMarka,AracModel,AracYil,AracYakit,AracSanziman,AracMusait,MusteriId,SubeId,AracFotograf,Photo,GunlukFiyat,MotorHacmi")] Arac arac)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            if (ModelState.IsValid && arac.AracSanziman != null && arac.AracYakit != null)
            {
                var aracVarmi = await _context.Araclar.AnyAsync(x => x.AracPlaka == arac.AracPlaka);
                if (aracVarmi == true)
                {
                    ViewBag.HataPlaka = "Aynı Plakalı araç mevcut";
                    return View();
                }
                if (arac.Photo != null)
                {

                    try
                    {
                        //if (arac.AracFotograf == null || arac.AracFotograf.Length == 0)
                        //{
                        //    ModelState.AddModelError("Photo", "Lütfen yüklemek için bir fotoğraf seçin.");
                        //    return View();
                        //}

                        var extension = Path.GetExtension(arac.Photo.FileName).ToLowerInvariant();

                        if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("Photo", "Geçersiz dosya türü. Sadece JPG, JPEG, PNG ve GIF dosyalarına izin verilir.");
                            return View();
                        }

                        if (arac.Photo.Length > MaxFileSize)
                        {
                            ModelState.AddModelError("Photo", "Dosya boyutu çok büyük. Maksimum izin verilen boyut 2MB'dir.");
                            return View();
                        }
                        var dosyaAdi = arac.AracModel.ToLower() + extension;
                        var filePath = Path.Combine("wwwroot/aracImg", dosyaAdi);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await arac.Photo.CopyToAsync(stream);
                        }
                        arac.MusteriId = 1;
                        arac.AracFotograf = dosyaAdi.ToLower();
                        arac.KiralamaIslemiId = _context.KiralamaIslemleri.Max(x => x.Id) + 1;
                        _context.Araclar.Add(arac);
                        await _context.SaveChangesAsync();
                        var kiralamaIslemiEkle = new KiralamaIslemi()
                        {
                            KiralamaBaslangicTarihi = DateTime.Now.AddDays(-365),
                            KiralamaBaslangicSaati = 8,
                            KiralamaBitisTarihi = DateTime.Now.AddDays(-364),
                            KiralamaBitisSaati = 8,
                            MusteriId = 1,
                            AracId = _context.Araclar.Max(x => x.Id),
                            SubeId = arac.SubeId,
                            SehirId = 1,
                            IslemZamani = DateTime.Now.AddDays(-366),
                            KiralananGunSayisi = 1,
                            KiralamaBedeli = 0,
                        };
                        _context.KiralamaIslemleri.Add(kiralamaIslemiEkle);

                        await _context.SaveChangesAsync();
                        return RedirectToAction("List", "Arac");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.HataAracEkle = "Yeni Araç Eklenemedi";
                        return RedirectToAction("List", "Arac");
                    }
                }
            }
            var yakitlar = await _context.Yakitlar.ToListAsync();
            var sanzimanlar = await _context.Sanzimanlar.ToListAsync();

            ViewBag.yakitlar = new SelectList(yakitlar, "YakitTipi", "YakitTipi");
            ViewBag.sanzimanlar = new SelectList(sanzimanlar, "SanzimanTipi", "SanzimanTipi");
            return View(arac);
        }
        // GET: Arac/Edit/5
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

            var arac = await _context.Araclar.FindAsync(id);
            if (arac == null)
            {
                return NotFound();
            }
            var yakitlar = await _context.Yakitlar.ToListAsync();
            var sanzimanlar = await _context.Sanzimanlar.ToListAsync();

            ViewBag.yakitlar = new SelectList(yakitlar, "YakitTipi", "YakitTipi");
            ViewBag.sanzimanlar = new SelectList(sanzimanlar, "SanzimanTipi", "SanzimanTipi");

            return View(arac);
        }

        // POST: Arac/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AracPlaka,AracMarka,AracModel,AracYil,AracYakit,AracSanziman,AracMusait,MusteriId,SubeId,MotorHacmi,AracFotograf,Photo,GunlukFiyat")] Arac arac)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            if (id != arac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Araç verilerini veritabanından al
                    var aracBul = await _context.Araclar.AsNoTracking().FirstOrDefaultAsync(x => x.AracPlaka == arac.AracPlaka);
                    if (aracBul == null)
                    {
                        ModelState.AddModelError("", "Araç bulunamadı.");
                        return View(arac);
                    }

                    // Fotoğraf işleme
                    if (arac.Photo != null)
                    {
                        var extension = Path.GetExtension(arac.Photo.FileName).ToLowerInvariant();

                        if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("Photo", "Geçersiz dosya türü. Sadece JPG, JPEG, PNG ve GIF dosyalarına izin verilir.");
                            return View(arac);
                        }

                        if (arac.Photo.Length > MaxFileSize)
                        {
                            ModelState.AddModelError("Photo", "Dosya boyutu çok büyük. Maksimum izin verilen boyut 2MB'dir.");
                            return View(arac);
                        }

                        var dosyaAdi = arac.AracModel.ToLower() + extension;
                        arac.AracFotograf = arac.Id + "ID_" + dosyaAdi;
                        var filePath = Path.Combine("wwwroot/aracImg", arac.AracFotograf);

                        // Dosyayı kaydet
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await arac.Photo.CopyToAsync(stream);
                        }

                    }
                    else
                    {
                        // Fotoğraf yüklenmemişse, mevcut fotoğrafı koru
                        arac.AracFotograf = aracBul.AracFotograf;
                    }

                    // Araç bilgilerini güncelle
                    arac.KiralamaIslemiId = aracBul.KiralamaIslemiId;
                    arac.MusteriId = aracBul.MusteriId;
                    arac.AracPlaka = aracBul.AracPlaka;

                    _context.Update(arac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AracExists(arac.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
                    return View(arac);
                }

                return RedirectToAction(nameof(List));
            }

            return View(arac);
        }

        // GET: Arac/Delete/5
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

            var arac = await _context.Araclar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arac == null)
            {
                return NotFound();
            }

            return View(arac);
        }

        // POST: Arac/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            var arac = await _context.Araclar.FindAsync(id);
            if (arac != null)
            {
                arac.aracSil = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        private bool AracExists(int id)
        {
            return _context.Araclar.Any(e => e.Id == id);
        }
    }
}
