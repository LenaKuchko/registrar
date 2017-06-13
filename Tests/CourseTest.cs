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
      Course firstCourse = new Course("Intro Econ");
      Course secondCourse = new Course("Intro Econ");
      //Assert
      Assert.Equal(firstCourse, secondCourse);
    }
    [Fact]
    public void Test_Save_SavesCourseToDatabase()
    {
      //Arrange
      Course testCourse = new Course("Intro Econ");
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
      Course testCourse = new Course("Intro Econ");
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
      Course testCourse = new Course("Intro Econ");
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
      Course firstTestCourse = new Course("Intro Econ");
      firstTestCourse.Save();
      Course secondTestCourse = new Course("Intro Women's Studies", firstTestCourse.Id);
      //Act
      secondTestCourse.UpdateCourse("Intro Econ");
      //Assert
      Assert.Equal(firstTestCourse, secondTestCourse);
    }
    [Fact]
    public void Test_Delete_ReturnsTrueIfListsAreTheSame()
    {
      //Arrange
      Course firstTestCourse = new Course("Intro Econ");
      firstTestCourse.Save();
      Course secondTestCourse = new Course("Intro Women's Studies");
      secondTestCourse.Save();
      Course thirdTestCourse = new Course("Intro Beyonce Studies");
      thirdTestCourse.Save();
      List<Course> expectedList = new List<Course>{firstTestCourse, secondTestCourse};
      //Act
      thirdTestCourse.Delete();
      List<Course> resultList = Course.GetAll();
      //Assert
      Assert.Equal(resultList, expectedList);
    }
    public void Dispose()
    {
      Course.DeleteAll();
    }
  }
}
