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
    [Fact]
    public void Test_UpdateTeacher_ReturnsTrueIfTeacherInfoIsTheSame()
    {
      //Arrange
      Teacher firstTestTeacher = new Teacher("Lina Shadrach");
      firstTestTeacher.Save();
      Teacher secondTestTeacher = new Teacher("Ms. Frizz", firstTestTeacher.Id);
      //Act
      secondTestTeacher.UpdateTeacher("Lina Shadrach");
      //Assert
      Assert.Equal(firstTestTeacher, secondTestTeacher);
    }
    [Fact]
    public void Test_Delete_ReturnsTrueIfListsAreTheSame()
    {
      //Arrange
      Teacher firstTestTeacher = new Teacher("Lina Shadrach");
      firstTestTeacher.Save();
      Teacher secondTestTeacher = new Teacher("Ms. Frizz");
      secondTestTeacher.Save();
      Teacher thirdTestTeacher = new Teacher("Severus Snape");
      thirdTestTeacher.Save();
      List<Teacher> expectedList = new List<Teacher>{firstTestTeacher, secondTestTeacher};
      //Act
      thirdTestTeacher.Delete();
      List<Teacher> resultList = Teacher.GetAll();
      //Assert
      Assert.Equal(resultList, expectedList);
    }
    public void Dispose()
    {
      Teacher.DeleteAll();
    }
  }
}
