using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMWebAPI.Models
{
    public class CMApplicationRepository : ICMApplicationRepository
    {
        private CMContext _context;

        public CMApplicationRepository(CMContext dataContext)
        {
            _context = dataContext;
        }
        public IList<CMApplication> GetAll()
        {
            string query = @"
                SELECT
                App.CI_ID as CI_ID, App.DisplayName as Name,
                stuff((select '; '+ LC.CategoryInstanceName
                    from v_LocalizedCategories LC,
                    vAdminCategoryMemberships ACM
                    where ACM.CategoryInstanceID = LC.CategoryInstanceID AND App.CI_UniqueID = ACM.ObjectKey
                    for xml path(''), type).value('.', 'varchar(max)'), 1, 1, '') As 'AdminCategories'
                FROM
                fn_ListLatestApplicationCIs(1033) App
                ORDER BY App.DisplayName
            ";

            var apps = _context.CMApplication
                .FromSql(query);
            return apps.ToList();
        }
        
        public IList<CMApplication> GetAppOnDP(string id, string appName)
        {
            string query = @"
                DECLARE @AppName varchar(512)
                DECLARE @DPName varchar(512)

                SET @AppName = '##APPNAME##'
                SET @DPName = '##DPNAME##'

                Declare @Apps Table (CI_ID int,
                    Name varchar(512),
                    AdminCategories varchar(512));

                INSERT INTO @Apps
                SELECT
                    App.CI_ID as CI_ID, App.DisplayName as Name,
                    stuff((select '; '+ LC.CategoryInstanceName
                    from v_LocalizedCategories LC,
                        vAdminCategoryMemberships ACM
                    where ACM.CategoryInstanceID = LC.CategoryInstanceID AND App.CI_UniqueID = ACM.ObjectKey
                    for xml path(''), type).value('.', 'varchar(max)'), 1, 1, '') As 'AdminCategories'
                FROM
                    fn_ListLatestApplicationCIs(1033) App
                WHERE App.DisplayName LIKE '%' + @AppName+ '%' 
                ORDER BY App.DisplayName

                Select a.CI_ID, a.Name, a.AdminCategories
                from v_PackageStatusDistPointsSumm s 
                join smspackages p on s.packageid = p.pkgid 
                join @apps a on a.name = p.name
                where s.installstatus ='Package Installation complete' 
                and s.servernalpath like '%' + @DPName + '%' 
                AND p.name like '%' + @AppName+ '%' 
                order by s.packageid
            ";
            query = query.Replace("##APPNAME##", appName);
            query = query.Replace("##DPNAME##", id);
            var apps = _context.CMApplication
                .FromSql(query);
            return apps.ToList();
        }
        public IList<CMApplication> GetByName(string id)
        {
            string query = @"
                Declare @Apps Table (CI_ID int, Name varchar(512), AdminCategories varchar(512));
                INSERT INTO @Apps
                    SELECT
                    App.CI_ID as CI_ID, App.DisplayName as Name,
                    stuff((select '; '+ LC.CategoryInstanceName
                        from v_LocalizedCategories LC,
                        vAdminCategoryMemberships ACM
                        where ACM.CategoryInstanceID = LC.CategoryInstanceID AND App.CI_UniqueID = ACM.ObjectKey
                        for xml path(''), type).value('.', 'varchar(max)'), 1, 1, '') As 'AdminCategories'
                    FROM
                    fn_ListLatestApplicationCIs(1033) App
                    ORDER BY App.DisplayName
                Select CI_ID, Name, AdminCategories from @Apps where Name LIKE '%###%'
            ";
            query = query.Replace("###", id);
            var apps = _context.CMApplication
                .FromSql(query);
            return apps.ToList();
        }
    }
}
