using HomeworkLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.HomeworkLib.GA
{

    public class Chromosome
    {
        /// <summary>
        /// Technically this is overkill, but I'm too lazy to manage efficient arrays.
        /// Can be optimized.
        /// </summary>
        private List<Link> _encodedString;

		/// <summary>
		/// Copies this instance.
		/// </summary>
		/// <returns>A deep copy of the Chromosome object.</returns>
		public Chromosome Copy()
		{
			// perform deep copy.
			List<Link> newLinks = new List<Link>();
			foreach(var l in _encodedString)
			{
                var nl = l.Copy();
                newLinks.Add(nl);
			}

			return new Chromosome(newLinks);
		}

        /// <summary>
        /// I guess I like this class better than the links one.
        /// </summary>
        /// <param name="genetics"></param>
        public Chromosome(List<Link> genetics)
        {
            _encodedString = genetics;
        }

        /// <summary>
        /// Gets the range in the current chromosome.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        private bool[] GetRange(int startIndex, int endIndex)
        {
            bool[] linkReplaceRange = new bool[endIndex - startIndex];
            int y = 0;

            // make copy of desired change.
            for (int i = 0; i < linkReplaceRange.Length; i++)
            {
                linkReplaceRange[y] = _encodedString[i].Active;
                y++;
            }

            // Make sure it's in range!
            return linkReplaceRange;
        }

        /// <summary>
        /// Replaces the current chromosome segment with the target range.
        /// </summary>
        /// <param name="rangeString"></param>
        /// <param name="startIndex"></param>
        /// <param name="endindex"></param>
        private void Replace(bool[] rangeString, int startIndex, int endindex)
        {
            int y = 0;

            for (int i = startIndex; i < endindex; i++)
            {
				//copy link over.
				Link newLink = _encodedString[i];
				newLink.Active = rangeString[y];

                _encodedString[i] = newLink;
                y++;
            }
        }

        /// <summary>
        /// Perform crossover of A and B chromosomes.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public static void Crossover(Chromosome a, Chromosome b, int startIndex, int endIndex)
        {
            // I'm using the bools because they are structs and not copied by ref.
            bool[] aString = a.GetRange(startIndex, endIndex);
            bool[] bString = b.GetRange(startIndex, endIndex);

            a.Replace(bString, startIndex, endIndex);
            b.Replace(aString, startIndex, endIndex);
        }

        /// <summary>
        /// Returns fitness value.
        /// </summary>
        public double Fitness
        {
            get
            {
				var f = new Fitness();
				return f.CalculateFitnessOfOverlayNetwork(_encodedString);
            }
        }

        /// <summary>
        /// Links.
        /// </summary>
        public List<Link> Links
        {
            get
            {
                return _encodedString;
            }
        }

		/// <summary>
		/// Populations the string.
		/// </summary>
		/// <param name="chromosomes">The chromosomes.</param>
		/// <returns></returns>
		public static string PopulationString(List<Chromosome> chromosomes)
		{
			string s = "";
			foreach (var link in chromosomes)
			{
				s += link.ToString() + Environment.NewLine;
			}

			return s;
		}

        /// <summary>
        /// Gets the fitness average of all chromosomes in this list.
        /// </summary>
        /// <param name="chromosomes"></param>
        /// <returns>Yay Linq!</returns>
        public static double AverageFitness(List<Chromosome> chromosomes)
        {
			double totalScore = 0;
			foreach (var c in chromosomes)
			{
				totalScore += c.Fitness;
			}

            return totalScore/(double)chromosomes.Count;
        }

		/// <summary>
		/// This concats all the genes together.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string s = "";

			foreach(var link in _encodedString)
			{
				s += link.Active ? "1" : "0";
			}

			return s;
		}
	}
}
