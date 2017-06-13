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

    public void Dispose()
    {
      Department.DeleteAll();
    }
  }
}
