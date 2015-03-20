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
    public class AppUserAPI : BaseAPI
    {

        public AppUserAPI(DbContext db)
                     :base(db)
        {
        }

        //-------------------------------------------------------------------------------------------------------
        public AppUser Find(int no)
        {
            this.SetLastError(0, "");
            if (no > 0)
            {
                AppUser user = this.db.AppUsers.FirstOrDefault(u => u.No == no);
                return (user);
            }
            this.SetLastError(-1, "invalid params");
            return (null);
        }

        public AppUser Find(int company_no, string id)
        {           
            this.SetLastError(0, "");
            if (company_no > 0 && id != null && id != "")
            {
                AppUser user = this.db.AppUsers.FirstOrDefault(u => u.CompanyNo == company_no && u.ID == id);
                return (user);
            }
            this.SetLastError(-1, "invalid params");
            return (null);
        }

        //-------------------------------------------------------------------------------------------------------
        public List<AppUser> List(int? company_no = null)
        {         
            // 以下方法會 inner join AppComany left join AppUserRole, 但顯示群組時, repeat call AppRole by used role
            var q = from u in this.db.AppUsers.Include("AppCompany").Include("AppUserRole")
                    select u;

            // 以下方法會先載入 AppUserRoles, inner join AppCompany, 但顯示群組時, repeat call AppUserRole by user, repeat call AppRole by used role
            //this.db.AppUserRoles.Load();
            //var q = from u in this.db.AppUsers.Include("AppCompany").Include("AppUserRole")
            //        select u;

            if (company_no != null && company_no > 0)
            {
                q = q.Where(u => u.CompanyNo == company_no);
            }
            q = q.OrderBy(u => u.CompanyNo).ThenByDescending(u => u.Name);
            return (q.ToList());
        }

        //-------------------------------------------------------------------------------------------------------
        public AppUser IsNameExisted(int company_no, string name, int? no = null)
        {
            this.SetLastError(0, "");
            if (company_no > 0 && name != null && name != "")
            {
                AppUser user;
                if (no != null && no > 0)
                    user = this.db.AppUsers.FirstOrDefault(u => u.No != no && u.CompanyNo == company_no && u.Name == name);
                else
                    user = this.db.AppUsers.FirstOrDefault(u => u.CompanyNo == company_no && u.Name == name);
                return (user);
            }
            this.SetLastError(-1, "invalid params");
            return (null);
        }

        //-------------------------------------------------------------------------------------------------------
        public int Create(AppUser user, string[] selectedRole)
        {                   
            int user_no = -1;
            this.SetLastError(0, "");
                                     
            if (user == null ||
                user.ID == null || user.ID == "" || user.Name == null || user.Name == "" ||
                user.Password == null || user.Password == "" || user.Email == null || user.Email == "" || 
                selectedRole == null || selectedRole.Length <= 0)
            {
                this.SetLastError(-1, "invalid params");
                return (user_no);
            }

            AppUser orguser = Find(user.CompanyNo, user.ID);
            if (orguser != null)
            {
                this.SetLastError(-2, "user_id is existed");
                return (user_no);
            }
            orguser = IsNameExisted(user.CompanyNo, user.Name);
            if (orguser != null)
            {
                this.SetLastError(-3, "name is existed");
                return (user_no);
            }

            int roleNo;
            try
            {
                for (int i = 0; i < selectedRole.Length; i++)
                {
                    Int32.TryParse(selectedRole[i], out roleNo);
                    user.AppUserRole.Add(new AppUserRole { UserNo = user.No, RoleNo = roleNo });
                }
                this.db.AppUsers.Add(user);
                this.db.SaveChanges();
                user_no = user.No;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string error = "";
                foreach (var item in ex.EntityValidationErrors)
                {
                    foreach (var item2 in item.ValidationErrors)                       
                        error += item2.PropertyName + ":" + item2.ErrorMessage + "\r\n";
                }
                this.SetLastError(-4, "validate error " + error);
            }
            catch (Exception ex)
            {
                this.SetLastError(-5, "fail to update db " + ex.Message);
            }

            return (user_no);
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        public bool Edit(AppUser user, string[] selectedRole)
        {
            this.SetLastError(0, "");

            if (user == null ||
                user.Name == null || user.Name == "" ||
                user.Password == null || user.Password == "" || user.Email == null || user.Email == "" || 
                selectedRole == null || selectedRole.Length <= 0)
            {
                this.SetLastError(-1, "invalid params");
                return (false);
            }

            AppUser orguser = IsNameExisted(user.CompanyNo, user.Name, user.No);
            if (orguser != null)
            {
                this.SetLastError(-2, "name is not existed");
                return (false);               
            }
            user.LastupdateTime = DateTime.Now;


            // add & delete AppUserRole
            int       roleNo;
            string selectedData = "," + String.Join(",", selectedRole) + ",";
            string orgData = "," + user.GetUserRoleString() + ",";
            AppUserRole[] orgArray = user.AppUserRole.ToArray();              
            for(int i=orgArray.Length-1; i>=0; i--)
            {
                if (selectedData.IndexOf("," + orgArray[i].RoleNo.ToString() + ",") < 0)   // org is not existed
                {
                    //user.AppUserRole.Remove(orgArray[i]);    // 因為移除 OneToManyCascadeDeleteConvention, 所以需要將 db.AppUserRoles 資料刪除
                    this.db.AppUserRoles.Remove(orgArray[i]);
                }
            }
            for (int i = 0; i < selectedRole.Length; i++)
            {
                Int32.TryParse(selectedRole[i], out roleNo);
                if (orgData.IndexOf("," + roleNo.ToString() + ",") < 0)   // selected role  is not existed
                    user.AppUserRole.Add(new AppUserRole { RoleNo = roleNo, UserNo = user.No });
            }

            bool result = false;
            try
            {
                this.db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                this.db.SaveChanges();
                result = true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string error = "";
                foreach (var item in ex.EntityValidationErrors)
                {
                    foreach (var item2 in item.ValidationErrors)
                        error += item2.PropertyName + ":" + item2.ErrorMessage + "\r\n";
                }
                this.SetLastError(-3, "validate error " + error);
            }
            catch (Exception ex)
            {
                this.SetLastError(-4, "fail to update db " + ex.Message);
            }
            return (result);
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        public bool Delete(int no)
        {
            this.SetLastError(0, "");

            if (no <= 0)
            {
                this.SetLastError(-1, "invalid params");
                return (false);
            }

            AppUser user = this.Find(no);
            if (user == null)
            {
                this.SetLastError(-2, "user is not existed");
                return (false);
            }

            bool result = false;
            try
            {
                AppUserRole[] roleArray = user.AppUserRole.ToArray();
                for (int i = 0; i < roleArray.Length; i++ )
                    this.db.AppUserRoles.Remove(roleArray[i]);   
                db.AppUsers.Remove(user);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                this.SetLastError(-3, "fail  to update db " + ex.Message);
            }
            return (result);
        }
        
    }
}
