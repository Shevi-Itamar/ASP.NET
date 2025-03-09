using lessson1.Models;
using Newtonsoft.Json;
using lessson1.Interfaces;

namespace lessson1.Services;
public class ConnectToDataService : IDataService
{
    private readonly string userFilePath = "./Data/Users.json";
    private readonly string jewelFilePath = "./Data/Jewels.json";
    
    public List<User> ReadUsers()
    {
        if (!File.Exists(userFilePath))
        {
            return new List<User>();
        }
        string json = File.ReadAllText(userFilePath);
        var users = JsonConvert.DeserializeObject<List<User>>(json);
        return users ?? new List<User>();
    }
         
    public List<Jewel> ReadJewels()
    {
        if (!File.Exists(jewelFilePath))
        {
            return new List<Jewel>();
        }
        string json = File.ReadAllText(jewelFilePath);
        var jewels = JsonConvert.DeserializeObject<List<Jewel>>(json);
        return jewels ?? new List<Jewel>();
    }

    public void WriteUsers(List<User> users)
{
    string json = JsonConvert.SerializeObject(users, Formatting.Indented);
    File.WriteAllText(userFilePath, json);
    string fileContent = File.ReadAllText(userFilePath);
}
 

    public void WriteJewels(List<Jewel> jewels)
    {
        string json = JsonConvert.SerializeObject(jewels, Formatting.Indented);
        File.WriteAllText(jewelFilePath, json);
    }
}

public static class DataServiceHelper
{
    public static void AddDataService(this IServiceCollection BuilderService)
    {
        BuilderService.AddSingleton<IDataService, ConnectToDataService>();
    }
}
