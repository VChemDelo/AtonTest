using System.ComponentModel.DataAnnotations;

namespace AtonTest.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Login { get; set; } = null;

        public string Password { get; set; } = null;

        public string Name { get; set; } = null;

        public int Gender { get; set; } = 2;

        public DateTime? Birthday { get; set; } = null;

        public bool Admin { get; set; } = false;

        public DateTime CreateOn { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = null!;

        public string ModifiedBy { get; set; } = string.Empty;

        public DateTime ModifiedOn { get; set; } = DateTime.MaxValue;

        public DateTime RevorkedOn { get; set; }   =  DateTime.MaxValue;

        public string RevorkedBy { get; set; } = string.Empty!;
    }
}
