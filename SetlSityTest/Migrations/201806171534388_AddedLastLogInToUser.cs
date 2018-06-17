namespace SetlSityTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLastLogInToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastLogIn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastLogIn");
        }
    }
}
