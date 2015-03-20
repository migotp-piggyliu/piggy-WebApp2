using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

namespace WebApp2
{
    public class AppRoleAPI : BaseAPI
    {

        public AppRoleAPI(DbContext db)
                     :base(db)
        {
        }

        //-------------------------------------------------------------------------------------------------------
        public List<AppRole> List(int company_no)
        {         
            List<AppRole> roleList;
            if (company_no > 0)
                roleList = this.db.AppRoles.Where(r => r.SupportCompanyNo == 0 || r.SupportCompanyNo == company_no).ToList();
            else
                roleList = this.db.AppRoles.Where(r => r.SupportCompanyNo >= 0).ToList();

            return (roleList);
        }
        
    }
}
