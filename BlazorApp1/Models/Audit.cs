using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models
{
    public class Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }    
        public string Description { get; set; }

        public string Type { get; set; }
        public int AccountID { get; set; }
        public Guid UserId { get; set; }

    }
}
