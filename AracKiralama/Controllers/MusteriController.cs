using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AracKiralama.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using NuGet.Protocol;
using Microsoft.Azure.Documents;
using AracKiralama.Helpers;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Drawing;

namespace AracKiralama.Controllers
{
    public class MusteriController : Controller
    {
        DataContext _context = new DataContext();


        // GET: Musteris
        public async Task<IActionResult> List()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }

            // Admin değilse ve düzenlemek istediği müşteri kendisi değilse hata döndür
            if (sessionId != 1)
            {
                return NotFound();
            }
            ViewBag.Filtre = "hepsi";
            return View(await _context.Musteriler.Where(x=>x.MusteriAktif== true).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> List(string musteriFiltre)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null || sessionId != 1)
            {
                return NotFound();
            }
            // Admin değilse ve düzenlemek istediği müşteri kendisi değilse hata döndür
            
            if (musteriFiltre == "hepsi")
            {
                return View(await _context.Musteriler.ToListAsync());
            }
            else if (musteriFiltre == "aktif")
            {
                return View(await _context.Musteriler.Where(x => x.MusteriAktif == true).ToListAsync());
            }
            else if (musteriFiltre == "silinmis")
            {
                return View(await _context.Musteriler.Where(x => x.MusteriAktif == false).ToListAsync());
            }
            else
            {
                return View(await _context.Musteriler.OrderBy(x => x.AdSoyad).ToListAsync());
            }


        }

        // GET: Musteris/Details/5
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

            var musteri = await _context.Musteriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
        }

       
        // GET: Musteris/Edit/5
        [SessionCheckFilterNormal]
        public async Task<IActionResult> Duzenle(int? id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }
            if (sessionId != 1 && sessionId != id)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }
            var musteri = await _context.Musteriler.FindAsync(id);
            if (musteri == null)
            {
                return NotFound();
            }
            return View(musteri);
        }
        public IActionResult KayitOl()
        {
            return View();
        }

        // POST: Musteris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public class PhotoUploadViewModel
        {
            public IFormFile Photo { get; set; }
        }
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 2 * 1024 * 1024; // 2MB
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionCheckFilterNormal]
        public async Task<IActionResult> Duzenle(int id, [Bind("Id,MusteriTc,MusteriAd,MusteriSoyad,Telefon,EPosta,MusteriDogTar,MusteriAdres,AracId,SubeId,ProfilFotografi,PP,MusteriSifre")] Musteri musteri, bool MusteriAktif)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }
            // Admin değilse ve düzenlemek istediği müşteri kendisi değilse hata döndür
            if (sessionId != 1 && sessionId != id)
            {
                return NotFound();
            }
            if (id != musteri.Id)
            {
                return NotFound();
            }
            var musteriBilgileri = await _context.Musteriler.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (musteriBilgileri == null)
            {
                return NotFound();
            }
            if (musteri.PP != null)
            {
                try
                {
                    if (musteri.ProfilFotografi == null || musteri.ProfilFotografi.Length == 0)
                    {
                        ModelState.AddModelError("Photo", "Lütfen yüklemek için bir fotoğraf seçin.");
                        return View(musteri);
                    }

                    var extension = Path.GetExtension(musteri.PP.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Photo", "Geçersiz dosya türü. Sadece JPG, JPEG, PNG ve GIF dosyalarına izin verilir.");
                        return View(musteri);
                    }

                    if (musteri.PP.Length > MaxFileSize)
                    {
                        ModelState.AddModelError("Photo", "Dosya boyutu çok büyük. Maksimum izin verilen boyut 2MB'dir.");
                        return View(musteri);
                    }

                    var dosyaAdi = id + musteri.PP.FileName;
                    var filePath = Path.Combine("wwwroot/assets/images/pps", dosyaAdi);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await musteri.PP.CopyToAsync(stream);
                    }

                    musteri.ProfilFotografi = dosyaAdi;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Dosya yükleme hatası: {ex.Message}");
                    return View(musteri);
                }
            }
            else
            {
                musteri.ProfilFotografi = musteriBilgileri.ProfilFotografi;
            }

            if (MusteriAktif != musteri.MusteriAktif && !MusteriAktif)
            {
                musteri.HesapPasifNedeni = "Admin Tarafından Silinen Hesap.";
            }
            musteri.MusteriAktif = MusteriAktif;
            musteri.MusteriSifre = musteriBilgileri.MusteriSifre;
            musteri.MusteriTc = musteriBilgileri.MusteriTc;
            musteri.AracId = musteriBilgileri.AracId;

            try
            {
                _context.Musteriler.Update(musteri);
                await _context.SaveChangesAsync();
                ViewBag.GuncelleDurum = "Müşteri kaydı başarıyla güncellendi";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusteriExists(musteri.Id))
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
                ViewBag.GuncelleDurum = "Müşteri kaydı GÜNCELLENEMEDİ!!";
                ViewBag.Ayni = "Aynı e-Posta ya da telefon numarası sistemde kayıtlı";
                return View(musteri);
            }

            if (sessionId == 1)
            {
                return RedirectToAction("List", "Musteri");
            }
            else
            {
                return RedirectToAction("Profilim", "Musteri");
            }
        }
        

        // GET: Musteris/Delete/5
        [SessionCheckFilterAdmin]
        public async Task<IActionResult> Delete(int? id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }

            // Admin değilse ve düzenlemek istediği müşteri kendisi değilse hata döndür
            if (sessionId != 1 && sessionId != id)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
        }

        // POST: Musteris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionCheckFilterAdmin]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }

            
            if (sessionId != 1 && sessionId != id)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler.FindAsync(id);
            if (musteri != null)
            {
                musteri.MusteriAktif = false;
                musteri.HesapPasifNedeni = "Admin tarafından pasif hale getirildi";
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
        public async Task<IActionResult> HesapSil1(int id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }


            if (sessionId != 1 && sessionId != id)
            {
                return NotFound();
            }

            var varmi = await _context.Musteriler.FindAsync(id);
            if (varmi != null)
                return View(varmi);
            else return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> HesapSil2(int id, string reason, string digerSebep)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }


            if (sessionId != 1 && sessionId != id)
            {
                return NotFound();
            }
            var varmi = await _context.Musteriler.FindAsync(id);
            if (varmi != null)
            {
                if (reason == "1") reason = "Hizmetlerden memnun değilim.";
                if (reason == "2") reason = "Daha iyi bir alternatif buldum.";
                if (reason == "3") reason = "Hesap güvenliği ile ilgili endişelerim var.";
                if (reason == "4") reason = "Fiyatlar çok yüksek.";
                if (reason == "5") reason = "Araçlar temiz ya da yeni değil.";
                if (digerSebep != null) reason += digerSebep;
                varmi.MusteriAktif = false;
                varmi.HesapPasifNedeni = reason;
                await _context.SaveChangesAsync();
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Index", "Home");
        }
        public bool TcKimlikNoDogrula(string tcKimlikNo)
        {
            // TC Kimlik No için regex kontrolü
            var regex = new Regex(@"^\d{11}$");
            if (!regex.IsMatch(tcKimlikNo))
                return false;

            // TC Kimlik No'nun ilk rakamı 0 olamaz
            if (tcKimlikNo[0] == '0')
                return false;

            // TC Kimlik No'nun rakamlarını al
            int[] digits = new int[11];
            for (int i = 0; i < 11; i++)
            {
                digits[i] = int.Parse(tcKimlikNo[i].ToString());
            }

            // İlk 10 rakamın toplamının birler basamağı, 11. rakama eşit olmalıdır
            int sumOfFirst10 = 0;
            for (int i = 0; i < 10; i++)
            {
                sumOfFirst10 += digits[i];
            }
            if (sumOfFirst10 % 10 != digits[10])
                return false;

            // 1., 3., 5., 7., ve 9. basamakların toplamının 7 katından,
            // 2., 4., 6., 8. basamakların toplamının çıkarılmasıyla elde edilen
            // sonucun birler basamağı 10. basamağa eşit olmalıdır
            int sum1 = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int sum2 = digits[1] + digits[3] + digits[5] + digits[7];
            int check10 = (sum1 * 7 - sum2) % 10;
            if (check10 != digits[9])
                return false;

            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> HesapOlustur(Musteri yeniMusteri, string sifreTekrar)
        {
            string pattern = @"^(\+90|0)?5\d{9}$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(yeniMusteri.Telefon)) ViewBag.Telefon = "Geçerli telefon numarası giriniz";
            bool tcDogru = TcKimlikNoDogrula(yeniMusteri.MusteriTc);
            if (tcDogru != true)
            {
                ViewBag.kayitDurum = "Geçerli bir Tc Kimlik numarası giriniz";
                return View("KayitOl", yeniMusteri);
            }
            if (ModelState.IsValid && sifreTekrar == yeniMusteri.MusteriSifre)
            {
                try
                {
                    _context.Musteriler.Add(yeniMusteri);
                    await _context.SaveChangesAsync();
                    ViewBag.kayitDurum = "Kayıt başarıyla eklendi. Giriş yapmak için yönlendirileceksiniz.";
                    return View("KayitTamam");
                }

                catch (DbUpdateException dbex)
                {
                    var exBilgi = dbex.InnerException.Message;
                    if (exBilgi.Contains("EPosta") == true)
                        ViewBag.kayitDurum = "Bu E-Posta daha önceden kaydedilmiş";
                    else if (exBilgi.Contains("MusteriTc") == true)
                        ViewBag.kayitDurum = "Bu TC no daha önceden kaydedilmiş";
                    else if (exBilgi.Contains("Telefon") == true)
                        ViewBag.kayitDurum = "Bu Telefon daha önceden kaydedilmiş";
                    return View("KayitOl", yeniMusteri);
                }
            }
            else
            {
                ViewBag.kayitDurum = "Şifreler eşleşmiyor";
                return View("KayitOl", yeniMusteri);
            }
        }
        public async Task<IActionResult> Giris(string EPosta, string MusteriSifre)
        {
            if (ModelState.IsValid)
            {
                var varmi = await _context.Musteriler.FirstOrDefaultAsync(x => x.EPosta == EPosta);

                if (varmi != null && varmi.MusteriAktif == true)
                {
                    if ((varmi.MusteriSifre == MusteriSifre) == true)
                    {
                        var girisGuncelle = new MusteriGiris()
                        {
                            MusteriId = varmi.Id,
                            GirisZamani = DateTime.Now,
                            CikisZamani = DateTime.Now.AddMinutes(30),

                        };
                        varmi.MusteriGirisId = _context.MusteriGirisleri.Max(x => x.Id) + 1;
                        await _context.MusteriGirisleri.AddAsync(girisGuncelle);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetInt32("Id", varmi.Id);
                        HttpContext.Session.SetString("ePosta", varmi.EPosta);
                        HttpContext.Session.SetString("Ad", varmi.MusteriAd);
                        HttpContext.Session.SetInt32("MusteriId", varmi.Id);
                        HttpContext.Session.SetString("E-Posta", varmi.EPosta);
                        HttpContext.Session.SetString("PP", varmi.ProfilFotografi ?? "resim_yok.jpg");
                        if (varmi.AdSoyad != null)
                            HttpContext.Session.SetString("Ad Soyad", varmi.AdSoyad);
                        //var jsonData = JsonSerializer.Serialize(varmi);
                        //HttpContext.Session.SetString("oturum", jsonData);
                        return RedirectToAction("KiralamaBaslangic", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [SessionCheckFilterNormal]
        public async Task<IActionResult> CikisYap()
        {
            var girisGuncelle = new MusteriGiris()
            {
                CikisZamani = DateTime.Now,
            };
            var cikisDurum = await _context.MusteriGirisleri.OrderBy(x => x.GirisZamani).LastOrDefaultAsync(x => x.MusteriId == HttpContext.Session.GetInt32("MusteriId"));
            if (cikisDurum != null)
            {

                cikisDurum.CikisZamani = DateTime.Now;
                _context.Update(cikisDurum);
                await _context.SaveChangesAsync();

                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [SessionCheckFilterNormal]
        public async Task<IActionResult> Profilim()
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }
            var aydi = HttpContext.Session.GetInt32("Id");
            var kullanici = await _context.Musteriler.FirstOrDefaultAsync(x => x.Id == aydi);
            if (kullanici != null)
            {
                var aracBilgi = await _context.Araclar.FirstOrDefaultAsync(x => x.Id == kullanici.AracId);
                if (aracBilgi != null)
                    ViewBag.SonArac = aracBilgi.AracMarka + " " + aracBilgi.AracModel;
            }
            return View(kullanici);
        }
        
        [SessionCheckFilterNormal]
        public IActionResult SifreDegistir()
        {
            return View();
        }
        [HttpPost]
        [SessionCheckFilterNormal]
        public async Task<IActionResult> SifreDegistir(string eskiSifre, string yeniSifre, string yeniSifreTekrar, int id)
        {
            var sessionId = HttpContext.Session.GetInt32("Id");
            if (sessionId == null)
            {
                return NotFound();
            }


            if (sessionId != 1 && sessionId != id)
            {
                return NotFound();
            }
            var sifreEslesme = yeniSifre == yeniSifreTekrar ? true : false;

            var kullanici = await _context.Musteriler.FindAsync(id);
            if (kullanici != null)
            {
                if (eskiSifre == kullanici.MusteriSifre && sifreEslesme == true)
                {
                    kullanici.MusteriSifre = yeniSifre;
                    await _context.SaveChangesAsync();
                    ViewBag.SifreDegistirme = "Şifre değiştirildi.";
                    return RedirectToAction("Profilim", "Musteri");
                }
            }
            else
            {
                return NotFound();
            }
            return View();
        }
        //public async Task<IActionResult> SifreDegistir()
        //{

        //}
        [SessionCheckFilterNormal]
        private bool MusteriExists(int id)
        {
            return _context.Musteriler.Any(e => e.Id == id);
        }
    }
}
