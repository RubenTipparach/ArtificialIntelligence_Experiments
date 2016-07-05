using HomeworkLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.HomeworkLib.GA
{
    /// <summary>
    /// To be implemented along with any other classes you might create such as
    /// crossover, mutation, selection, etc.
    /// </summary>
    public class Algorithm
    {

        /// <summary>
        /// 
        /// </summary>
        private int _generations;

        /// <summary>
        /// 
        /// </summary>
        private int _populationSize;

		private int _geneSize;

        /// <summary>
        /// 
        /// </summary>
        private double _crossoverProbability;

        /// <summary>
        /// 
        /// </summary>
        private double _mutationProbability;

        /// <summary>
        /// Represents the link.
        /// </summary>
        private List<Link> _initialLinks;

        /// <summary>
        /// 
        /// </summary>
        private List<Chromosome> _population;

        /// <summary>
        /// Store the data in a list of each run for analysis later.
        /// </summary>
        private List<AlgorithmResult> _resultData;

		private Random _random;

		private double _globalMax = 0;

        /// <summary>
        /// Initialize an instance of the algorithm.
        /// </summary>
        /// <param name="generations"></param>
        /// <param name="initialPopulation"></param>
        /// <param name="crossoverProbability"></param>
        /// <param name="mutationProbability"></param>
        public Algorithm(List<Link> links, int generations, int initialPopulation, double crossoverProbability, double mutationProbability)
        {
            _initialLinks = links;
			_geneSize = links.Count;
			_generations = generations;
            _populationSize = initialPopulation;
            _crossoverProbability = crossoverProbability;
            _mutationProbability = mutationProbability;

			_random = new Random();
			_population = new List<Chromosome>(_populationSize);
        }

        /// <summary>
        /// This performs a run across all chromosomes.
        /// </summary>
        public IEnumerable<AlgorithmResult> Run()
        {
			//_random = new Random();

			// Step 1.
			GenerateChromosomes();

            // initial eval
            yield return new AlgorithmResult()
            {
                MaxFitness = MaxFitness(_population),
                AverageFitness = Chromosome.AverageFitness(_population),

                // assign these as we go.
                NumberOfCrossOversOccured = 0,
                NumberOfMutationsOccured = 0//,
                //GetChromosomeString = Chromosome.PopulationString(_population) // <-- use for debug.
            };

            // Happens during each gneration.
            for (int i = 0; i < _generations; i++)
            {
                // dump the data!
                yield return RunStructure();
            }
        }

        /// <summary>
        /// The algorithm broken down into a call structure.
        /// </summary>
        protected AlgorithmResult RunStructure()
        {
            // Step2 selection & Step 3 crossover.
            int crossoversOccured = PerformCrossOver(); // PerformCrossOver() do it again! 

            // Step 4.
            int mutationsOccured = PerformMutation();

            return new AlgorithmResult()
            {
                MaxFitness = MaxFitness(_population),
                AverageFitness = Chromosome.AverageFitness(_population),

                // assign these as we go.
                NumberOfCrossOversOccured = crossoversOccured,
                NumberOfMutationsOccured = mutationsOccured//,
                //GetChromosomeString = Chromosome.PopulationString(_population) // <-- use for debug.
            };
        }

		/// <summary>
		/// Calculate what the maximum fit ness is of a given set of chromosomes.
		/// </summary>
		/// <param name="chromosomes"></param>
		/// <returns></returns>
		public double MaxFitness(List<Chromosome> chromosomes)
		{
			double max = chromosomes.Max(p => p.Fitness);

			if (_globalMax < max)
			{
				_globalMax = max;
			}

			return _globalMax;
		}

		/// <summary>
		/// Step 1. Randomly generate the genetics <see cref="_populationSize"/> tiems.
		/// </summary>
		protected void GenerateChromosomes()
        {
			// randomly generate chromosomes for our population.
			for (int i = 0; i < _populationSize; i++)
			{
				List<Link> newLinks = new List<Link>(_geneSize);

                for (int j = 0; j < _geneSize; j++)
                {
                    newLinks.Add(_initialLinks[j].GenerateNewRandomLink(_random));
                }

                // finished!
                _population.Add( new Chromosome(newLinks));
            }
        }

        /// <summary>
        /// Step 2. Perform crossover. Select a bunch of pairs,
        /// then randomly perform crossovers using the <see cref="_crossoverProbability"/>.
        /// </summary>
        protected int PerformCrossOver()
        {
            int crossoverCounter = 0;

            List<Chromosome> nextGeneration = new List<Chromosome>(_population.Count);

            for (int i = 0; i < _populationSize/2; i++)
			{
				double selection = _random.NextDouble();

                // Random pair selected from population, based on 'tournament' selection.
                Chromosome aCrosser = PerformTournamentSelection();
                Chromosome bCrosser = PerformTournamentSelection();

                // randomly perform crossovers. Roll a dice or something.
                if (selection < _crossoverProbability)
                {
                    // Generate a random range from the number of genes.
                    int startRange = _random.Next(_geneSize);
                    int endRange = _random.Next(startRange, _geneSize);

                    // Hopefully my trust in references is honored.
                    Chromosome.Crossover(aCrosser, bCrosser, startRange, endRange);

                    crossoverCounter++;
                }

                nextGeneration.Add(aCrosser);
                nextGeneration.Add(bCrosser);
            }

            _population = nextGeneration;
            return crossoverCounter;
        }

        /// <summary>
        /// Step 3. Mutate a random gene using the <see cref="_mutationProbability"/>.
        /// </summary>
        protected int PerformMutation()
        {
            int mutationCounter = 0;

			//Random r = new Random(); Local random causing trouble!!!!!!!

			// randomly mutate.
			for (int i = 0; i < _population.Count; i++)
			{
                // randomly perform crossovers. Roll a dice or something.
                //double mutate = _random.NextDouble();
                double mutate = (new Random()).NextDouble();
                if (mutate < _mutationProbability)
                {
                    int flipBit = _random.Next(_geneSize);

					//flip this.
					Link copy = _population[i].Links[flipBit];
					copy.Active = !copy.Active;

					//copy over new link.
                    _population[i].Links[flipBit] = copy;

                    mutationCounter++;
                }
            }

            return mutationCounter;
        }

        /// <summary>
        /// Step 2. Perform the 'Tournament' selection. Randomly select two, which ever wins
        /// get's selected. Do this <see cref="_populationSize"/> times.
        /// </summary>
        protected Chromosome PerformTournamentSelection()
        {
            int a = _random.Next(_population.Count);
            int b = _random.Next(_population.Count);


            // Warning! Copy this, ;ooks like lots of side effects if I dont!
            Chromosome aContester = _population[a].Copy();
            Chromosome bContester = _population[b].Copy();

            Chromosome winner;

            // the winner of the tournament goes to the next round!
            if (aContester.Fitness > bContester.Fitness)
            {
                winner = aContester;
            }
            else
            {
                winner = bContester;
            }

            return winner;
        }

        /// <summary>
        /// The raw links. Will be replaced as iteration goes on.
        /// </summary>
        public List<Link> Links
        {
            get
            {
                return _initialLinks;
            }
        }
    }
}