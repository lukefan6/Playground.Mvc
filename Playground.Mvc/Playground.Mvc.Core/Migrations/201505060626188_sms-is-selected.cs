namespace Playground.Mvc.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smsisselected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sms", "IsSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sms", "IsSelected");
        }
    }
}
