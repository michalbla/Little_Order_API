using TNAI.Model;

namespace TNAI.Repository
{
    public class BaseRepository
    {
        protected AppDbContext DbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
