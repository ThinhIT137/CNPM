using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Tourist_Area
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Img { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [ForeignKey("CreatedBy")]
        public virtual User? User { get; set; }
        public virtual ICollection<Hottel> Hottels { get; set; } = new List<Hottel>();
    }
}
