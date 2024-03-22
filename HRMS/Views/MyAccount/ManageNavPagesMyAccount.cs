using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HRMS.Views.MyAccount
{
    public static class ManageNavPagesMyAccount
    {

        public static string ActivePageKey => "ActivePage";

        public static string MyAccountHome => "MyAccountHome";

        //public static string Employees => "Employees";

        //public static string Departments => "Departments";

        public static string Projects => "IndexProjects";
        public static string Leaves => "Leaves";
        //public static string Roles => "Roles";

        public static string MyAccountHomeNavClass(ViewContext viewContext) => PageNavClass(viewContext, MyAccountHome);

        //public static string EmployeesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Employees);

        //public static string DepartmentsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Departments);

        public static string ProjectsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Projects);
        public static string LeavesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Leaves);
        //public static string RolesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Roles);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;

    }
}
