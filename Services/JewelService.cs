using lessson1.Models;
using lessson1.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace lessson1.Services
{
    public class JewelService :IJewelService
    {
        List<Jewel> listJewels { get; }

        int nextId = 3;

        public JewelService()
        {
        listJewels = new List<Jewel> 
        {
            new Jewel { Id = 1, Name = "Ring" ,Weight=25},
            new Jewel { Id = 2, Name = "Necklace",Weight= 50 }
        };
        }
        public List<Jewel> GetAll() => listJewels;

        public Jewel Get(int id) => listJewels.FirstOrDefault(j=> j.Id == id);

        public void Add(Jewel newJewel)
        {
            newJewel.Id = nextId++;
            listJewels.Add(newJewel);
        }

        public void Delete(int id)
        {
            var oldJewel = Get(id);
            if(oldJewel is null)
                return;

            
            listJewels.Remove(oldJewel);
        }

        public void Update(Jewel newJewel)
        {
            var index = listJewels.FindIndex(j => j.Id == newJewel.Id);
            if(index == -1)
                return;

            listJewels[index] = newJewel;
        }

        public int Count { get =>  listJewels.Count();}
    }

}
