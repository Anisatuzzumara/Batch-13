using System.Collections.Generic;      

namespace Poker.Interfaces
{
    public interface ITable
    {
        /// <summary>
        /// Sets a community card on the table.
        /// </summary>
        /// <param name="card">The card to be added to the community cards.</param>
        void SetCommunityCard(ICard card);

        /// <summary>
        /// Gets the list of community cards on the table.
        /// </summary>
        /// <returns>A list of community cards.</returns>
        List<ICard> GetCommunityCards();
    }
}