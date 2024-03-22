using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HRMS.Views.Admin
{
    public static class ManageNavPagesAdmin
    {
        public static string ActivePageKey => "ActivePage";

        public static string AdminDashboard => "AdminDashboard";

        public static string Employees => "Employees";

        public static string Departments => "Departments";

        public static string Projects => "IndexProjects";
        public static string Leaves => "Leaves";
        public static string Roles => "Roles";
        public static string SkillTypes => "SkillTypes";
        //public static string Classes => "Classes";
        //public static string Seasons => "Seasons";
        //public static string CostCodes => "CostCodes";
        //public static string Sizes => "Sizes";
        //public static string DefAccount => "DefAccount";
        //public static string AppConfig => "AppConfig";
        //public static string Reports => "Reports";

        public static string AdminDashboardNavClass(ViewContext viewContext) => PageNavClass(viewContext, AdminDashboard);

        public static string EmployeesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Employees);

        public static string DepartmentsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Departments);

        public static string ProjectsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Projects);
        public static string LeavesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Leaves);
        public static string RolesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Roles);
        public static string SkillTypeNavClass(ViewContext viewContext) => PageNavClass(viewContext, SkillTypes);
        //public static string ClassesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Classes);
        //public static string SeasonsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Seasons);
        //public static string CostCodesNavClass(ViewContext viewContext) => PageNavClass(viewContext, CostCodes);
        //public static string SizesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Sizes);
        //public static string DefAccountNavClass(ViewContext viewContext) => PageNavClass(viewContext, DefAccount);
        //public static string AppConfigNavClass(ViewContext viewContext) => PageNavClass(viewContext, AppConfig);
        //public static string ReportsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Reports);



        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
