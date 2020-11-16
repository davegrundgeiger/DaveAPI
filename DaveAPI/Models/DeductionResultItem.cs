using System;
namespace DaveAPI.Models
{
    /// <summary>
    /// Represents a single item in the results collection returned by the Deduct operation.
    /// </summary>
    public class DeductionResultItem
    {
        /// <summary>
        /// The payer for whom points are being reported.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// The number of points.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// The UTC date/time at which the deduction was made.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
