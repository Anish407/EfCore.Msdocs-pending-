namespace EfCore.Infra.Database;

public class Student
{
    public string FirstName { get; set; }
    public int Id { get; set; }

    public int StudentDetailId { get; set; }
    public StudentDetails StudentDetails { get; set; }
}