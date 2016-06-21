using System.Collections.Generic;
using System.IO;
using System;

public class FileHelper {

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

}
