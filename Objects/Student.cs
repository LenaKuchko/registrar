using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.IO;

namespace Registrar
{
  public class Student
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public DateTime Enrollment { get; set; }

    public Student(string name, int year, DateTime enrollment, int id = 0)
    {
      Id = id;
      Name = name;
      Year = year;
      Enrollment = enrollment;
    }

    public static List<Student> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Student> students = new List<Student>{};
      while (rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        int studentYear = rdr.GetInt32(2);
        DateTime studentEnrollment = Convert.ToDateTime(rdr.GetString(3));
        Student newStudent = new Student(studentName, studentYear, studentEnrollment, studentId);
        students.Add(newStudent);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return students;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }



  }
}
