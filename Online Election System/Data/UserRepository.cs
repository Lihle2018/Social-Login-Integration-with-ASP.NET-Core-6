namespace Online_Election_System.Data
{
    public class UserRepository:RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
