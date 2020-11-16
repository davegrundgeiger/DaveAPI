using System;
using System.Collections.Generic;
using DaveAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaveAPI.Controllers
{
    /// <summary>
    /// The controller for the Add operation endpoint.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdditionController : ControllerBase
    {
        /// <summary>
        /// The handler for the Add operation endpoint.
        /// </summary>
        /// <param name="additions">The collection of items specifying the additions to be made.</param>
        [HttpPost]
        public void Post([FromBody] Addition[] additions)
        {
            foreach (Addition addition in additions)
            {
                Program.DataStore.Add(addition);
            }
        }
    }
}
