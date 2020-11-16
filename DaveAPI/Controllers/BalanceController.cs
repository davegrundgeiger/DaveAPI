using System;
using System.Collections.Generic;
using DaveAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaveAPI.Controllers
{
    /// <summary>
    /// The controller for the Balance operation endpoint.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceController : ControllerBase
    {
        /// <summary>
        /// The handler for the Balance operation endpoint.
        /// </summary>
        /// <param name="user">The name of the user for which to get the balance.</param>
        /// <returns>A collection of objects, one for each payer, that give the balance for that payer for the specified user.</returns>
        [HttpGet]
        public IEnumerable<BalanceItem> Get(string user)
        {
            return Program.DataStore.GetBalanceForUser(user);
        }
    }
}
