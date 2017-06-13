using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.IO;

namespace Registrar
{
  public class Teacher
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public Teacher(string name, int id = 0)
    {
      Id = id;
      Name = name;
    }

    public override bool Equals(System.Object otherTeacher)
    {
      if (!(otherTeacher is Teacher))
      {
        return false;
      }
      else
      {
        Teacher newTeacher = (Teacher) otherTeacher;
        bool idEquality = this.Id == newTeacher.Id;
        bool nameEquality = this.Name == newTeacher.Name;
        return (idEquality && nameEquality);
      }
    }

    public static List<Teacher> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM teachers;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Teacher> teachers = new List<Teacher>{};
      while (rdr.Read())
      {
        int teacherId = rdr.GetInt32(0);
        string teacherName = rdr.GetString(1);
        Teacher newTeacher = new Teacher(teacherName, teacherId);
        teachers.Add(newTeacher);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return teachers;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM teachers;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
