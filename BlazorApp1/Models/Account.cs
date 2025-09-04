using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int OrderBy { get; set; }
        
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }

    }
}
