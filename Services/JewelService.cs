using lessson1.Models;
using lessson1.Interfaces;

namespace lessson1.Services
{
    public class JewelService : IJewelService
    {   private readonly IDataService _IDataService;
        public JewelService( IDataService IDataService){
            _IDataService= IDataService;
        }      

         public List<Jewel> GetAll() => _IDataService.ReadJewels();
        public Jewel? Get(int id) => _IDataService.ReadJewels().FirstOrDefault(j => j.Id == id);

        public void Add(Jewel newJewel)
        {   
            var jewelsList=_IDataService.ReadJewels();
            newJewel.Id = jewelsList.Count()+1;
            jewelsList.Add(newJewel);
            _IDataService.WriteJewels(jewelsList);
        }

        public void Delete(int id)
{
    
    var jewelsList = _IDataService.ReadJewels();
    var oldJewel = Get(id);
    if (oldJewel is null)
    {
        return ;
    }

    jewelsList.RemoveAll(jewel => jewel.Id == id);
    _IDataService.WriteJewels(jewelsList);
}


        public void Update(Jewel newJewel)
        {
            var jewelsList=_IDataService.ReadJewels();
            var index = jewelsList.FindIndex(j => j.Id == newJewel.Id);
            if (index == -1)
                return;
            jewelsList[index] = newJewel;
            _IDataService.WriteJewels(jewelsList);
        }
        public int Count { get => _IDataService.ReadJewels().Count(); }
        
    }

    public static class JewelServiceHelper
    {
        public static void AddJewelService(this IServiceCollection BuilderService)
        {
            BuilderService.AddSingleton<IJewelService, JewelService>();
        }
    }

}
