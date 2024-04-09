// Classic example of flyweight - string internation in .NET.
namespace GOF
{
    public class FlyweightGOF
    {
        public void Flyweight()
        {
            Product product1 = new(10);
            Product product2 = new(10);
            Product product3 = new(15);
            Product product4 = new(15);

            Console.WriteLine(object.ReferenceEquals(product1.Info, product2.Info));
            Console.WriteLine(object.ReferenceEquals(product2.Info, product3.Info));
            Console.WriteLine(object.ReferenceEquals(product3.Info, product4.Info));
        }
    }

    public static class InternatedPool<TKey, TValue> where TValue: new() where TKey: notnull
    {
        private static Dictionary<TKey, TValue> internated = new();

        public static TValue GetReference(TKey key)
        {
            if (internated.TryGetValue(key, out var result)) { return result; }
            else { return internated[key] = new(); }
        }

        public static void Internate(TKey key, TValue value) => internated[key] = value;
    }


    public class Product 
    {
        public ProductInfo Info { get; init; }
        public Product(int id)
        {
            Info = InternatedPool<int, ProductInfo>.GetReference(id);
        }
    }

    public record class ProductInfo();

}