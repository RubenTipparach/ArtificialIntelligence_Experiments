using System.Collections.Generic;
using System.IO;
using System;
using GeneticAlgorithm.HomeworkLib.GA;

public class FileHelper {

	/// <summary>
	/// Reads the file.
	/// </summary>
	/// <param name="fileName">Name of the file.</param>
	/// <returns></returns>
	public static  List<Link> ReadFile(string fileName)
    {
        List<Link> network = new List<Link>();

        // read each line of text file
        using (StreamReader reader = new StreamReader(fileName))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t'); //split by tabs

                network.Add(
                    new Link()
                    {
                        Node1 = Convert.ToInt32(items[0]),
                        Node2 = Convert.ToInt32(items[1]),
                        Cost = Convert.ToDouble(items[2]),
                        Active = false
                    // randomize Active for genetic DNA stuff :3
                });
            }
        }

        return network;
    }

	/// <summary>
	/// Writes the file. Dump the chromosome data.
	/// </summary>
	/// <param name="fileName">Name of the file.</param>
	/// <param name="results">The results.</param>
	public static void WriteFile(string fileName, List<AlgorithmResult> results)
	{
		// read each line of text file
		using (StreamWriter writer = new StreamWriter(fileName))
		{
			int i = 0;
			foreach (var r in results)
			{
				writer.WriteLine(
					 Environment.NewLine + "---------------------------" + Environment.NewLine
					 + i + 
					 Environment.NewLine + "---------------------------" + Environment.NewLine
					 + r.GetChromosomeString + 
					 Environment.NewLine + "---------------------------" + Environment.NewLine);
				i++;
			}
		}

	}

}
