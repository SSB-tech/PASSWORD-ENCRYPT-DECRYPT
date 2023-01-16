using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks.Dataflow;
using System.Data;

namespace Password_Encrypt_Decrypt.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class encrypt : ControllerBase
	{
		private readonly IConfiguration configuration;

		public encrypt(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		//Simple Encrypt

		[HttpPost]
		[Route("api/[controller]/encrypt")]
		public string enc(string input)
		{
			return PWencdec.encrypt(input);
		}	
		
		//Simple Decrypt
		
		[HttpPost]
		[Route("api/[controller]/decrypt")]
		public string dec(string input)
		{
			return PWencdec.decrypt(input);
		}
		//storing encrypted pw in database

		[HttpPost]
		[Route("api/[controller]/signup")]
		public async Task<IActionResult> Signup(enc data)
		{
			var connection = new SqlConnection(configuration.GetConnectionString("encrypt"));
			data.password = PWencdec.encrypt(data.password);
			await connection.ExecuteAsync("insert into encrypt (username, password) values (@username, @password)", data);
			Console.WriteLine(PWencdec.decrypt("MTIzNDVhc2RmYXNmYXNkZg=="));
			return Ok();
		}

		//Giving Access to people
		[HttpPost]
		[Route("api/[controller]/login")]

		public async Task<IActionResult> decrypt(Login enter)
		{
			enter.Password = PWencdec.encrypt(enter.Password);
			var connection = new SqlConnection(configuration.GetConnectionString("encrypt"));
			var ret = await connection.QueryAsync<enc>("select * from encrypt where username=@Username AND password = @Password", new {Username=enter.Username, Password =enter.Password });

			//return Ok(ret); //test garna use gareko

			if (ret == null || ret.Count()==0) //.count() checks total no of element in an array, this func is working here
			{
				return Ok("invalid");
			}
			else
			{
				return Ok("valid");
			}
			return Ok(ret);
		}


	}
}
