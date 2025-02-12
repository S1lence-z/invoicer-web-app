using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AddressController : ControllerBase
	{
		[HttpGet(Name = "GetAddress")]
		public string Get()
		{
			return "Address";
		}

		[HttpPost(Name = "PostAddress")]
		public string Post()
		{
			return "Address";
		}

		[HttpPut(Name = "PutAddress")]
		public string Put()
		{
			return "Address";
		}

		[HttpDelete(Name = "DeleteAddress")]
		public string Delete()
		{
			return "Address";
		}
	}
}
