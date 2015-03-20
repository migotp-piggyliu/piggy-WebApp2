namespace WebApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("app_company2")]
    public class AppCompany
    {
        public AppCompany()
        {
            //this.AppUser = new HashSet<AppUser>();
            this.CreateTime = DateTime.Now;
            this.LastupdateTime = DateTime.Now;
        }

        [Column("company_no", Order = 1), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [Column("company_id", Order = 2)]
        [MaxLength(128)]
        [MinLength(2)]
        [Required]
        public string ID { get; set; }

        [Column("company_name", Order = 3)]
        [MaxLength(128)]
        [MinLength(2)]
        [Required]
        public string Name { get; set; }

        [Column("email", Order = 4)]
        [MaxLength(128)]
        [MinLength(5)]
        [Required]
        public string Email { get; set; }

        [Column("contact", Order =5 )]
        [MaxLength(128)]
        public string Contact { get; set; }

        [Column("create_time", Order =6)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreateTime { get; set; }

        [Column("lastupdate_time", Order =7)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? LastupdateTime { get; set; }

        public virtual ICollection<AppUser> AppUser { get; set; }

        public override string ToString()
        {
            return (string.Format("AppCompany(No={0},ID={1},Name={2},Email={3},Contact={4},CreateTime={5},LastupdateTime={6})",
                                  this.No, this.ID, this.Name, this.Email, this.Contact,
                                  this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"), 
                                  (this.LastupdateTime == null) ? "" : ((DateTime)this.LastupdateTime).ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}
