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
			IResult<IAresApiResponse> subjectInformation = await aresApiService.GetEntityInformationByIcoAsync(ico);

			if (subjectInformation.IsSuccess)
			{
				return Ok(subjectInformation.Data);
			}
			return StatusCode(subjectInformation.StatusCode, subjectInformation.ErrorMessage);
		}
	}
}
