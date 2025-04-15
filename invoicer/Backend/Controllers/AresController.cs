using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Application.ServiceInterfaces;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AresController(IAresApiService aresApiService) : ControllerBase
	{
		[HttpGet("{ico:int}", Name = "GetEntityInformationByIco")]
		[ProducesResponseType(200)]
		[ProducesResponseType(500)]

		public async Task<IActionResult> GetEntityInformationByIco(string ico)
		{
			IResult<IAresApiResponse> responseData = await aresApiService.GetEntityInformationByIcoAsync(ico);

			if (responseData.IsSuccess)
			{
				return Ok(responseData.Data);
			}
			return StatusCode(responseData.StatusCode, responseData);
		}
	}
}
