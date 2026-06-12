namespace Alchiweb-App1.Core;

public static partial class PageUrls
{
    public const string NotFound2 = "/notfound";

    public const string AdminUsers = "/admin/users";
    public const string AdminUsersProfile = "/admin/users/{Id:guid}";

    public const string AdminSchoolSettings = "/admin/school/settings";
    public const string AdminMembers = "/admin/members";
    public const string AdminMemberProfile = "/admin/members/{Id:guid}";
    public const string AdminSchoolGrades = "/admin/school/grades";
    public const string AdminSchoolSubjects = "/admin/school/subjects";
    public const string AdminSchoolYears = "/admin/school/years";

    public const string Students = "/all-students";
    public const string StudentProfile = "/students/{Id:guid}";
    public const string SchoolClasses = "/schoolclasses";
    public const string SchoolClassesDetails = "/schoolclasses/{Id:guid}";

    public const string MainDashboard = "/maindashboard";
    public const string AdminDashboard = "/admin/dashboard";
    public const string AlumniDashboard = "/alumni/dashboard";
    public const string ManagerDashboard = "/manager/dashboard";
    public const string TeacherDashboard = "/teacher/dashboard";

    public const string CursusDomains = "/cursus/domains";
    public const string CursusCycles = "/cursus/cycles";

    public const string DevTest = "/test";
    public const string DevTest2 = "/test2";

}
