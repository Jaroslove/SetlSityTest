namespace SetlSityTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHistoryTab : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameTask = c.String(),
                        InputData = c.String(),
                        OutputData = c.String(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Histories");
        }
    }
}
