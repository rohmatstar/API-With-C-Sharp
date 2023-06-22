using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_employees")]
    public class Employee : BaseEntity
    {
        [Column("nik", TypeName = "nchar(6)")]
        public string Nik { get; set; }
        [Column("first_name", TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }
        [Column("last_name", TypeName = "nvarchar(100)")]
        public string? LastName { get; set; }
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }
        [Column("gender")]
        public GenderEnum Gender { get; set; }
        [Column("hiring_date") ]
        public DateTime HiringDate { get; set; }
        [Column("email", TypeName = "nvarchar(50)")]
        public string Email { get; set; }
        [Column("phone_number", TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; }
        public Account? Accounts { get; set; }
        public Education? Educations { get; set; }
        public Booking? Bookings { get; set; }
        public ICollection<Account> Account { get; set; }
        public ICollection<Booking> Booking { get; set; }
        public Employee? Employees { get; set; }
    }
}