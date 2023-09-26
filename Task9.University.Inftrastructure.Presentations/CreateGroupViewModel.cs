using System.ComponentModel.DataAnnotations;

namespace Task9.University.Infrastructure.Presentations;
public class CreateGroupViewModel
{
    [Required]
    public string? Name { get; set; }
    public int? CourseId { get; set; }
}
