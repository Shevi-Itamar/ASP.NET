using Microsoft.AspNetCore.Mvc;
using lessson1.Models;
using lessson1.Interfaces;

namespace lessson1.Controllers;

[ApiController]
[Route("[controller]")]
public class JewelController : ControllerBase
{
    private IJewelService JewelService;


        public JewelController(IJewelService jewelService)
        {
            this.JewelService = jewelService;
        }


        [HttpGet]
        public ActionResult<List<Jewel>> GetAll() =>
            JewelService.GetAll();


        [HttpGet("{id}")]
        public ActionResult<Jewel> Get(int id)
        {
            var jewel = JewelService.Get(id);

            
          if (jewel == null)
              return NotFound();
        //  throw new Exception("error");
          return jewel;
        }

        [HttpPost] 
        public IActionResult Create(Jewel newJewel)
        {
            JewelService.Add(newJewel);
            return CreatedAtAction(nameof(Create), new {id=newJewel.Id}, newJewel);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Jewel newJewel)
        {
            if (id != newJewel.Id)
                return BadRequest();

            var existingJewel = JewelService.Get(id);
            if (existingJewel is null)
                return  NotFound();

            JewelService.Update(newJewel);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var oldJewel = JewelService.Get(id);
            if (oldJewel is null)
                return  NotFound();

            JewelService.Delete(id);

            return Content(JewelService.Count.ToString());
        }
    }
