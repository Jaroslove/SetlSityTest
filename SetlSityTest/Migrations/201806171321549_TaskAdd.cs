namespace SetlSityTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserTasks",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Task_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Task_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.Task_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Task_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserTasks", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.ApplicationUserTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserTasks", new[] { "Task_Id" });
            DropIndex("dbo.ApplicationUserTasks", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserTasks");
            DropTable("dbo.Tasks");
        }
    }
}
