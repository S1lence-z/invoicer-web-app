using Application.DTOs;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Api;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EntityController(IEntityService entityService, IEntityInvoiceNumberingStateService entityNumberingStateService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetEntityById")]
		[ProducesResponseType(typeof(EntityDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				EntityDto entity = await entityService.GetByIdAsync(id);
				return Ok(entity);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Entity not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create(e.Message, e.Message, 500));
			}
		}

		[HttpGet(Name = "GetAllEntities")]
		[ProducesResponseType(typeof(IList<EntityDto>), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<EntityDto> entityList = await entityService.GetAllAsync();
				return Ok(entityList);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create(e.Message, e.Message, 500));
			}
		}

		[HttpPost(Name = "PostEntity")]
		[ProducesResponseType(typeof(EntityDto), 201)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Post([FromBody] EntityDto entity)
		{
			if (!ModelState.IsValid)
				return BadRequest(ApiErrorResponse.Create("Invalid model state", ModelState.ToString()!, 400));

			try
			{
				EntityDto newEntity = await entityService.CreateAsync(entity);
				await entityNumberingStateService.CreateByEntityId(newEntity.Id);
				return CreatedAtRoute("GetEntityById", new { id = newEntity.Id }, newEntity);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create(e.Message, e.Message, 500));
			}
		}

		[HttpPut("{id:int}", Name = "PutEntity")]
		[ProducesResponseType(typeof(EntityDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Put(int id, [FromBody] EntityDto entity)
		{
			if (!ModelState.IsValid)
				return BadRequest(ApiErrorResponse.Create("Invalid model state", ModelState.ToString()!, 400));

			try
			{
				EntityDto? updatedEntity = await entityService.UpdateAsync(id, entity);
				return Ok(updatedEntity);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create(e.Message, e.Message, 500));
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteEntity")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await entityService.DeleteAsync(id);
				if (!wasDeleted)
				{
					return NotFound(ApiErrorResponse.Create("Entity not found", $"Entity with ID {id} not found.", 404));
				}
				return Ok($"Entity with ID {id} was deleted.");
			}
			catch (InvalidOperationException e)
			{
				return BadRequest(ApiErrorResponse.Create(e.Message, e.Message, 400));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create(e.Message, e.Message, 500));
			}
		}
	}
}
