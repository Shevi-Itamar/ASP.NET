using lessson1.Models;
using lessson1.Interfaces;
namespace lessson1.Services
{
    public class UserService : IUserService
    {

        private readonly IDataService _IDataService;
        public UserService( IDataService IDataService){
            _IDataService= IDataService;
        }      

        public User? Authenticate(string name, string password)
        {  
            return _IDataService.ReadUsers().FirstOrDefault(u => u.Name == name && u.Password == password);
        }


        public List<User> GetAll() => _IDataService.ReadUsers();

        public User? Get(int id) => _IDataService.ReadUsers().FirstOrDefault(j => j.Id == id);

        public void Add(User newUser)
        {
            var usersList = _IDataService.ReadUsers();
            newUser.Id = usersList.Count() + 1;
            usersList.Add(newUser);
            _IDataService.WriteUsers(usersList);
        }

    public void Delete(int id)
{   
    var usersList = _IDataService.ReadUsers();
    var oldUser = usersList.FirstOrDefault(u => u.Id == id);
    if (oldUser is null)
        return;

    usersList.Remove(oldUser);
    _IDataService.WriteUsers(usersList);
    }


       public void Update(User newUser, bool keepExistingCart = true)
{
    var usersList = _IDataService.ReadUsers();
    var index = usersList.FindIndex(j => j.Id == newUser.Id);
    
    if (index == -1) return;

    if (keepExistingCart) 
    {
        newUser.Cart = usersList[index].Cart; 
    }

    usersList[index] = newUser;
    _IDataService.WriteUsers(usersList);
}


        public int Count { get => _IDataService.ReadUsers().Count(); }
    }


        public static class UserServiceHelper
        {
        public static void AddUserService(this IServiceCollection BuilderService)
        {
            BuilderService.AddSingleton<IUserService, UserService>();
        }
    }

}

