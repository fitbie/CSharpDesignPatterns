namespace GOF.Singleton
{
    public class SingletonGOF
    {
        public void Signleton()
        {
            new Thread(() => Console.WriteLine(SimpleSingleton.Instance.GetText())).Start();
            Console.WriteLine(SimpleSingleton.Instance.GetText());

            new Thread(() => Console.WriteLine(LazySignleton.someNum)).Start();
            Console.WriteLine(LazySignleton.Instance.GetText());
        }

    }

    public class SimpleSingleton
    {
        private static SimpleSingleton instance = new();
        public static SimpleSingleton Instance => instance;

        // or
        // public static SimpleSingleton Instance { get; } = new();

        private SimpleSingleton() 
        {
            Console.WriteLine($"Singleton created on {DateTime.Now.TimeOfDay}");
        }


        public string GetText() => "Singleton text";

    }


    public class LazySignleton
    {
        public static int someNum = 101;
        private static Lazy<LazySignleton> instance = new(() => new LazySignleton());
        public static LazySignleton Instance
        {
            get => instance.Value;
        }

        private LazySignleton() => Console.WriteLine($"LazySingleton created on {DateTime.Now.TimeOfDay}");

        static LazySignleton() => Console.WriteLine($"Static Lazy constructor");

        public string GetText() => "LazySingleton text";

    }

}