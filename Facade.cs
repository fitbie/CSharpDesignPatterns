namespace GOF
{
    public class FacadeGOF
    {
        public void Facade()
        {
            Framework framework = new(new(), new(), new());
            framework.Start();
        }
    }

    public class Framework(StaticAnalyzer analyzer, Compiler compiler, Runtime runtime)
    {
        public void Start()
        {
            analyzer.Analyze();
            Console.WriteLine("Analyzing complete!");
            var bin = compiler.Compile();
            Console.WriteLine("Compiling complete!");
            runtime.Execute(bin);
            Console.WriteLine("App is running");
            Console.ReadKey();
        }
    }


    public class StaticAnalyzer
    {
        public void Analyze() => Console.WriteLine("Analyzing..");
    }

    public class Compiler()
    {
        public class BinaryData {}

        public BinaryData Compile()
        {
            Console.WriteLine("Compiling");
            return new BinaryData();
        }
    }

    public class Runtime
    {
        public void Execute(Compiler.BinaryData data) => Console.WriteLine("Executing..");
    }

}