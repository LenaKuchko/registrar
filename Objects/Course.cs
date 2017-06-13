using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.IO;

namespace Registrar
{
  public class Course
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public Course(string name, int id = 0)
    {
      Id = id;
      Name = name;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.Id == newCourse.Id;
        bool nameEquality = this.Name == newCourse.Name;
        return (idEquality && nameEquality);
      }
    }

    public static List<Course> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Course> courses = new List<Course>{};
      while (rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        Course newCourse = new Course(courseName, courseId);
        courses.Add(newCourse);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return courses;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
