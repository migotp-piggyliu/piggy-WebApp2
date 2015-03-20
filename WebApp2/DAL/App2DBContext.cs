namespace WebApp2
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class AppDBContext : DbContext
    {
        // Your context has been configured to use a 'App1DBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ConsoleApp1.App1DBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'App1DBContext' 
        // connection string in the application configuration file.
        public AppDBContext()
            : base("name=App2DBContext")
        {
            // this.Configuration.LazyLoadingEnabled = false;                    
            Database.SetInitializer<AppDBContext>(new DataInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 資料表名稱設定為實體類型名稱之複數化版本的慣例。
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //用於對任何必要關聯性啟用串聯刪除的慣例。
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // 偵測主索引鍵屬性的慣例。 辨識的命名模式的優先順序是：1. ' Id' 2. [type name]Id 主索引鍵的偵測不區分大小寫。 
            //modelBuilder.Conventions.Remove<IdKeyDiscoveryConvention>();
            // 用來探索名稱為相依導覽屬性名稱與主體類型主索引鍵屬性名稱組合之外部索引鍵屬性的慣例。
            //modelBuilder.Conventions.Remove<NavigationPropertyNameForeignKeyDiscoveryConvention>();
            //用來探索名稱符合主體類型主索引鍵屬性名稱之外部索引鍵屬性的慣例。
            //modelBuilder.Conventions.Remove<PrimaryKeyNameForeignKeyDiscoveryConvention>();

            modelBuilder.Entity<AppRole>()
                                     .HasMany(c => c.AppFunction).WithMany(f => f.AppRole)
                                     .Map(t => t.MapLeftKey("role_no")
                                     .MapRightKey("func_no")
                                     .ToTable("app_role_function2"));

        } 

        public DbSet<AppCompany> AppCompanies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppFunction> AppFunctions { get; set; }

        public DbSet<AppUserRole> AppUserRoles { get; set; }
        //public DbSet<AppRoleFunction> AppRoleFunctions { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}