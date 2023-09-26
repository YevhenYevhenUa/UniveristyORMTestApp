using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.University.Domain.Core;

[Table("GROUPS")]
public partial class Group
{
    [Key]
    [Column("GROUP_ID")]
    public int GroupId { get; set; }

    [Column("COURSE_ID")]
    public int? CourseId { get; set; }

    [Column("NAME")]
    [StringLength(100)]
    public string? Name { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Groups")]
    public virtual Course? Course { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
