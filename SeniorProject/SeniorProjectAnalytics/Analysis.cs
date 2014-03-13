using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeniorProject;

namespace SeniorProjectAnalytics
{
    class Analysis
    {
        private ISimilarity isSim;
        private ICompressible[] inputs;
        private ICompressible[] output;

        /// <summary>
        /// Read file "input.txt" and set list of TicketCompressible items.
        /// </summary>
        /// <param name="entitys">Array of Ticket to be overwritten.</param>
        public void ObtainTickets(ref TicketCompressible[] entitys)
        {
            // TODO: implement
        }

        /// <summary>
        /// Finds all tickets which are similar to the entity. 
        /// </summary>
        /// <param name="entity">Ticket to use for comparison.</param>
        /// <param name="entityList">Array of tickets to compare against.</param>
        /// <returns>An array of tickets similar to entity.</returns>
        public TicketCompressible[] FindSimilars(TicketCompressible entity, TicketCompressible[] entityList)
        {
            // TODO: implement
            return entityList;
        }

        public void Main(string[] args )
        {
            // TODO: implement

            // TODO: ObtainTickets

            // TODO: FindSimilars for each ticket and write to file
        }
    }
}
