using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace AracKiralama.Models
{
    public class Arac
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Araç Plakası Gerekli!")]
        [Display(Name = "Araç Plakası")]
        [RegularExpression("^(0[1-9]|[1-7][0-9]|8[01])[A-Z a-z]{1,3}\\d{2,5}$", ErrorMessage = "Geçersiz araç plakası.")]
        public string? AracPlaka { get; set; }
        [Required(ErrorMessage = "Araç Markası Gerekli!")]
        [Display(Name = "Markası")]
        public string? AracMarka { get; set; }
        [Required(ErrorMessage = "Araç Modeli Gerekli!")]
        [Display(Name = "Modeli")]
        public string? AracModel { get; set; }
        [Required(ErrorMessage = "Araç Yılı Gerekli!")]
        [Display(Name = "Araç Yılı")]
        [Range(2000, int.MaxValue, ErrorMessage = "Geçersiz yıl değeri.")]
        public string? AracYil { get; set; }
        [Required(ErrorMessage = "Araç Yakıt Türü Gerekli!")]
        [Display(Name = "Yakıt")]
        public string? AracYakit { get; set; }
        [Required(ErrorMessage = "Araç Şanzıman Tipi Gerekli!")]
        [Display(Name = "Şanzıman")]
        public string? AracSanziman { get; set; }
        public bool AracMusait { get; set; }
        public int? MusteriId { get; set; }
        public int? SubeId { get; set; }
        [Required(ErrorMessage = "Araç Motor Hacmi Gerekli!")]
        [Display(Name = "Motor Hacmi")]
        public int? MotorHacmi { get; set; }
        [Required(ErrorMessage = "Araç Fotoğrafı Gerekli!")]
        [Display(Name = "Fotoğraf")]
        public string? AracFotograf { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        [Required(ErrorMessage = "Araç Günlük Fiyatı Gerekli!")]
        [Display(Name = "Günlük Fiyat")]
        public float GunlukFiyat { get; set; }
        public int KiralamaIslemiId { get; set; }
        public ICollection<KiralamaIslemi>? KiralamaIslemleri { get; set; }
        public bool aracSil { get; set; } = true;
    }
}
