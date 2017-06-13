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
    public void Dispose()
    {
      Student.DeleteAll();
    }
  }
}
