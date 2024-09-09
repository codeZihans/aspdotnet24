using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class UpdateGetProductsSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """ 
                        
                CREATE OR ALTER PROCEDURE GetProducts
                @PageIndex int,
                @PageSize int,
                @OrderBy nvarchar(50),
                @Title nvarchar(max) = '%',
                @PostDateFrom datetime = Null,
                @PostDateTo datetime = Null,
                @Body nvarchar(max) = '%',
                @CategoryId uniqueidentifier = Null,
                @Total int output,
                @TotalDisplay int output
            AS
            BEGIN
                SET NOCOUNT ON;
                Declare @sql nvarchar(max);
                Declare @countsql nvarchar(max);
                Declare @paramList nvarchar(max);
                Declare @countparamList nvarchar(max);

                -- Collecting Total
                Select @Total = count(*) from Products;

                -- Collecting Total Display
                SET @countsql = 'select @TotalDisplay = count(*) from Products p inner join
                                 Categories c on p.CategoryId = c.Id where 1 = 1';

                SET @countsql = @countsql + ' AND p.Title LIKE ''%'' + @xTitle + ''%''';
                SET @countsql = @countsql + ' AND p.Body LIKE ''%'' + @xBody + ''%''';

                IF @PostDateFrom IS NOT NULL
                    SET @countsql = @countsql + ' AND p.PostDate >= @xPostDateFrom';

                IF @PostDateTo IS NOT NULL
                    SET @countsql = @countsql + ' AND p.PostDate <= @xPostDateTo';

                IF @CategoryId IS NOT NULL
                    SET @countsql = @countsql + ' AND p.CategoryId = @xCategoryId';

                SELECT @countparamList = '@xTitle nvarchar(max),
                                          @xBody nvarchar(max),
                                          @xPostDateFrom datetime,
                                          @xPostDateTo datetime,
                                          @xCategoryId uniqueidentifier,
                                          @TotalDisplay int output';

                exec sp_executesql @countsql, @countparamList,
                                   @xTitle = @Title,
                                   @xBody = @Body,
                                   @xPostDateFrom = @PostDateFrom,
                                   @xPostDateTo = @PostDateTo,
                                   @xCategoryId = @CategoryId,
                                   @TotalDisplay = @TotalDisplay output;

                -- Collecting Data
                SET @sql = 'select p.Id,p.Title, p.Body, p.PostDate, c.Name as CategoryName from Products p inner join
                            Categories c on p.CategoryId = c.Id where 1 = 1';

                SET @sql = @sql + ' AND p.Title LIKE ''%'' + @xTitle + ''%''';
                SET @sql = @sql + ' AND p.Body LIKE ''%'' + @xBody + ''%''';

                IF @PostDateFrom IS NOT NULL
                    SET @sql = @sql + ' AND p.PostDate >= @xPostDateFrom';

                IF @PostDateTo IS NOT NULL
                    SET @sql = @sql + ' AND p.PostDate <= @xPostDateTo';

                IF @CategoryId IS NOT NULL
                    SET @sql = @sql + ' AND p.CategoryId = @xCategoryId';

                SET @sql = @sql + ' Order by ' + @OrderBy + ' OFFSET @PageSize * (@PageIndex - 1) 
                                   ROWS FETCH NEXT @PageSize ROWS ONLY';

                SELECT @paramList = '@xTitle nvarchar(max),
                                     @xBody nvarchar(max),
                                     @xPostDateFrom datetime,
                                     @xPostDateTo datetime,
                                     @xCategoryId uniqueidentifier,
                                     @PageIndex int,
                                     @PageSize int';

                exec sp_executesql @sql, @paramList,
                                   @xTitle = @Title,
                                   @xBody = @Body,
                                   @xPostDateFrom = @PostDateFrom,
                                   @xPostDateTo = @PostDateTo,
                                   @xCategoryId = @CategoryId,
                                   @PageIndex = @PageIndex,
                                   @PageSize = @PageSize;
            END
            GO

            """;
            migrationBuilder.Sql(sql);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.DropTable(sql);

        }
    }
}
