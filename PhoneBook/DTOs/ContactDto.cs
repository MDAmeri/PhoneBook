using System.ComponentModel.DataAnnotations;

namespace PhoneBook.DTOs
{
    public class ContactDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        [MaxLength(20)]
        public string? HomeNumber { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}