using API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

[Table("tb_m_university")]
public class University : BaseEntity
{
    [Column("code", TypeName = "nvarchar(50)")]
    public string Code { get; set; }

    [Column("name", TypeName = "nvarchar(100)")]
    public string? Name { get; set; }

    // Cardinality
    [InverseProperty("Universities")]
    public ICollection<Education> Educations { get; set; }
}
