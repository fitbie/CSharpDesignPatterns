// NegativeNumberStrategy strategy = new ArithmeticNegative();
// Console.WriteLine(strategy.MakeNumberNegative(10));

// strategy = new BitNegative();
// Console.WriteLine(strategy.MakeNumberNegative(10));


// public abstract class NegativeNumberStrategy
// {
//     public abstract int MakeNumberNegative(int number);
// }


// public class ArithmeticNegative : NegativeNumberStrategy
// {
//     public override int MakeNumberNegative(int number)
//     {
//         if (number > 0) { number *= -1; };
//         return number;
//     }
// }

// public class BitNegative : NegativeNumberStrategy
// {
//     public override int MakeNumberNegative(int number)
//     {
//         if (number > 0) { number = (~number) + 1; }
//         return number;
//     }
// }