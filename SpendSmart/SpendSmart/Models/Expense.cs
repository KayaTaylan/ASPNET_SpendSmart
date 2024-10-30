using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        [Required]
        public string? Description { get; set; } //? means that we can  now have a NULL Value for this property
    }
}
