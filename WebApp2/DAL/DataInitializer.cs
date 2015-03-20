
namespace WebApp2
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using NLog;

    public class DataInitializer : DropCreateDatabaseIfModelChanges<AppDBContext>
    {
        //  CreateDatabaseIfNotExists
        // DropCreateDatabaseIfModelChanges
        // DropCreateDatabaseAlways

        private readonly Logger logger = null;

        public DataInitializer()
            : base()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        protected override void Seed(AppDBContext db)
        {
            base.Seed(db);
            DoInit(db);
        }

        public void DoInit(AppDBContext db)
        {
            this.logger.Info("Initialize database...");

            // Add Companies
            //if (db.AppCompanies.FirstOrDefault() != null)
            {
                List<AppCompany> compList = new List<AppCompany> { 
                                                                         new AppCompany { ID = "PPPP", Name = "PPPP公司", Email = "piggy_liu@migosoft.com", Contact = "PPPP 管理員" },
                                                                         new AppCompany { ID = "KKKK", Name = "KKKK公司", Email = "kency_huang@migosoft.com", Contact = "KKKK 管理員" },
                                                                         new AppCompany { ID = "XXXX", Name = "XXXX公司", Email = "XXXX@migosoft.com", Contact = "XXXX 管理員" }};
                compList.ForEach(c => db.AppCompanies.Add(c));
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.logger.Error("fail to add Companies..." + ex.Message);
                }
            }

            AppCompany comp;
            AppRole role;
            AppFunction func;
            AppUser user;

            // Add Roles
            //if (db.AppRoles.FirstOrDefault() != null)
            {
                comp = db.AppCompanies.FirstOrDefault(c => c.ID == "PPPP");
                List<AppRole> roleList = new List<AppRole> { 
                                                                         new AppRole { ID = "Admin", Name="Administrator", SupportCompanyNo = 0 },
                                                                         new AppRole { ID = "Users", Name="Users", SupportCompanyNo = 0 },
                                                                         new AppRole { ID = "SysTemp1", Name="System Template1", SupportCompanyNo = -1 },
                                                                         new AppRole { ID = "PPP_Sys", Name="PPPP公司-系統功能", SupportCompanyNo = 0 },                                                      
                                                                         new AppRole { ID = "PPP_Data", Name="PPPP公司-資料功能", SupportCompanyNo = 0 },
                                                                         new AppRole { ID = "PPP_Report", Name="PPPP公司-報表功能", SupportCompanyNo = 0 }};
                roleList.ForEach(r => db.AppRoles.Add(r));
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.logger.Error("fail to add Roles..." + ex.Message);
                }
            }

            // Add Functions
            //if (db.AppFunctions.FirstOrDefault() != null)
            {
                List<AppFunction> funcList = new List<AppFunction> { 
                                                                         new AppFunction { ID = "sys", Name="系統", Path="/sys/index.html", ParentNo=0, Level=1 },
                                                                         new AppFunction { ID = "data", Name="資料", Path="/data/index.html", ParentNo=0, Level=1 },
                                                                         new AppFunction { ID = "report", Name="報表", Path="/report/index.html", ParentNo=0, Level=1 }};
                funcList.ForEach(f => db.AppFunctions.Add(f));
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.logger.Error("fail to add Functions..." + ex.Message);
                }
            }

            // Add Users
            //if (db.AppUsers.FirstOrDefault() != null)
            {
                comp = db.AppCompanies.FirstOrDefault(c => c.ID == "PPPP");
                if (comp != null)
                {
                    for (int i = 1; i <= 11; i++)
                    {
                        user = new AppUser
                        {
                            CompanyNo = comp.No,
                            ID = "piggy" + i.ToString(),
                            Name = "piggy" + i.ToString() + "名稱",
                            Password = "12345678",
                            Email = "piggy" + i.ToString() + "@migosoft.com"
                        };
                        db.AppUsers.Add(user);
                    }
                }

                comp = db.AppCompanies.FirstOrDefault(c => c.ID == "KKKK");
                if (comp != null)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        user = new AppUser
                        {
                            CompanyNo = comp.No,
                            ID = "kency" + i.ToString(),
                            Name = "kency" + i.ToString() + "名稱",
                            Password = "12345678",
                            Email = "kency" + i.ToString() + "@migosoft.com"
                        };
                        db.AppUsers.Add(user);
                    }
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.logger.Error("fail to add Users..." + ex.Message);
                }
            }

            // Add UserRole
            //if (db.AppUserRoles.FirstOrDefault() != null)
            {
                user = db.AppUsers.FirstOrDefault(u => u.ID == "piggy1");
                role = db.AppRoles.FirstOrDefault(r => r.ID == "Admin");
                if (user != null && role != null)
                {
                    db.AppUserRoles.Add(new AppUserRole { RoleNo = role.No, UserNo = user.No });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error("fail to add User Roles..." + ex.Message);
                    }
                }
            }

            // Add RoleFunction
            role = db.AppRoles.FirstOrDefault(r => r.ID == "Admin");
            func = db.AppFunctions.FirstOrDefault(f => f.ID == "sys");
            if (role != null && func != null)
            {               
                role.AppFunction.Add(func);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.logger.Error("fail to add Role Functions..." + ex.Message);
                }
            }
            

        }

    }
}
