using System;
namespace DaveAPI.Models
{
    /// <summary>
    /// Represents a single line in a user's balance report.
    /// </summary>
    public class BalanceItem
    {
        /// <summary>
        /// The payer with whom the points are associated.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// The number of points.
        /// </summary>
        public int Points { get; set; }
    }
}
