using lessson1.Models;
namespace lessson1.Interfaces;
public interface IDataService
{
    List<User> ReadUsers();
    List<Jewel>ReadJewels();
    void WriteUsers(List<User> users);
    void WriteJewels(List<Jewel> jewels);
}
