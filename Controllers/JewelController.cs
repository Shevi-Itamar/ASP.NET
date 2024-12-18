using Microsoft.AspNetCore.Mvc;
using lessson1.Models;

namespace lessson1.Controllers;

[ApiController]
[Route("[controller]")]
public class JewelController : ControllerBase
{
    private static List<Jewel> listJewels;
    static JewelController()
    {
        listJewels = new List<Jewel> 
        {
            new Jewel { Id = 1, Name = "Ring" ,Weight=25},
            new Jewel { Id = 2, Name = "Necklace",Weight= 50 }
        };
    }

    [HttpGet]
    public IEnumerable<Jewel> Get()
    {
        return listJewels;
    }

    [HttpGet("{id}")]
    public ActionResult<Jewel> Get(int id)
    {
        var Jewel = listJewels.FirstOrDefault(j => j.Id == id);
        if (Jewel == null)
            return BadRequest("invalid id");
        return Jewel;
    }

    [HttpPost]
    public ActionResult Insert(Jewel newJewel)
    {        
        var maxId = listJewels.Max(p => p.Id);
        newJewel.Id = maxId + 1;
        listJewels.Add(newJewel);

        return CreatedAtAction(nameof(Insert), new { id = newJewel.Id }, newJewel);
    }  

    
    [HttpPut("{id}")]
    public ActionResult Update(int id, Jewel newJewel)
    { 
        var oldJewel = listJewels.FirstOrDefault(j => j.Id == id);
        if (oldJewel == null) 
            return BadRequest("invalid id");
        if (newJewel.Id != oldJewel.Id)
            return BadRequest("id mismatch");

        oldJewel.Name = newJewel.Name;
        oldJewel.Weight = newJewel.Weight;

        return NoContent();
    } 


    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
     { 
        var oldJewel = listJewels.FirstOrDefault(j => j.Id == id);
        if (oldJewel == null) 
            return BadRequest("invalid id");
        listJewels.Remove(oldJewel);

        return NoContent();
    } 
    
}
