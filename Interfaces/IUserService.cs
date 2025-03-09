using lessson1.Models;


namespace lessson1.Interfaces
{
    public interface IUserService
    {
        public User? Authenticate(string name, string password);
        List<User> GetAll();

        User? Get(int id);

        void Add(User newUser);

        void Delete(int id);

        void Update(User newUser,bool keepExistingCart = true);

        int Count { get;}
        
    }
}
