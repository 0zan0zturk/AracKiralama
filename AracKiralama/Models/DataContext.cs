using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Reflection.Metadata;

namespace AracKiralama.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Musteri> Musteriler => Set<Musteri>();
        public DbSet<Arac> Araclar => Set<Arac>();
        public DbSet<KiralamaIslemi> KiralamaIslemleri => Set<KiralamaIslemi>();
        public DbSet<Sube> Subeler => Set<Sube>();
        public DbSet<Sehir> Sehirler => Set<Sehir>();
        public DbSet<MusteriGiris> MusteriGirisleri => Set<MusteriGiris>();
        public DbSet<Sanziman> Sanzimanlar => Set<Sanziman>();
        public DbSet<Yakit> Yakitlar => Set<Yakit>();   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Musteri>()
                .HasIndex(m => m.MusteriTc)
                .IsUnique();
            modelBuilder.Entity<Musteri>()
                .HasIndex(m => m.Telefon)
                .IsUnique();
            modelBuilder.Entity<Musteri>()
                .HasIndex(m => m.EPosta)
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=ABRA-A5\\SQLEXPRESS; Database=AracKiralama;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //            QueryTrackingBehavior.NoTracking
            //NoTracking: Bu ayar, sorgulardan dönen varlıkların DbContext tarafından izlenMEMEsini sağlar. Yani, AsNoTracking() metodunun etkisini tüm sorgulara uygular.Bu, performansı artırabilir çünkü Entity Framework Core, izlenen varlıkları takip etmek için ekstra bellek ve işlem gücü harcamaz.
        }


    }
}
