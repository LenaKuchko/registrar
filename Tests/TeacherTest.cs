using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Registrar
{
  [Collection("Registrar")]
  public class TeacherTest : IDisposable
  {
    public TeacherTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_TeacherDatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Teacher.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueForSameInfo()
    {
      //Arrange, Act
      Teacher firstTeacher = new Teacher("Lina Shadrach");
      Teacher secondTeacher = new Teacher("Lina Shadrach");
      //Assert
      Assert.Equal(firstTeacher, secondTeacher);
    }
    [Fact]
    public void Test_Save_SavesTeacherToDatabase()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      //Act
      List<Teacher> result = Teacher.GetAll();
      List<Teacher> expectedResult = new List<Teacher>{testTeacher};
      //Assert
      Assert.Equal(result, expectedResult);
    }
    [Fact]
    public void Test_Save_AssignsIdToTeacherInDatabase()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      //Act
      Teacher savedTeacher = Teacher.GetAll()[0];
      int testId = testTeacher.Id;
      int expectedId = savedTeacher.Id;
      //Assert
      Assert.Equal(testId, expectedId);
    }
    [Fact]
    public void Test_Find_FindsTeacherInDatabase()
    {
      //Arrange
      Teacher testTeacher = new Teacher("Lina Shadrach");
      testTeacher.Save();
      //Act
      Teacher foundTeacher = Teacher.Find(testTeacher.Id);
      //Assert
      Assert.Equal(testTeacher, foundTeacher);
    }
    public void Dispose()
    {
      Teacher.DeleteAll();
    }
  }
}
