using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.GOF;

/// <summary>
/// Strategy, also known as Policy - behavioral pattern that enables selecting an algorithm at runtime. This is achieved by incapsulating algorithm inside object instead
/// of implementing a single algorithm directly, such strategy objects has common base class,  therefore, the set of implementations of the base strategy class
/// forms something like "family of algorithms". 
/// <para>Strategy lets the algorithm exist independently from clients that use it, and strategies themselves are independent from clients, except for pull design, i.e. client
/// pass itself to the strategy, so it can read required values from client, but usually strategies are designed to operate only with the arguments they need in order 
/// to maximize reuse in the code base (for the same reason the Flyweight or Singleton pattern is often applied to strategies).
/// </para>
/// <para>
/// Strategies is the pattern I use most often, and the pattern I observe most often in software in general. One of the most cool examples of strategies in .NET are 
/// the <see cref="IComparer{T}"/> implementations used in ordering collections. If we imagine methods like 
/// <see cref="List{T}.Sort(IComparer{T}?)"/>, <see cref="Array.Sort(Array, IComparer?)"/>, <see cref="Enumerable.Order{T}(IEnumerable{T}, IComparer{T}?)"/>, etc.
/// without passing IComparer, framework developers would have to define a huge number of if/switch statements to implement new sort criteria, and end user would have 
/// to inherit huge classes to define custom comparison criteria. However, the problem is solved by passing an <see cref="IComparer{T}.Compare(T?, T?)"/> implementation, 
/// or another way - implement this strategy by deriving from <see cref="Comparer{T}"/>, or create instance of pre-defined ComparisonComparer via 
/// <see cref="Comparer{T}.Create(Comparison{T})"/> and initialize it with <see cref="Comparison{T}"/> delegate.
/// </para>
/// <para>
/// At the same time in methods like <see cref="Array.Sort(Array, IComparer?)"/> the Strategy occurs once again - the sorting algorithm itself is determined by the
/// size of the collection, for small collections we use insertion sorting, which is slow but does not require preliminary preparation and uses minimum memory, 
/// and for large collections we use algorithms like fast sorting.
/// </para>
/// </summary>
public class StrategyPattern
{
    public void Strategy()
    {
        NegativeNumberStrategy strategy = new ArithmeticNegative();
        Console.WriteLine(strategy.MakeNumberNegative(10));

        strategy = new BitNegative();
        Console.WriteLine(strategy.MakeNumberNegative(10));

        strategy = new ParseNegative();
        Console.WriteLine(strategy.MakeNumberNegative(10));
    }
}


// We're gonna use simplest example possible - implementations of "make number negative" algorithms, incapsulated in strategy object.

public abstract class NegativeNumberStrategy
{
    public abstract int MakeNumberNegative(int number);
}


public class ArithmeticNegative : NegativeNumberStrategy
{
    public override int MakeNumberNegative(int number)
    {
        if (number > 0) number *= -1;
        return number;
    }
}

public class BitNegative : NegativeNumberStrategy
{
    public override int MakeNumberNegative(int number)
    {
        if (number > 0) number = (~number) + 1;
        return number;
    }
}


public class ParseNegative : NegativeNumberStrategy
{
    public override int MakeNumberNegative(int number)
    {
        if (number > 0) number = int.Parse($"-{number.ToString()}");
        return number;
    }
}
