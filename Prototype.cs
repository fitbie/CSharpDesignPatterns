// This pattern is already implemented in .NET thanks to ICloneable and MemberwiseClone().
namespace GOF.Prototype
{
    public class PrototypeGof
    {
        public void Prototype()
        {
            MachinePart part = new(0, "Engine", 130_000, [ new(4, "Shaft", 103.21f), new(2, "Piston", 13.74f, [ new(5, "PistonHead", 5.74f) ]) ]);
            Console.WriteLine(part);
            Console.WriteLine(part.Clone(false));
        }

    }



    public class MachinePart : IPrototype<MachinePart>
    {
        public float Weight { get; init; }
        public string Name { get; init; }
        public int ID { get; init; }
        public List<MachinePart> NeighbourParts { get; set; } = new();

        public MachinePart(int id, string name, float weight, params MachinePart[] neighbours)
        {
            ID = id; Name = name; Weight = weight;
            foreach (var mp in neighbours) { NeighbourParts.Add(mp); }
        }

        public override string ToString() => $"{Name} with neighbour parts: {string.Join(' ', NeighbourParts.Select((p) => p.Name))}";

        public MachinePart Clone(bool memberwise)
        {
            if (memberwise) { return (MemberwiseClone() as MachinePart)!; }

            MachinePart[] newParts = new MachinePart[NeighbourParts.Count];
            for (int i = 0; i < newParts.Length; i++) { newParts[i] = NeighbourParts[i].Clone(memberwise); }
            return new(ID, Name, Weight, newParts);
        }
    }


    public interface IPrototype<T>
    {
        public T Clone(bool memberwise);
    }

}