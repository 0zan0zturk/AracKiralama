namespace AracKiralama.Models
{
    public class Sube
    {
        public int Id { get; set; }
        public string? SubeAdi { get; set; }
        public string? SubeAdresi { get; set; }
        //public List<Arac>? SubedekiAraclar { get; set; }
        public int? SehirId { get; set; }
        public Sehir? Sehir { get; set; }

    }
}
