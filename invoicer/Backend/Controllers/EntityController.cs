using Application.DTOs;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EntityController(IEntityService entityService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetEntityById")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				EntityDto? entity = await entityService.GetByIdAsync(id);
				return Ok(entity);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpGet(Name = "GetAllEntities")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<EntityDto> entityList = await entityService.GetAllAsync();
				return Ok(entityList);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPost(Name = "PostEntity")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] EntityDto entity)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				EntityDto? newEntity = await entityService.CreateAsync(entity);
				return CreatedAtRoute("GetEntityById", new { id = newEntity!.Id }, newEntity);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPut("{id:int}", Name = "PutEntity")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] EntityDto entity)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				EntityDto? updatedEntity = await entityService.UpdateAsync(id, entity);
				return Ok(updatedEntity);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteEntity")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await entityService.DeleteAsync(id);
				if (!wasDeleted)
					return NotFound($"Entity with ID {id} not found.");
				return Ok($"Entity with ID {id} was deleted.");
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}
