using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Hottel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Img { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public int TouristId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [ForeignKey("CreatedBy")]
        public virtual User? User { get; set; }
        [ForeignKey("TouristId")]
        public virtual Tourist_Area? Tourist_Area { get; set; }
    }
}
