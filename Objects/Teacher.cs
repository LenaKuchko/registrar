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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO teachers (name) OUTPUT INSERTED.id VALUES (@TeacherName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@TeacherName";
      nameParameter.Value = this.Name;

      cmd.Parameters.Add(nameParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
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

    public void UpdateTeacher(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE teachers SET name = @NewName OUTPUT INSERTED.name WHERE id = @TeacherId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter teacherIdParameter = new SqlParameter();
      teacherIdParameter.ParameterName = "@TeacherId";
      teacherIdParameter.Value = this.Id;
      cmd.Parameters.Add(teacherIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Name = rdr.GetString(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }


    public static Teacher Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM teachers WHERE id = @TeacherId;", conn);
      SqlParameter teacherIdParameter = new SqlParameter();
      teacherIdParameter.ParameterName = "@TeacherId";
      teacherIdParameter.Value = id.ToString();
      cmd.Parameters.Add(teacherIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTeacherId = 0;
      string foundTeacherName = null;
      while(rdr.Read())
      {
        foundTeacherId = rdr.GetInt32(0);
        foundTeacherName = rdr.GetString(1);
      }
      Teacher foundTeacher = new Teacher(foundTeacherName, foundTeacherId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundTeacher;
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
