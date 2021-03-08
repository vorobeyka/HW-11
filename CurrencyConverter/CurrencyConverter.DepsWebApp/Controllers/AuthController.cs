using Microsoft.AspNetCore.Mvc;
using System;
using DepsWebApp.Filters;
using DepsWebApp.Models;

namespace DepsWebApp.Controllers
{
    /// <summary>
    /// Authorization controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Register client with login and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Status code</returns>
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] User user)
        {
            throw new NotImplementedException();
        }
    }
}
