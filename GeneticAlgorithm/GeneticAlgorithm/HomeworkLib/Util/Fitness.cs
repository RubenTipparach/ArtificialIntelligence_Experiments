using System.Collections.Generic;

namespace HomeworkLib.Util
{
    public class Fitness
    {
        /// <summary>
        /// fitness value. 1 - cost/totalcost
        /// </summary>
        private double _fitness;

        /// <summary>
        /// cost of active links.
        /// </summary>
        private double _cost;

        /// <summary>
        /// cost of the total network.
        /// </summary>
        private double _costCompleteNetwork;

        /// <summary>
        /// The fitness function of each network.
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public double CalculateFitnessOfOverlayNetwork(List<Link> network)
        {
            _fitness = 0.0;
            _cost = 0.0;
            _costCompleteNetwork = 0.0;

            for (int i = 0; i < network.Count; i++)
            {
                _costCompleteNetwork += network[i].Cost;

                if (network[i].Active)
                {
                    _cost += network[i].Cost;
                }
            }

            _fitness = 1 - (_cost / _costCompleteNetwork);
            return _fitness;
        }
    }
}