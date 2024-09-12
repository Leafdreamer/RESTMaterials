using Microsoft.AspNetCore.Mvc;
using MaterialsLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTMaterials.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private MaterialRepository materials;

        public MaterialsController(MaterialRepository Materials)
        {
            materials = Materials;
        }

        // GET: api/<MaterialsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Material>> Get(string? filter = null, string? sorting = null)
        {
            List<Material>? list = materials.GetAll();

            if (filter != null)
            {
                // filtered list
                list = materials.Filter(filter);
            }
            if (sorting != null)
            {
                // sorted list
                list = materials.SortBy(sorting);
            }
            if (filter == null && sorting == null)
            {
                // normal list
                list = materials.GetAll();
            }

            if (list == null || list.Count == 0)
            {
                return NoContent();
            }

            return Ok(list);
        }

        // GET api/<MaterialsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Material> Get(int id)
        {
            Material? material = materials.GetById(id);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // POST api/<MaterialsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Material> Post([FromBody] Material value)
        {
            try
            {
                value.ValidateName();
                value.ValidateType();
                value.ValidateDateTime();
                value.ValidateAmount();

                Material newMat = materials.Add(value);

                return Created("/" + newMat.Id, newMat);
            }
            catch (ArgumentOutOfRangeException aex)
            {
                return BadRequest(aex);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex);
            }
        }

        // PUT api/<MaterialsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Material> Put(int id, [FromBody] Material value)
        {
            Material? tbu = materials.Update(id, value);

            if (tbu == null)
            {
                return NotFound();
            }

            return Ok(tbu);
        }

        // DELETE api/<MaterialsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Material> Delete(int id)
        {
            Material? tbd = materials.Delete(id);

            if (tbd == null)
            {
                return NotFound();
            }

            return Ok(tbd);
        }
    }
}
