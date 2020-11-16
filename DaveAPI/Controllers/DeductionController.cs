using System;
using System.Collections.Generic;
using DaveAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaveAPI.Controllers
{
    /// <summary>
    /// The controller for the Deduct operation endpoint.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DeductionController : ControllerBase
    {
        /// <summary>
        /// The handler for the Deduct operation endpoint.
        /// </summary>
        /// <param name="deduction">An object that specifies the deduction to be made.</param>
        /// <returns>A collection of items, one for each payer, indicating the points that were deducted from
        /// the balance for that payer for the user specified in the deduction.</returns>
        [HttpPost]
        public IList<DeductionResultItem> Post([FromBody] Deduction deduction)
        {
            return Program.DataStore.Deduct(deduction);
        }
    }
}
