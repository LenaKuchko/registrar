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

    public List<Course> GetCourses()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE teacher_id = @TeacherId;", conn);

     SqlParameter teacherIdParameter = new SqlParameter();
     teacherIdParameter.ParameterName = "@TeacherId";
     teacherIdParameter.Value = this.Id;
     cmd.Parameters.Add(teacherIdParameter);
     SqlDataReader rdr = cmd.ExecuteReader();

     List<Course> courses = new List<Course> {};
     while(rdr.Read())
     {
       int courseId = rdr.GetInt32(0);
       string courseName = rdr.GetString(1);
       int courseTeacherId = rdr.GetInt32(2);
       Course newCourse = new Course(courseName, courseTeacherId, courseId);
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

   public void AddStudent(Student student)
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO students_teachers (student_id, teacher_id) VALUES (@StudentId, @TeacherId);", conn);

     SqlParameter teacherIdParameter = new SqlParameter();
     teacherIdParameter.ParameterName = "@TeacherId";
     teacherIdParameter.Value = this.Id;
     cmd.Parameters.Add(teacherIdParameter);

     SqlParameter studentIdParameter = new SqlParameter();
     studentIdParameter.ParameterName = "@StudentId";
     studentIdParameter.Value = student.Id;
     cmd.Parameters.Add(studentIdParameter);

     cmd.ExecuteNonQuery();
     if (conn != null)
     {
       conn.Close();
     }
   }

   public List<Student> GetStudents()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("SELECT students.* FROM teachers JOIN students_teachers ON (teachers.id = students_teachers.teacher_id) JOIN students ON (students_teachers.student_id = students.id)  WHERE teachers.id = @TeacherId;", conn);

     SqlParameter teacherIdParameter = new SqlParameter();
     teacherIdParameter.ParameterName = "@TeacherId";
     teacherIdParameter.Value = this.Id;

     cmd.Parameters.Add(teacherIdParameter);
     SqlDataReader rdr = cmd.ExecuteReader();

     List<Student> students = new List<Student>{};

     while(rdr.Read())
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

   public void AddDepartment(Department department)
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO departments_teachers (department_id, teacher_id) VALUES (@DepartmentId, @TeacherId);", conn);

     SqlParameter departmentIdParameter = new SqlParameter();
     departmentIdParameter.ParameterName = "@DepartmentId";
     departmentIdParameter.Value = department.Id;
     cmd.Parameters.Add(departmentIdParameter);

     SqlParameter teacherIdParameter = new SqlParameter();
     teacherIdParameter.ParameterName = "@TeacherId";
     teacherIdParameter.Value = this.Id;
     cmd.Parameters.Add(teacherIdParameter);

     cmd.ExecuteNonQuery();
     if (conn != null)
     {
       conn.Close();
     }
   }

   public List<Department> GetDepartments()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("SELECT departments.* FROM teachers JOIN departments_teachers ON (teachers.id = departments_teachers.teacher_id) JOIN departments ON (departments_teachers.department_id = departments.id)  WHERE teachers.id = @TeacherId;", conn);

     SqlParameter teacherIdParameter = new SqlParameter();
     teacherIdParameter.ParameterName = "@TeacherId";
     teacherIdParameter.Value = this.Id;

     cmd.Parameters.Add(teacherIdParameter);
     SqlDataReader rdr = cmd.ExecuteReader();

     List<Department> departments = new List<Department>{};

     while(rdr.Read())
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

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM teachers WHERE id = @TeacherId;", conn);

      SqlParameter teacherIdParameter = new SqlParameter();
      teacherIdParameter.ParameterName = "@TeacherId";
      teacherIdParameter.Value = this.Id;

      cmd.Parameters.Add(teacherIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM teachers;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
