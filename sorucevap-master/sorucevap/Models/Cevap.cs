using System.ComponentModel.DataAnnotations;

namespace sorucevap.Models
{
    public class Cevap
    {
        [Key]
        public int Id { get; set; }
        public string CevapIcerik { get; set; }
        public int soruId { get; set; }
        public virtual Soru soru { get; set; }
    }
}
