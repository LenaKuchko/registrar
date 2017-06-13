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
