using System;
/// <summary>
/// This class defines the structure of a link.
/// </summary>
public struct Link {

    /// <summary>
    /// node 1
    /// </summary>
    public int Node1;

    /// <summary>
    /// node 2
    /// </summary>
    public int Node2;

    /// <summary>
    /// cost
    /// </summary>
    public double Cost;

    /// <summary>
    /// is the node active
    /// </summary>
    public bool Active;

	
    /// <summary>
    /// Returns a link copy that is randomly activated.
    /// Useful for initializing a population.
    /// </summary>
    /// <returns></returns>
    public Link GenerateNewRandomLink(Random r)
    {
        double isActive = r.NextDouble();

        return new Link()
        {
            Node1 = this.Node1,
            Node2 = this.Node2,
            Cost = this.Cost,
            Active = isActive > 0.5 ? true : false
        };
    }

    public Link Copy()
    {
        return new Link()
        {
            Node1 = this.Node1,
            Node2 = this.Node2,
            Cost = this.Cost,
            Active = this.Active
        };
    }


    /// <summary>
    /// Displays the current node in string format.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("{0}-{1} Cost: {2}, Active: {3} ", Node1, Node2, Cost, Active);
    }
}
