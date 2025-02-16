using Backend.Services.AresApiService;
using Backend.Services.AresApiService.Models;
using Backend.Services.AresApiService.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AresController(AresApiService aresApiService) : ControllerBase
	{
		[HttpGet("{ico:int}", Name = "GetEntityInformationByIco")]
		public async Task<IActionResult> GetEntityInformationByIco(string ico)
		{
			IResult<IAresApiResponse> subjectInformation = await aresApiService.GetEntityInformationByIcoAsync(ico);

			if (subjectInformation.IsSuccess)
			{
				return Ok(subjectInformation.Data);
			}
			return StatusCode(subjectInformation.StatusCode, subjectInformation.ErrorMessage);
		}
	}
}
