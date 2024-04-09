namespace GOF.FabrickMethod
{
    public class FabrickMethodGOF
    {
        public void FabrickMethod()
        {
            Bakery bakery = new DonutBakery();
            Console.WriteLine(bakery.Bake().Name);

            bakery = new CroissantBakery();
            Console.WriteLine(bakery.Bake().Name);
        }
    }



    public abstract class Bakery
    {
        public abstract Baking Bake();
    }

    public class DonutBakery : Bakery { public override Baking Bake() => new Donut("Strawbery Donut"); }
    public class CroissantBakery : Bakery { public override Baking Bake() => new Croissant("Chocolate Croissant"); }

    public record class Baking(string Name);
    public record class Donut(string Name) : Baking(Name);
    public record class Croissant(string Name) : Baking(Name);
    
}