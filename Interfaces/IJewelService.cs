using lessson1.Models;
using System.Collections.Generic;
using System.Linq;

namespace lessson1.Interfaces
{
    public interface IJewelService
    {
        List<Jewel> GetAll();

        Jewel? Get(int id);

        void Add(Jewel newJewel);

        void Delete(int id);

        void Update(Jewel newJewel);

        int Count { get;}
        
    }
}
