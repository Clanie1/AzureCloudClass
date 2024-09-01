namespace WebApplication1.entities;

using System.ComponentModel.DataAnnotations;
public class Student
{
    [Key]
    public int Student_id { get; set; }
    public string Fullname { get; set; }
    
}