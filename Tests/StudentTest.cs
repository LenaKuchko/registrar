using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Registrar
{
  [Collection("Registrar")]
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_StudentDatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Student.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_SavesStudentToDatabase()
    {
      //Arrange
      Student testStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      testStudent.Save();
      //Act
      List<Student> result = Student.GetAll();
      List<Student> expectedResult = new List<Student>{testStudent};
      //Assert
      Assert.Equal(result, expectedResult);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameInfo()
    {
      //Arrange, Act
      Student firstStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      Student secondStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Test_Save_AssignsIdToStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      testStudent.Save();
      //Act
      Student savedStudent = Student.GetAll()[0];
      int testId = testStudent.Id;
      int expectedId = savedStudent.Id;
      //Assert
      Assert.Equal(testId, expectedId);
    }

    [Fact]
    public void Test_Find_FindsStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      testStudent.Save();
      //Act
      Student foundStudent = Student.Find(testStudent.Id);
      //Assert
      Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void Test_UpdateStudent_ReturnsTrueIfStudentInfoIsTheSame()
    {
      //Arrange
      Student firstTestStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      firstTestStudent.Save();
      Student secondTestStudent = new Student("Nick", 2, new DateTime(2017, 6, 9), firstTestStudent.Id);
      //Act
      secondTestStudent.UpdateStudent(1, "Jared");
      //Assert
      Assert.Equal(firstTestStudent, secondTestStudent);
    }

    [Fact]
    public void Test_Delete_ReturnsTrueIfListsAreTheSame()
    {
      //Arrange
      Student firstTestStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      firstTestStudent.Save();
      Student secondTestStudent = new Student("Nick", 2, new DateTime(2017, 6, 9));
      secondTestStudent.Save();
      Student thirdTestStudent = new Student("Joshua", 3, new DateTime(2017, 6, 9));
      thirdTestStudent.Save();
      List<Student> expectedList = new List<Student>{firstTestStudent, secondTestStudent};
      //Act
      thirdTestStudent.Delete();
      List<Student> resultList = Student.GetAll();
      //Assert
      Assert.Equal(resultList, expectedList);
    }

    [Fact]
    public void Test_AddCourse_AddCourseToStudent()
    {
      //Arrange
      Course newCourse = new Course("History", 1);
      newCourse.Save();
      Student testStudent = new Student("Jared", 1, new DateTime(2017, 6, 9));
      testStudent.Save();
      //Act
      testStudent.AddCourse(newCourse);
      List<Course> testStudentCourses = testStudent.GetCourses();
      List<Course> expectedList = new List<Course>{newCourse};
      //Assert
      Assert.Equal(expectedList, testStudentCourses);
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
