using System;
using System.Collections.Generic;
using System.Linq;

namespace DaveAPI.Models
{
    /// <summary>
    /// The service data store.
    /// </summary>
    public class DataStore
    {
        /// <summary>
        /// Initializes a new instance of the DataStore class.
        /// </summary>
        public DataStore()
        {
            AdjustedAdditionHistoryByUser = new Dictionary<string, IList<Addition>>();
        }

        /// <summary>
        /// The history of positive additions for each user, adjusted for subsequent deductions and negative additions.
        /// </summary>
        /// <remarks>We essentially just add Addition records to this list as they come in. We keep track of all of the individual
        /// addition records in order to use points in the order in which they were acrued. When points are deducted,
        /// we walk through the list, consuming points as we go, subtracting the consumed points from the Addition
        /// records and updating them in the data store. Negative additions are treated similarly to deductions, except that the
        /// subtractions are only done for the specified payer.</remarks>
        private Dictionary<string, IList<Addition>> AdjustedAdditionHistoryByUser { get; set; }

        /// <summary>
        /// Gets the adjusted addition history for a specified user.
        /// </summary>
        /// <param name="user">The user for which to get the adjusted addition history.</param>
        /// <returns>The adjusted addition history for a specified user.</returns>
        private IList<Addition> GetAdjustedAdditionHistoryForUser(string user)
        {
            IList<Addition> adjustedAdditionHistory;
            if (!AdjustedAdditionHistoryByUser.TryGetValue(user, out adjustedAdditionHistory))
            {
                adjustedAdditionHistory = new List<Addition>();
                AdjustedAdditionHistoryByUser[user] = adjustedAdditionHistory;
            }
            return adjustedAdditionHistory;
        }

        /// <summary>
        /// Implements the service's Add operation.
        /// </summary>
        /// <param name="addition">An object specifying the parameters for the operation.</param>
        public void Add(Addition addition)
        {
            if (addition.Points == 0)
            {
                return;
            }

            IList<Addition> adjustedAdditionHistory = GetAdjustedAdditionHistoryForUser(addition.User);

            if (addition.Points > 0)
            {
                // When the points are positive, we simply add the record to the user's history.
                adjustedAdditionHistory.Add(addition);
            }
            else // < 0
            {
                // When the points are negative, we reduce any positive balance this user has for the
                // specified payer, taking care to handle it correctly if the balance is spread across
                // multiple history items, and also being sure not to bring the balance negative.
                int remainingToSubtract = -addition.Points;
                for (int i = 0; i < adjustedAdditionHistory.Count && remainingToSubtract > 0; ++i)
                {
                    Addition a = adjustedAdditionHistory[i];
                    if (a.Payer == addition.Payer)
                    {
                        int subtracting = Math.Min(a.Points, remainingToSubtract);
                        a.Points -= subtracting;
                        remainingToSubtract -= subtracting;
                    }
                }
            }
        }

        /// <summary>
        /// Implements the service's Deduct operation.
        /// </summary>
        /// <param name="deduction">An object specifying the parameters for the operation.</param>
        /// <returns>A collection that indicates how much was deducted from the user per payer.</returns>
        public IList<DeductionResultItem> Deduct(Deduction deduction)
        {
            var result = new List<DeductionResultItem>();
            var now = DateTime.UtcNow;

            IList<Addition> adjustedAdditionHistory = GetAdjustedAdditionHistoryForUser(deduction.User);

            int remainingToDeduct = deduction.Points;
            for (int i = 0; i < adjustedAdditionHistory.Count && remainingToDeduct > 0; ++i)
            {
                Addition a = adjustedAdditionHistory[i];

                if (a.Points == 0)
                {
                    continue;
                }

                // Deduct the points (but not more than this Addition record has.)
                int deducting = Math.Min(a.Points, remainingToDeduct);
                a.Points -= deducting;

                // Put this Addition's payer in the result set. Each payer appears at most once, with the points accumulated per payer.
                // Note that if the result collection were expected to be large, then an indexed lookup would be in order. Assumed not needed here.
                DeductionResultItem item = result.FirstOrDefault(i => i.Payer == a.Payer);
                if (item == null)
                {
                    result.Add(new DeductionResultItem
                    {
                        Payer = a.Payer,
                        Points = -deducting, // points are returned from this operation as negative numbers
                        Date = now
                    });

                }
                else
                {
                    item.Points -= deducting; // points are returned from this operation as negative numbers
                }

                remainingToDeduct -= deducting;
            }

            return result;
        }

        /// <summary>
        /// Implements the service's balance operation.
        /// </summary>
        /// <param name="user">The user for whom to return the balance.</param>
        /// <returns>A collection of objects, one for each payer, indicating the user's total balance for that payer.</returns>
        public IList<BalanceItem> GetBalanceForUser(string user)
        {
            var result = new List<BalanceItem>();

            foreach (Addition addition in GetAdjustedAdditionHistoryForUser(user))
            {
                // Put this Addition's payer in the result set. Each payer appears at most once, with the points accumulated.
                // Note that if the result collection were expected to be large, then an indexed lookup would be in order. Assumed not needed here.
                BalanceItem item = result.FirstOrDefault(i => i.Payer == addition.Payer);
                if (item == null)
                {
                    result.Add(new BalanceItem
                    {
                        Payer = addition.Payer,
                        Points = addition.Points
                    });

                }
                else
                {
                    item.Points += addition.Points;
                }
            }

            return result;
        }
    }
}
