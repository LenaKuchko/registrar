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

    public static Department Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId;", conn);
      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(departmentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundDepartmentId = 0;
      string foundDepartmentName = null;
      while(rdr.Read())
      {
        foundDepartmentId = rdr.GetInt32(0);
        foundDepartmentName = rdr.GetString(1);
      }
      Department foundDepartment = new Department(foundDepartmentName, foundDepartmentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundDepartment;
    }

    public void AddCourse(Course course)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses_departments (course_id, department_id) VALUES (@CourseId, @DepartmentId);", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.Id;
      cmd.Parameters.Add(departmentIdParameter);

      SqlParameter courseIdParameter = new SqlParameter();
      courseIdParameter.ParameterName = "@CourseId";
      courseIdParameter.Value = course.Id;
      cmd.Parameters.Add(courseIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM departments JOIN courses_departments ON (departments.id = courses_departments.department_id) JOIN courses ON (courses_departments.course_id = courses.id)  WHERE departments.id = @DepartmentId;", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.Id;

      cmd.Parameters.Add(departmentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Course> courses = new List<Course>{};

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        int teacherId = rdr.GetInt32(2);

        Course newCourse = new Course(courseName, teacherId, courseId);
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

    public void AddTeacher(Teacher teacher)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departments_teachers (department_id, teacher_id) VALUES (@DepartmentId, @TeacherId);", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.Id;
      cmd.Parameters.Add(departmentIdParameter);

      SqlParameter teacherIdParameter = new SqlParameter();
      teacherIdParameter.ParameterName = "@TeacherId";
      teacherIdParameter.Value = teacher.Id;
      cmd.Parameters.Add(teacherIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Teacher> GetTeachers()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT teachers.* FROM departments JOIN departments_teachers ON (departments.id = departments_teachers.department_id) JOIN teachers ON (departments_teachers.teacher_id = teachers.id)  WHERE departments.id = @DepartmentId;", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.Id;

      cmd.Parameters.Add(departmentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Teacher> teachers = new List<Teacher>{};

      while(rdr.Read())
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

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM departments WHERE id = @DepartmentId;", conn);

      SqlParameter departmentIdParameter = new SqlParameter();
      departmentIdParameter.ParameterName = "@DepartmentId";
      departmentIdParameter.Value = this.Id;

      cmd.Parameters.Add(departmentIdParameter);
      cmd.ExecuteNonQuery();

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
