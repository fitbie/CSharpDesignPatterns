namespace GOF.Builder
{
    public class BuilderGOF
    {
        public void Builder()
        {
            IntBuilder intBuilder = new();
            intBuilder.AppendEnd(3);
            intBuilder.AppendEnd(5);
            intBuilder.AppendFront(7);
            Console.WriteLine(intBuilder.ToInt());
        }
    }



    public class IntBuilder
    {
        private int value = 0;
        private int tens = 0;


        public void AppendEnd(int val)
        {
            value = (value * (int)Math.Pow(10, tens++)) + val;
        }


        public void AppendFront(int val)
        {
            value += val * (int)Math.Pow(10, tens++);
        }


        public int ToInt()
        {
            return value;
        }
    }

}