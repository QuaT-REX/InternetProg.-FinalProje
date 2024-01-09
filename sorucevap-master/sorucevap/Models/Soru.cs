using System.ComponentModel.DataAnnotations;

namespace sorucevap.Models
{
    public class Soru
    {
        [Key]
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public bool Durum { get; set; }
        public int userId { get; set; }
        public string SoruSoranBilgi { get; set; }

    }
}
