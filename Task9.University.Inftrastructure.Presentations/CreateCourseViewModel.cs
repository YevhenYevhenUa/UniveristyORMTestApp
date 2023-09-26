using System.ComponentModel.DataAnnotations;

namespace Task9.University.Infrastructure.Presentations;
public class CreateCourseViewModel
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Description { get; set; }
}
