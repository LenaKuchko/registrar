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
    public void Dispose()
    {
      Teacher.DeleteAll();
    }
  }
}
