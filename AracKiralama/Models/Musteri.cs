using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AracKiralama.Models
{

    public class Musteri
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen 'Şifre' giriniz")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Şifre en az 8, en fazla 30 karakter uzunluğunda olmalıdır.")]
        public string MusteriSifre { get; set; }
        [Required(ErrorMessage = "Lütfen 'TC No' giriniz")]
        [Display(Name = "TC Kimlik No")]
        public string MusteriTc { get; set; }
        [Required(ErrorMessage = "Lütfen 'İsim' giriniz")]
        [Display(Name = "Ad")]
        public string MusteriAd { get; set; }
        [Required(ErrorMessage = "Lütfen 'Soyisim' giriniz")]
        [Display(Name = "Soyad")]
        public string MusteriSoyad { get; set; }
        public string? AdSoyad { get { return this.MusteriAd + " " + MusteriSoyad; } }
        [Required(ErrorMessage = "Lütfen 'Telefon' giriniz")]
        [Display(Name = "Telefon")]
        [MinLength(10, ErrorMessage = "Lütfen geçerli telefon numarası giriniz")]
        [MaxLength(14, ErrorMessage = "Lütfen geçerli telefon numarası giriniz")]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Lütfen 'E-Posta' giriniz")]
        [Display(Name = "E-Posta")]
        public string EPosta { get; set; }
        [Required(ErrorMessage = "Lütfen 'Doğum Tarihi' giriniz")]
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly MusteriDogTar { get; set; }
        [Required(ErrorMessage = "Lütfen 'Adres' giriniz")]
        [Display(Name = "Adres")]
        public string MusteriAdres { get; set; }
        public int? AracId { get; set; }
        public Arac? SonArac { get; set; }
        public int? SubeId { get; set; }
        public bool MusteriAktif { get; set; } = true;
        [Display(Name = "Profil Fotoğrafı")]
        public string? ProfilFotografi { get; set; } = "resim_yok.jpg";
        [NotMapped]
        public IFormFile? PP { get; set; }
        public int MusteriGirisId { get; set; }
        public MusteriGiris? GirisBilgisi { get; set; }
        public string HesapPasifNedeni { get; set; } = "yok";



    }
}
