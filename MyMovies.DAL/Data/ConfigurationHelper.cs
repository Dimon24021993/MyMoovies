using MyMovies.DAL.SeedData;

namespace MyMovies.DAL.Data
{
    public class ConfigurationHelper
    {
        public ConfigurationHelper(DataBaseContext context)
        {
            Context = context;
        }

        public DataBaseContext Context { get; set; }

        public void SeedConfiguration()
        {
            ApplicationUserSeed.SeedApplicationUsers(Context);



            DataBaseSeed.DataBaseSeedMethod(Context);

        }
    }
}