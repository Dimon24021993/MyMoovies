Add-Migration <migration_name>
Update-Database
-----dataBase first-----
scaffold-DbContext "Server=.;Database=MyMovies;trusted_connection=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities