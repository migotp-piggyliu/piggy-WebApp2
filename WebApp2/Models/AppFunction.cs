namespace WebApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("app_function2")]
    public class AppFunction
    {
        public AppFunction()
        {           
            this.CreateTime = DateTime.Now;
            this.AppRole = new HashSet<AppRole>();
        }

        [Column("func_no", Order=1), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [Column("func_id", Order = 2)]
        [MaxLength(64)]
        [MinLength(2)]
        [Required]
        public string ID { get; set; }

        [Column("func_name", Order = 3)]
        [MaxLength(128)]
        [MinLength(2)]
        [Required]
        public string Name { get; set; }

        [Column("path", Order = 4)]
        [MaxLength(128)]
        [MinLength(2)]
        public string Path { get; set; }

        [Column("func_type", Order = 5)]
        public int Type { get; set; }

        [Column("func_parent_no", Order = 6)]
        public int ParentNo { get; set; }

        [Column("func_level", Order = 7)]
        public int Level { get; set; }

        [Column("create_time", Order = 8)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreateTime { get; set; }

        public virtual ICollection<AppRole> AppRole { get; set; }

        public override string ToString()
        {            
            return (string.Format("AppFunction(No={0},ID={1},Name={2},Path={3},Type={4},ParentNo={5},Level={6},CreateTime={7})",
                                  this.No, this.ID, this.Name, this.Path, this.Type, this.ParentNo, this.Level,
                                  this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")));
        }       

    }
}
