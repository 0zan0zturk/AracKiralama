namespace AracKiralama.Models
{
    public class MusteriGiris
    {
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public DateTime GirisZamani { get; set; } = DateTime.Now;
        public DateTime CikisZamani { get; set; } = DateTime.Now;
    }
}
