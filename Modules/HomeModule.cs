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
        // model.Add("show-info", null);
        return View["index.cshtml", model];
      };
    }
  }
}
