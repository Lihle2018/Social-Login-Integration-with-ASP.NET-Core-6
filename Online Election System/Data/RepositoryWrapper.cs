namespace Online_Election_System.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IUserRepository _user;
        private AppDbContext _appDbContext;
        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    return new UserRepository(_appDbContext);
                }
                return _user;

            }
        }
        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
