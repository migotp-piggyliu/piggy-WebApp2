namespace WebApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("app_role_function2")]
    public class AppRoleFunction
    {
        public AppRoleFunction()
        {           
            this.CreateTime = DateTime.Now;
        }

        [Column("role_func_no", Order=1), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [Column("role_no", Order=2)]
        [Required]
        public int RoleNo { get; set; }

        [Column("func_no", Order=3)]
        [Required]
        public int FunctionNo { get; set; }

        [Column("create_time", Order = 4)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreateTime { get; set; }

        [ForeignKey("RoleNo")]
        public virtual AppRole AppRole { get; set; }

        [ForeignKey("FunctionNo")]
        public virtual AppFunction AppFunction { get; set; }


        public override string ToString()
        {
            return (string.Format("AppUserRole(No={0},RoleNo={1},FunctionNo={2},CreateTime={3})",
                                  this.No, this.RoleNo, this.FunctionNo, this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")));
        }

    }
}
