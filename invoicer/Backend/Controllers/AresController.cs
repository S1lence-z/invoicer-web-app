using Backend.Services.AresApiService;
using Backend.Services.AresApiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AresController(AresApiService aresApiService) : ControllerBase
	{
		[HttpGet("{ico}", Name = "GetEntityInformationByIco")]
		public async Task<IActionResult> GetEntityInformationByIco(string ico)
		{
			SubjectInformation? subjectInformation = await aresApiService.GetEntityInformationByIcoAsync(ico);
			if (subjectInformation == null)
			{
				return NotFound($"Entity with ICO {ico} not found");
			}
			return Ok(subjectInformation);
		}
	}
}
