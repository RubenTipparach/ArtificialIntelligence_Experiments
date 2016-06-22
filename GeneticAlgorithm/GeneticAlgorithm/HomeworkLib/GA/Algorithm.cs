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
        private int _initialPopulation;

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

        private Random random = new Random();

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
            _generations = generations;
            _initialPopulation = initialPopulation;
            _crossoverProbability = crossoverProbability;
            _mutationProbability = mutationProbability;

            _population = new List<Chromosome>(_initialPopulation);
        }

        /// <summary>
        /// This performs a run across all chromosomes.
        /// </summary>
        public IEnumerable<AlgorithmResult> Run()
        {
            // Step 1.
            GenerateChromosomes();

            // What happens during each gneration.
            for (int i = 0; i < _generations; i++)
            {
                yield return RunStructure();
            }
        }

        /// <summary>
        /// The algorithm broken down into a call structure.
        /// </summary>
        protected AlgorithmResult RunStructure()
        {
            // Step 2.
            int crossoversOccured = PerformCrossOver(); // PerformCrossOver() do it again! 

            // Step 3.
            int mutationsOccured = PerformMutation();

            // Step 4.
            PerformSelection();

            return new AlgorithmResult()
            {
                MaxFitness = Chromosome.MaxFitness(_population),
                AverageFitness = Chromosome.AverageFitness(_population),

                // assign these as we go.
                NumberOfCrossOversOccured = crossoversOccured,
                NumberOfMutationsOccured = mutationsOccured
            };
        }

        /// <summary>
        /// Step 1. Randomly generate the genetics <see cref="_initialPopulation"/> tiems.
        /// </summary>
        protected void GenerateChromosomes()
        {
            // randomly generate chromosomes for our population.
            Loop((int i) =>
            {
                List<Link> newLinks = new List<Link>(_initialLinks.Count);

                for (int j = 0; j < _initialLinks.Count; j++)
                {
                    newLinks.Add(_initialLinks[j].GenerateNewRandomLink(random));
                }

                // finished!
                _population.Add( new Chromosome(newLinks));
            });
        }

        /// <summary>
        /// Step 2. Perform crossover. Select a bunch of pairs,
        /// then randomly perform crossovers using the <see cref="_crossoverProbability"/>.
        /// </summary>
        protected int PerformCrossOver()
        {
            int crossoverCounter = 0;

            Loop((int i) =>
            {
                // randomly perform crossovers. Roll a dice or something.
                if (random.NextDouble() < _crossoverProbability)
                {
                    // random pair from population.
                    Chromosome aCrosser = _population[random.Next(_population.Count)];
                    Chromosome bCrosser = _population[random.Next(_population.Count)];

                    // Generate a random range from the number of genes.
                    int startRange = random.Next(_initialLinks.Count);
                    int endRange = random.Next(startRange, _initialLinks.Count);

                    // Hopefully my trust in references is honored.
                    Chromosome.Crossover(aCrosser, bCrosser, startRange, endRange);

                    crossoverCounter++;
                }
            });

            return crossoverCounter;
        }

        /// <summary>
        /// Step 3. Mutate a random gene using the <see cref="_mutationProbability"/>.
        /// </summary>
        protected int PerformMutation()
        {
            int mutationCounter = 0;
            Random r = new Random();

            // randomly mutate.
            Loop((int i) =>
            {
                // randomly perform crossovers. Roll a dice or something.
                if (r.NextDouble() < _mutationProbability)
                {
                    int flipBit = r.Next(_initialLinks.Count);
                    
                    //flip this.
                    _population[i].Links[flipBit].Active = !_population[i].Links[flipBit].Active;

                    mutationCounter++;
                }
            });

            return mutationCounter;
        }

        /// <summary>
        /// Step 4. Perform the 'Tournament' selection. Randomly select two, which ever wins
        /// get's selected. Do this <see cref="_initialPopulation"/> times.
        /// </summary>
        protected void PerformSelection()
        {
            // take the current list, and copy it into a new one using the tournament rules.

            List<Chromosome> nextGeneration = new List<Chromosome>(_initialPopulation);
            Random r = new Random();

            Loop((int i) =>
            {
                Chromosome aContester = _population[r.Next(_population.Count)];
                Chromosome bContester = _population[r.Next(_population.Count)];

                // the winner of the tournament goes to the next round!
                if (aContester.Fitness > bContester.Fitness)
                {
                    nextGeneration.Add(aContester);
                }
                else
                {
                    nextGeneration.Add(bContester);
                }
            });

            //replace with new crossoverPopulation
            _population = nextGeneration;

        }

        /// <summary>
        /// I just hate typing for loops.
        /// </summary>
        private void Loop(Action<int> iterate)
        {
            for (int i = 0; i < _initialPopulation; i++)
            {
                iterate(i);
            }
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