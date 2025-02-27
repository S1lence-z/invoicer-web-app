using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Domain.ServiceInterfaces;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AresController(IAresApiService aresApiService) : ControllerBase
	{
		[HttpGet("{ico:int}", Name = "GetEntityInformationByIco")]
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
