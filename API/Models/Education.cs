using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_educations")]
    public class Education : BaseEntity
    {
        [Column("major", TypeName = "nvarchar(100)")]
        public string Major { get; set; }

        [Column("degree", TypeName = "nvarchar(10)")]
        public string Degree { get; set; }

        [Column("gpa", TypeName = "real")]
        public double Gpa { get; set; }

        [Column("university_guid")]
        public Guid UniversityGuid { get; set; }

        // Cardinality
        [InverseProperty("Educations")]
        public University Universities { get; set; }
        public Employee? Employees { get; set; }
    }
}