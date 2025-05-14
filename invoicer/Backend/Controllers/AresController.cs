using Microsoft.AspNetCore.Mvc;
using Application.ExternalServiceInterfaces;
using Infrastructure.ExternalServices.AresApi.AresApiModels;
using Application.Interfaces;
using Application.DTOs.AresApi;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AresController(IAresApiService aresApiService) : ControllerBase
	{
		[HttpGet("{ico:int}", Name = "GetEntityInformationByIco")]
		[ProducesResponseType(typeof(SubjectInformation), 200)]
		[ProducesResponseType(typeof(AresApiErrorResponse), 400)]

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
