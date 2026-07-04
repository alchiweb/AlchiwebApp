namespace Alchiweb-App1.Client.Core.Components.Layout;

public partial class MainLayout
{
#if ALCHIWEBAPP
    [AutoInject] protected IStringLocalizer<Resources.ClientI18n> localizerAlchiweb-App1 = default!;
    [AutoInject] protected IStringLocalizer<I18n> localizerAlchiweb-App1Core = default!;
    private async Task SetNavPanelItemsBusiness(ClaimsPrincipal authUser, List<BitNavItem> navPanelItems)
    {
        // Alchiweb-App1

        if (authUser!.IsInRole(AppRoles.SuperAdmin))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.Admin_Dashboard,
                IconName = BitIconName.AutoRacing,
                Url = PageUrls.AdminDashboard,
            });
        }
        else if (authUser!.IsInRole(AppRoles.Teacher))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.menu_main_dashboard,
                IconName = BitIconName.AutoRacing,
                Url = PageUrls.TeacherDashboard,
            });
        }
        else if (authUser!.IsInRole(AppRoles.AlumniManager))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.Alumni_Dashboard,
                IconName = BitIconName.AutoRacing,
                Url = PageUrls.AlumniDashboard,
            });
        }
        else if (authUser!.IsInRole(AppRoles.PedagogicManager))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.menu_main_dashboard,
                IconName = BitIconName.AutoRacing,
                Url = PageUrls.ManagerDashboard,
            });
        }

        if (await authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageMembers))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.menu_main_members,
                IconName = BitIconName.ExternalUser,
                Url = PageUrls.AdminMembers,
            });
        }

        if (await authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageUsers))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.menu_main_users,
                IconName = BitIconName.UserWindow,
                Url = PageUrls.AdminUsers,
                IsEnabled = false,
            });
        }


        var (manageSchoolSettings, manageSchoolGrades, manageSchoolClasses, manageSchoolSubjects) = await(
            authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageSchoolSettings),
            authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageSchoolGrades),
            authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageSchoolClasses),
            authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageSchoolSubjects)
            );


        // Alchiweb-App1
        if (manageSchoolSettings || manageSchoolGrades || manageSchoolClasses || manageSchoolSubjects)
        {

            BitNavItem schoolItem = new()
            {
                Text = Resources.ClientI18n.menu_main_school,
                IconName = BitIconName.Home,
                ChildItems = []
            };

            navPanelItems.Add(schoolItem);

            if (manageSchoolSettings)
            {
                schoolItem.ChildItems.Add(new()
                {
                    Text = Resources.ClientI18n.menu_main_school_settings,
                    IconName = BitIconName.Settings,
                    Url = PageUrls.AdminSchoolSettings,
                });
            }

            if (manageSchoolGrades)
            {
                schoolItem.ChildItems.Add(new()
                {
                    Text = Resources.ClientI18n.menu_main_school_grades,
                    IconName = BitIconName.ViewList,
                    Url = PageUrls.AdminSchoolGrades,
                });
            }

            if (manageSchoolClasses)
            {
                schoolItem.ChildItems.Add(new()
                {
                    Text = Resources.ClientI18n.menu_main_school_schoolclasses,
                    IconName = BitIconName.Calendar,
                    Url = PageUrls.SchoolClasses,
                });
            }
            if (manageSchoolSubjects)
            {
                schoolItem.ChildItems.Add(new()
                {
                    Text = Resources.ClientI18n.menu_main_school_subjects,
                    IconName = BitIconName.DietPlanNotebook,
                    Url = PageUrls.AdminSchoolSubjects,
                });
            }
        }


        if (await authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageStudents))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.menu_main_students,
                IconName = BitIconName.People,
                Url = PageUrls.Students,
            });
        }
        if (await authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageDevTest))
        {
            navPanelItems.Add(new()
            {
                Text = Resources.ClientI18n.menu_main_test,
                IconName = BitIconName.TestUserSolid,
                Url = PageUrls.DevTest,
            });
        }

        var (manageCursusCycles, manageCursusDomains) = await(
            authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageCursusCycles),
            authorizationService.IsAuthorized(authUser!, AppFeatures.Business.ManageCursusDomains)
            );


        // Alchiweb-App1
        if (manageCursusCycles || manageCursusDomains)
        {

            BitNavItem cursusItem = new()
            {
                Text = Resources.ClientI18n.menu_main_cursus,
                IconName = BitIconName.Certificate,
                ChildItems = []
            };

            navPanelItems.Add(cursusItem);

            if (manageCursusCycles)
            {
                cursusItem.ChildItems.Add(new()
                {
                    Text = Resources.ClientI18n.menu_main_cursus_cycles,
                    IconName = BitIconName.BarChart3,
                    Url = PageUrls.CursusCycles,
                });
            }

            if (manageCursusDomains)
            {
                cursusItem.ChildItems.Add(new()
                {
                    Text = Resources.ClientI18n.menu_main_cursus_domains,
                    IconName = BitIconName.SslCertificate,
                    Url = PageUrls.CursusDomains,
                });
            }
        }
    }
#endif
}
