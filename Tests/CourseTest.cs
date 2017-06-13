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
    public void Dispose()
    {
      Course.DeleteAll();
    }
  }
}
