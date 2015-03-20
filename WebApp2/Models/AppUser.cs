namespace WebApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("app_user2")]
    public class AppUser
    {
        public AppUser()
        {
            this.CreateTime = DateTime.Now;
            this.LastupdateTime = DateTime.Now;
            this.AppUserRole = new HashSet<AppUserRole>();
        }

        [Column("user_no", Order=1), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [Column("company_no", Order = 2)]
        [Required]
        public int CompanyNo { get; set; }

        [Column("user_id", Order = 3)]
        [MaxLength(32)]
        [MinLength(2)]
        [Required]
        public string ID { get; set; }

        [Column("user_name", Order = 4)]
        [MaxLength(128)]
        [MinLength(2)]
        [Required]
        public string Name { get; set; }

        [Column("password", Order = 5)]
        [MaxLength(64)]
        [MinLength(8)]
        [Required]
        public string Password { get; set; }

        [Column("email", Order = 6)]
        [MaxLength(128)]
        [MinLength(5)]
        [Required]
        public string Email { get; set; }

        [Column("create_time", Order = 7)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreateTime { get; set; }

        [Column("lastupdate_time", Order = 8)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? LastupdateTime { get; set; }

        [ForeignKey("CompanyNo")]
        public virtual AppCompany AppCompany { get; set; }

        public virtual ICollection<AppUserRole> AppUserRole { get; set; }

        public override string ToString()
        {
            return (string.Format("AppUser(No={0},CompanyNo={1}[{10}/{9}],ID={2},Name={3},Password={4},Email={5},CreateTime={6},LastUpdateTime={8})",
                                  this.No, this.CompanyNo, this.ID, this.Name, this.Password, this.Email, 
                                  this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), 
                                  (this.LastupdateTime == null) ? "" : ((DateTime)this.LastupdateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                                  this.AppCompany.ID, this.AppCompany.Name));
        }

        //-----------------------------------------------------------------------------------------------------------
        public string GetUserRoleString()
        {
            string result = "";
            foreach (var item in this.AppUserRole)
            {
                 if (result != "") result += ",";
                result += item.AppRole.No.ToString();               
            }
            return (result);
        }

    }
}
