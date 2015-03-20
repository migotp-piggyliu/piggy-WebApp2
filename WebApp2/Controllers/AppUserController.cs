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
    public class AppUserController : Controller
    {
        protected AppDBContext db;
        protected AppUserAPI userAPI;

        public AppUserController() : base()
        {
            this.db = new AppDBContext();
            this.userAPI = new AppUserAPI(this.db);
        }

        // GET
        public ActionResult Index(int? CompanyNo)
        {
            this.xInitCompanyDropDownList(CompanyNo);
            return View(this.userAPI.List(CompanyNo));
        }

        //---------------------------------------------------------------------------------------------------
        // GET: User/Details/5
        public ActionResult Details(int? no)
        {
            if (no == null || no <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = this.userAPI.Find((int)no);
            if (user == null)
            {
                return HttpNotFound();
            }

            string result = "";
            foreach (var item in user.AppUserRole)
            {
                foreach (var func in item.AppRole.AppFunction)
                {
                    result += string.Format("RoleNo={0}, RoleID={1}, FuncNo={2}, FuncID={3}\r\n",
                                                                  item.AppRole.No, item.AppRole.ID, func.No, func.ID);
                }
            }
            return View(user);
        }

        //---------------------------------------------------------------------------------------------------
        public ActionResult Create()
        {
            this.xInitCompanyDropDownList();
            this.xInitRoleCheckBoxs();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyNo,ID,Name,Password,Email")]AppUser user, string[] selectedRole)
        {
            bool bOK = true;
            if (user.CompanyNo <= 0)
            {
                bOK = false;
                ModelState.AddModelError("CompanyNo", "CompanyNo is not existed");
            }
            if (user.ID == null || user.ID == "")
            {
                bOK = false;
                ModelState.AddModelError("ID", "ID can not be empty");
            }
            if (user.Name == null || user.Name == "")
            {
                bOK = false;
                ModelState.AddModelError("Name", "Name can not be empty");
            }
            if (user.Email == null || user.Email == "")
            {
                bOK = false;
                ModelState.AddModelError("Email", "Name can not be empty");
            }
            if (selectedRole == null || selectedRole.Length <= 0)
            {
                bOK = false;
                ModelState.AddModelError("AppUserRole", "Role can not be empty");
            }

            if (bOK == true)
            {
                int user_no = this.userAPI.Create(user, selectedRole);
                if (user_no > 0)
                {
                    return RedirectToAction("Index");
                }

                bOK = false;
                int error_code = this.userAPI.LastErrorCode;
                if (error_code == -2)
                    ModelState.AddModelError("ID", "ID is existed");
                else if (error_code == -3)
                    ModelState.AddModelError("Name", "Name is existed");
                else
                    ModelState.AddModelError("", this.userAPI.LastError);
            }

            this.xInitCompanyDropDownList(user.CompanyNo);    
            this.xInitRoleCheckBoxs(user.CompanyNo, ((selectedRole != null) ? (String.Join(",", selectedRole)) : ""));
            return View(user);
        }

        //---------------------------------------------------------------------------------------------------
        public ActionResult Edit(int? no)
        {
            if (no == null || no <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = this.userAPI.Find((int)no);
            if (user == null)
            {
                return HttpNotFound();
            }

            this.xInitCompanyDropDownList(user.CompanyNo);
            this.xInitRoleCheckBoxs(user.CompanyNo, user.GetUserRoleString());
            return View(user);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? no, string[] selectedRole)
        {
            if (no == null || no <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = this.userAPI.Find((int)no);
            if (user == null)
            {
                return HttpNotFound();
            }

            bool bOK = true;
            if (TryUpdateModel(user, "", new string[] { "Name", "Password", "Email" }) == false)
            {
                bOK = false;
                ModelState.AddModelError("", "fail to update model");
            }
            if (user.Name == null || user.Name == "")
            {
                bOK = false;
                ModelState.AddModelError("Name", "Name can not be empty");
            }
            if (user.Email == null || user.Email == "")
            {
                bOK = false;
                ModelState.AddModelError("Email", "Name can not be empty");
            }
            if (selectedRole == null || selectedRole.Length <= 0)
            {
                bOK = false;
                ModelState.AddModelError("AppUserRole", "Role can not be empty");
            }

            if (bOK == true)
            {
                if (this.userAPI.Edit(user, selectedRole) == true)
                {
                    return RedirectToAction("Index");
                }

                bOK = false;
                int error_code = this.userAPI.LastErrorCode;
                if (error_code == -2)
                    ModelState.AddModelError("Name", "Name is existed");
                else
                    ModelState.AddModelError("", this.userAPI.LastError);
            }

            this.xInitCompanyDropDownList(user.CompanyNo);
            this.xInitRoleCheckBoxs(user.CompanyNo, ((selectedRole != null) ? (String.Join(",", selectedRole)) : ""));
            return View(user);
        }

        //---------------------------------------------------------------------------------------------------
        // GET: User/Delete/5
        public ActionResult Delete(int? no)
        {
            if (no == null || no <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = this.userAPI.Find((int)no);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int no)
        {
            if (no <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser user = this.userAPI.Find((int)no);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (this.userAPI.Delete(no) == true)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", this.userAPI.LastError);
            return View(user);
        }

        //---------------------------------------------------------------------------------------------------
        private void xInitCompanyDropDownList(int? company_no = 0)
        {           
            AppCompanyAPI compAPI = new AppCompanyAPI(this.db);         
             ViewBag.CompanyNo = new SelectList(compAPI.List(), "No", "Name", company_no);
        }

        private void xInitRoleCheckBoxs(int company_no=0, string selectedData=null)
        {
            string selectedData2 = selectedData;
            if (selectedData2 != null && selectedData2 != "")
                selectedData2 = "," + selectedData2 + ",";

           AppRoleAPI roleAPI = new AppRoleAPI(this.db);
           List<AppRole> allList = roleAPI.List(company_no);
            var roleList = new List<AssignedUserRole>();
            foreach (var role1 in allList)
            {
                AssignedUserRole role = new AssignedUserRole
                {
                    No = role1.No,
                    Name = role1.Name,
                    Assigned = false
                };
                roleList.Add(role);

                if (selectedData2 != null && selectedData2 != "")
                {
                    if (selectedData2.IndexOf("," + role.No.ToString() + ",") >= 0)
                        role.Assigned = true;
                }
            }
            ViewBag.AppUserRole = roleList;
        }

    }
}
