namespace EfCore.Infra.Database;

public class StudentDetails
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public string Email { get; set; }

    public Student Student { get; set; }
}