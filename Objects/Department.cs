using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.IO;

namespace Registrar
{
  public class Department
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public Department(string name, int id = 0)
    {
      Id = id;
      Name = name;
    }

    public override bool Equals(System.Object otherDepartment)
    {
      if (!(otherDepartment is Department))
      {
        return false;
      }
      else
      {
        Department newDepartment = (Department) otherDepartment;
        bool idEquality = this.Id == newDepartment.Id;
        bool nameEquality = this.Name == newDepartment.Name;
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departments (name) OUTPUT INSERTED.id VALUES (@DepartmentName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@DepartmentName";
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

    public static List<Department> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Department> departments = new List<Department>{};
      while (rdr.Read())
      {
        int departmentId = rdr.GetInt32(0);
        string departmentName = rdr.GetString(1);
        Department newDepartment = new Department(departmentName, departmentId);
        departments.Add(newDepartment);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return departments;
    }

    public void UpdateDepartment(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE departments SET name = @NewName OUTPUT INSERTED.name WHERE id = @DepartmentId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.Id;
      cmd.Parameters.Add(departmentIdParameter);
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


    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
