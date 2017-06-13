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
    
    public void Dispose()
    {
      Student.DeleteAll();
    }
  }
}
