using System.ComponentModel.DataAnnotations;

namespace Task9.University.Infrastructure.Presentations;
public class CreateStudentViewModel
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    public int? GroupId { get; set; }
}
