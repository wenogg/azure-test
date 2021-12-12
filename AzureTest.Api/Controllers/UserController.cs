using AzureTest.Core.Entities;
using AzureTest.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTest.Api.Controllers {
	/// <summary>
	/// Collection of users
	/// </summary>
	[Route("users")]
	public class UserController : Controller {
		private readonly SandboxDBContext _context;
		private List<User> _users = new List<User>();
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context"></param>
		public UserController(SandboxDBContext context) {
			_context = context;
		}

		/// <summary>
		/// Returns a collection of users
		/// </summary>
		/// <returns></returns>
		[HttpGet("")]
		[ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
		public async Task<ActionResult<List<User>>> Get(string name) {
			var users = await _context.Users.Where(s => string.IsNullOrEmpty(name) || s.Name.ToLower().Contains(name.ToLower())).ToListAsync();
			return Ok(users);
		}

		/// <summary>
		/// Returns a single user
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id:int}")]
		[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
		public async Task<ActionResult<User>> Get(int id) {
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

			if (user == null) {
				return NotFound();
			}

			return Ok(user);
		}

	}

}
