using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.University.Domain.Core;

[Table("STUDENTS")]
public partial class Student
{
    [Key]
    [Column("STUDENT_ID")]
    public int StudentId { get; set; }

    [Column("GROUP_ID")]
    public int? GroupId { get; set; }

    [Column("FIRST_NAME")]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Column("LAST_NAME")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Students")]
    public virtual Group? Group { get; set; }
}
