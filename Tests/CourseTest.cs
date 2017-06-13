using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Registrar
{
  [Collection("Registrar")]
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_CourseDatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Course.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameInfo()
    {
      //Arrange, Act
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      Course firstCourse = new Course("Intro Econ", testTeacher.Id);
      Course secondCourse = new Course("Intro Econ", testTeacher.Id);
      //Assert
      Assert.Equal(firstCourse, secondCourse);
    }
    [Fact]
    public void Test_Save_SavesCourseToDatabase()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      Course testCourse = new Course("Intro Econ", testTeacher.Id);
      testCourse.Save();
      //Act
      List<Course> result = Course.GetAll();
      List<Course> expectedResult = new List<Course>{testCourse};
      //Assert
      Assert.Equal(result, expectedResult);
    }
    [Fact]
    public void Test_Save_AssignsIdToCourseInDatabase()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      Course testCourse = new Course("Intro Econ", testTeacher.Id);
      testCourse.Save();
      //Act
      Course savedCourse = Course.GetAll()[0];
      int testId = testCourse.Id;
      int expectedId = savedCourse.Id;
      //Assert
      Assert.Equal(testId, expectedId);
    }
    [Fact]
    public void Test_Find_FindsCourseInDatabase()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      Course testCourse = new Course("Intro Econ", testTeacher.Id);
      testCourse.Save();
      //Act
      Course foundCourse = Course.Find(testCourse.Id);
      //Assert
      Assert.Equal(testCourse, foundCourse);
    }
    [Fact]
    public void Test_UpdateCourse_ReturnsTrueIfCourseInfoIsTheSame()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      Course firstTestCourse = new Course("Intro Econ", testTeacher.Id);
      firstTestCourse.Save();
      Course secondTestCourse = new Course("Intro Women's Studies", 4, firstTestCourse.Id);
      //Act
      secondTestCourse.UpdateCourse("Intro Econ", testTeacher.Id);
      //Assert
      Assert.Equal(firstTestCourse, secondTestCourse);
    }
    [Fact]
    public void Test_Delete_ReturnsTrueIfListsAreTheSame()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      Course firstTestCourse = new Course("Intro Econ", testTeacher.Id);
      firstTestCourse.Save();
      Course secondTestCourse = new Course("Intro Women's Studies", testTeacher.Id);
      secondTestCourse.Save();
      Course thirdTestCourse = new Course("Intro Beyonce Studies", testTeacher.Id);
      thirdTestCourse.Save();
      List<Course> expectedList = new List<Course>{firstTestCourse, secondTestCourse};
      //Act
      thirdTestCourse.Delete();
      List<Course> resultList = Course.GetAll();
      //Assert
      Assert.Equal(resultList, expectedList);
    }

    [Fact]
    public void Test_AddStudent_AddStudentToCourse()
    {
      //Arrange
      Student newStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      newStudent.Save();
      Course testCourse = new Course("History", 1);
      testCourse.Save();
      //Act
      testCourse.AddStudent(newStudent);
      List<Student> testCourseStudents = testCourse.GetStudents();
      List<Student> expectedList = new List<Student>{newStudent};
      //Assert
      Assert.Equal(expectedList, testCourseStudents);
    }

    [Fact]
    public void Test_AddDepartment_AddDepartmentToCourse()
    {
      //Arrange
      Department newDepartment = new Department("History");
      newDepartment.Save();
      Course testCourse = new Course("Intro Econ", 1);
      testCourse.Save();
      //Act
      testCourse.AddDepartment(newDepartment);
      List<Department> testCourseDepartments = testCourse.GetDepartments();
      List<Department> expectedList = new List<Department>{newDepartment};
      //Assert
      Assert.Equal(expectedList, testCourseDepartments);
    }
    public void Dispose()
    {
      Course.DeleteAll();
      Teacher.DeleteAll();
      Student.DeleteAll();
    }
  }
}
