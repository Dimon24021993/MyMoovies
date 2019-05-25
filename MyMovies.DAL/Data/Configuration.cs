using System.Data.Entity.Migrations;
using System.Dynamic;

namespace MyMovies.DAL.Data
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataBaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new DataBaseSqlServerMigrationSqlGenerator());
            SetHistoryContextFactory("System.Data.SqlClient", (conn, schema) => new SqlHistoryContext(conn, schema));
            CodeGenerator = new SqlMigrationCodeGenerator();
        }

        protected override void Seed(DataBaseContext context)
        {
            new ConfigurationHelper(context).SeedConfiguration();
        }
    }
}