using System.ComponentModel.DataAnnotations;

namespace AracKiralama.Models
{
    public class KiralamaIslemi
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Kiralama Başlangıç Tarihi alanı boş bırakılamaz")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime KiralamaBaslangicTarihi { get; set; }
        [Required(ErrorMessage = "Kiralama Başlangıç Saati alanı boş bırakılamaz")]
        public int KiralamaBaslangicSaati { get; set; }
        [Required(ErrorMessage = "Kiralama Bitiş Tarihi alanı boş bırakılamaz")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime KiralamaBitisTarihi { get; set; }
        [Required(ErrorMessage = "Kiralama Başlangıç Saati alanı boş bırakılamaz")]
        public int KiralamaBitisSaati { get; set; }
        public int MusteriId { get; set; }
        public Musteri? Musteri { get; set; }
        public int AracId { get; set; }
        public Arac? Arac { get; set; }
        public int? SubeId { get; set; }
        public Sube? Sube { get; set; }
        public int SehirId{ get; set; }
        public Sehir? Sehir { get; set; }
        public DateTime IslemZamani { get; set; } = DateTime.Now;
        public int KiralananGunSayisi { get; set; }
        public double KiralamaBedeli { get; set; }
    }
}

