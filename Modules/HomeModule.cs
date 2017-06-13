using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("listDepartments", Department.GetAll());
        model.Add("listCourses", Course.GetAll());
        model.Add("listTeachers", Teacher.GetAll());
        model.Add("listStudents", Student.GetAll());
        model.Add("show-info", null);
        return View["index.cshtml", model];
      };
      Get["/departments/new"] = _ => {
        Dictionary<string, string> model = new Dictionary<string, string>{};
        model.Add("form-type", "new-department");
        return View["form.cshtml", model];
      };
      Post["/departments/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Department department = new Department(Request.Form["department-name"]);
        department.Save();
        model.Add("listDepartments", Department.GetAll());
        model.Add("listCourses", Course.GetAll());
        model.Add("listTeachers", Teacher.GetAll());
        model.Add("listStudents", Student.GetAll());
        model.Add("newDepartment", department);
        model.Add("show-info", "new-department-info");

        return View["index.cshtml", model];
      };
    }
  }
}
