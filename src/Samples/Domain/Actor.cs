namespace Samples.Domain;

public class Actor
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public virtual List<Movie> Movies { get; set; } = new List<Movie>();
}
