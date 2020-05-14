using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.Data.Migrations
{
	public partial class AddSprocForSubtreeExtraction : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				CREATE OR ALTER PROCEDURE dbo.GetWorkItemsSubtree
					@WorkItemId int  
				AS
					SET NOCOUNT ON;

					WITH WorkItemsHierarchy
					(
						Id,
						Title,
						[Description],
						Executors,
						[Status],
						ActualExecutionTime,
						PlannedExecutionTime,
						EndedAt,
						ParentId,
						CreatedAt
					) AS
					(
						SELECT
							Id,
							Title,
							[Description],
							Executors,
							[Status],
							ActualExecutionTime,
							PlannedExecutionTime,
							EndedAt,
							ParentId,
							CreatedAt
						FROM WorkItems WHERE Id = @WorkItemId
						UNION ALL
						SELECT
							wi.Id,
							wi.Title,
							wi.[Description],
							wi.Executors,
							wi.[Status],
							wi.ActualExecutionTime,
							wi.PlannedExecutionTime,
							wi.EndedAt,
							wi.ParentId,
							wi.CreatedAt
						FROM WorkItems AS wi
						INNER JOIN WorkItemsHierarchy AS h
						ON h.Id = wi.ParentId
					)

					SELECT *
					FROM WorkItemsHierarchy;
				GO
            ");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DROP PROCEDURE dbo.GetWorkItemsSubtree");
		}
	}
}
