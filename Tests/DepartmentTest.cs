using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Registrar
{
  [Collection("Registrar")]
  public class DepartmentTest : IDisposable
  {
    public DepartmentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_DepartmentDatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Department.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameInfo()
    {
      //Arrange, Act
      Department firstDepartment = new Department("History");
      Department secondDepartment = new Department("History");
      //Assert
      Assert.Equal(firstDepartment, secondDepartment);
    }

    [Fact]
    public void Test_Save_AssignsIdToDepartmentInDatabase()
    {
      //Arrange
      Department testDepartment = new Department("History");
      testDepartment.Save();
      //Act
      Department savedDepartment = Department.GetAll()[0];
      int testId = testDepartment.Id;
      int expectedId = savedDepartment.Id;
      //Assert
      Assert.Equal(testId, expectedId);
    }

    [Fact]
    public void Test_UpdateDepartment_ReturnsTrueIfDepartmentInfoIsTheSame()
    {
      //Arrange
      Department firstTestDepartment = new Department("History");
      firstTestDepartment.Save();
      Department secondTestDepartment = new Department("Classics", firstTestDepartment.Id);
      //Act
      secondTestDepartment.UpdateDepartment("History");
      //Assert
      Assert.Equal(firstTestDepartment, secondTestDepartment);
    }

    [Fact]
    public void Test_Find_FindsDepartmentInDatabase()
    {
      //Arrange
      Department testDepartment = new Department("History");
      testDepartment.Save();
      //Act
      Department foundDepartment = Department.Find(testDepartment.Id);
      //Assert
      Assert.Equal(testDepartment, foundDepartment);
    }

    [Fact]
    public void Test_Delete_ReturnsTrueIfListsAreTheSame()
    {
      //Arrange
      Department firstTestDepartment = new Department("History");
      firstTestDepartment.Save();
      Department secondTestDepartment = new Department("Classics");
      secondTestDepartment.Save();
      Department thirdTestDepartment = new Department("Biology");
      thirdTestDepartment.Save();
      List<Department> expectedList = new List<Department>{firstTestDepartment, secondTestDepartment};
      //Act
      thirdTestDepartment.Delete();
      List<Department> resultList = Department.GetAll();
      //Assert
      Assert.Equal(resultList, expectedList);
    }

    [Fact]
    public void Test_AddCourse_AddCourseToDepartment()
    {
      //Arrange
      Course newCourse = new Course("Intro Econ", 1);
      newCourse.Save();
      Department testDepartment = new Department("History");
      testDepartment.Save();
      //Act
      testDepartment.AddCourse(newCourse);
      List<Course> testDepartmentCourses = testDepartment.GetCourses();
      List<Course> expectedList = new List<Course>{newCourse};
      //Assert
      Assert.Equal(expectedList, testDepartmentCourses);
    }

    public void Dispose()
    {
      Department.DeleteAll();
      Course.DeleteAll();
    }
  }
}
