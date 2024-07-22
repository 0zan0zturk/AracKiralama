namespace AracKiralama.Models
{
    public class KiralamaViewModel
    {
        public string? Filtre { get; set; }
        public List<KiralamaIslemi> KiralamaIslemleri { get; set; }
        public DateTime? BaslangicTarihi { get; set; } 
        public DateTime? BitisTarihi { get; set; } 
        
    }
}
