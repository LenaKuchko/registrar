using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar
{
  public class Student
  {
    public int Id { get; set; };
    public string Name { get; set; };
    public int Year { get; set; };
    public DateTime Enrollment { get; set; };

    public Student(string name, int year, DateTime enrollment, int id = 0)
    {
      Id = id;
      Name = name;
      Year = year;
      Enrollment = enrollment;
    }



  }
}
