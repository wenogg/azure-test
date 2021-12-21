using AutoMapper;
using AzureTest.Core.Entities;
using AzureTest.Core.ViewModels;
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
		private readonly IMapper _mapper;
		private List<ApplicationUser> _users = new List<ApplicationUser>();
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="context"></param>
		public UserController(SandboxDBContext context, IMapper mapper) {
			_context = context;
			_mapper = mapper;
		}

		/// <summary>
		/// Returns a collection of users
		/// </summary>
		/// <returns></returns>
		[HttpGet("")]
		[ProducesResponseType(typeof(List<UserViewModel>), StatusCodes.Status200OK)]
		public async Task<ActionResult<List<ApplicationUser>>> Get(string name) {
			var users = await _context.Users.Where(s => string.IsNullOrEmpty(name) || s.UserName.ToLower().Contains(name.ToLower())).ToListAsync();
			var items = _mapper.Map<List<ApplicationUser>, List<UserViewModel>>(users);
			return Ok(items);
		}

		/// <summary>
		/// Returns a single user
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApplicationUser>> GetOne(string id) {
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

			if (user == null) {
				return NotFound();
			}
			var item = _mapper.Map<UserViewModel>(user);
			return Ok(item);
		}

	}

}
