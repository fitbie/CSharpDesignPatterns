namespace GOF.Adapter
{
    public class AdapterGOF
    {
        public void Adapter()
        {
            Shape shape = new Triangle();
            shape.DrawShape();
            shape = new Square();
            shape.DrawShape();
            shape = new UpperTextAdapter();
            shape.DrawShape();
        }

    }    


    public abstract class Shape 
    { 
        public abstract void DrawShape();
    } 

    public class Triangle : Shape
    {
        public override void DrawShape() => Console.WriteLine("\x25B2");
    }

    public class Square : Shape
    {
        public override void DrawShape() => Console.WriteLine("\x25A0");
    }

    public class UpperTextAdapter : Shape
    {
        public override void DrawShape() => ThirdPartyLibrary.DrawText();
    }


    public class ThirdPartyLibrary
    {
        public static void DrawText() { Console.WriteLine(Console.ReadLine()!.ToUpper()); }
    }

}