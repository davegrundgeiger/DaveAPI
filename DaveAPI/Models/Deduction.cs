using System;
namespace DaveAPI.Models
{
    /// <summary>
    /// Represents an amount that is to be deducted from a user's balance.
    /// </summary>
    public class Deduction
    {
        /// <summary>
        /// The user from whom to deduct points.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The number of points to deduct.
        /// </summary>
        public int Points { get; set; }
    }
}
