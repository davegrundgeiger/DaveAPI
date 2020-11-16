using System;
namespace DaveAPI.Models
{
    /// <summary>
    /// Represents an addition to be made to the points for a specified user and payer.
    /// </summary>
    public class Addition
    {
        /// <summary>
        /// The user for whom to add points.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// The payer for which the points are added.
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// The points to add.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// The transaction date.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
