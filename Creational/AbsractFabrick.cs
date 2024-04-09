namespace GOF.AbstractFabrick
{
    public class AbstractFabrickGOF
    {
        public void AbstractFabrick()
        {
            ShoesFabrick fabrick = new SneakersFabrick();
            Console.WriteLine(fabrick.MakeShoes());

            fabrick = new BootFabrick();
            Console.WriteLine(fabrick.MakeShoes());
        }
    }


    public abstract class ShoesFabrick
    {
        public abstract string MakeShoes(); 
    }

    public class SneakersFabrick : ShoesFabrick
    {
        public override string MakeShoes()
        {
            return "Puma Sneakers";
        }
    }

    public class BootFabrick : ShoesFabrick
    {
        public override string MakeShoes()
        {
            return "Leather Boots";
        }
    }


}