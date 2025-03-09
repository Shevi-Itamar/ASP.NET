using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
             return jewel;
        }





        [HttpPost] 
        [Authorize(Policy ="Admin")]
        public IActionResult Create(Jewel newJewel)
        {   try
    {
        var claims = User.Claims;

        JewelService.Add(newJewel);
        return CreatedAtAction(nameof(Create), new {id = newJewel.Id}, newJewel);
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Internal server error");
    }

        }


        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(int id, Jewel newJewel)
        {   if (newJewel == null || newJewel.Id != id) 
                return BadRequest();

            var existingJewel = JewelService.Get(id);
            if (existingJewel is null)
                return  NotFound();

            JewelService.Update(newJewel);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var oldJewel = JewelService.Get(id);
            Console.WriteLine("oldJewel"+oldJewel+"   id"+id);
            if (oldJewel is null)
                return  NotFound();

            JewelService.Delete(id);

            return Content(JewelService.Count.ToString());
        }
    }
