using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.HomeworkLib.GA
{
    /// <summary>
    /// Represents the result of each run/iteration.
    /// We can use this data for the plotting.
    /// </summary>
    public class AlgorithmResult
    {
        public double MaxFitness
        {
            get;
            set;
        }

        public double AverageFitness
        {
            get;
            set;
        }
        
        public int NumberOfMutationsOccured
        {
            get;
            set;
        }

        public int NumberOfCrossOversOccured
        {
            get;
            set;
        }

		public string GetChromosomeString
		{
			get;
			set;
		}

        public override string ToString()
        {
            return string.Format("max ftn: {0},  avg ftn: {1}, mutations: {2}, crossovers: {3}", 
                MaxFitness, AverageFitness, NumberOfMutationsOccured, NumberOfCrossOversOccured);
        }
    }
}
