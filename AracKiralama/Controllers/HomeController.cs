using AracKiralama.Helpers;
using AracKiralama.Models;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace AracKiralama.Controllers
{
    
    public class HomeController : Controller
    {
        DataContext _context = new DataContext();
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                return RedirectToAction("KiralamaBaslangic");
            }
            else
            {
                return View();
            }

        }
        [SessionCheckFilterNormal]
        public async Task<IActionResult> KiralamaBaslangic()
        {
            if (HttpContext.Session.GetString("E-Posta") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Sehir = await _context.Sehirler.ToListAsync();
            //var mydate = new MyDate() { DateStart = "2024-01-01" };
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> GirisDurum()
        {
            ViewBag.GirisBilgilendirme = "Giriþ baþarýlý";
            ViewBag.Girismi = HttpContext.Session.GetString("oturum");
            return View();
        }
        public IActionResult MailGonder()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MailGonder(string? Ad, string? Soyad, string? Telefon, string? ePosta, string? Baslik, string? Mesaj)
        {
            string desen = @"\b(?:https?|ftp):\/\/[\w-]+(\.[\w-]+)+\S*";
            if (Regex.IsMatch(Ad, desen) || Regex.IsMatch(Soyad, desen) || Regex.IsMatch(Telefon, desen) || Regex.IsMatch(ePosta, desen) || Regex.IsMatch(Mesaj, desen))
            {
                ViewBag.DesenKontrolSonucu = "Link barýndýran içerikler gönderilemez!";
            }
            else
            {
                MailMessage mailim = new MailMessage();
                mailim.To.Add("ozturkmaildeneme@gmail.com");
                mailim.From = new MailAddress("ozturkmaildeneme@gmail.com");
                mailim.Subject = "Mesaj Konusu : " + Baslik;
                mailim.Body = Ad + " " + Soyad + " " + ePosta + " " + Telefon + " bilgilerine sahip kiþi tarafýndan gönderilen içerik <br>" + Mesaj;
                mailim.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("ozturkmaildeneme@gmail.com", "brvz dtgz hxuy okqb");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mailim);
                    ViewBag.DesenKontrolSonucu = "Mesaj Baþarýyla Gönderildi";
                }
                catch (Exception ex)
                {
                    ViewBag.DesenKontrolSonucu = "Mesaj Gönderilemedi. Hata nedeni " + ex.Message; //@TempData["Message"] olarak okunabilir.
                }

            }
            return View();
        }
    }
}
