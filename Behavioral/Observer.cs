namespace Patterns.GOF;

/// <summary>
/// Observer pattern allows subject (publisher) perform one-to-many calls to observers (subscribers, listeners) in order to notify them about some changes, and observers
/// will react, i.e. handle this changes in some method.
/// <br/>In addition, such calls performed via either common interfaces or events (delegates), which helps to prevent high coupling in the code.
/// <para>
/// Most common and handy way to implement the pattern - use delegates and events. Delegates are pointers to methods, subject's delegate can contain any number of 
/// subscribers methods, which signature (arguments, return value) is corresponding to delegate's. 
/// Events act as incapsulation access methods around delegates, allowing subscribers only to subscribe/unsubscribe, but not call/change source delegate.
/// <br/>At the beginning (for example, initialization or construction), observer will subscribe  (i.e. combine/add) some method to subject's delegate (incapsulated in event). 
/// Then, when something changed, subject invoke its event, which leads to invoking all subscribers methods. 
/// Additionally delegate can pass arguments to its handlers (subscribers), for example some event arguments, containing list of changes. 
/// This implementation is very simple and powerful, 
/// considering that observers do not need implement any additional interfaces/mixins and ANY method, with corresponding to delegate signature, can be subscribed as event handler.
/// <br/>When using delegates and events - keep in mind that subject will hold reference to observer (in case you subscribed instance, not static method), so to prevent
/// memory leaks you need explicitly unsubscribe from event, for example using IDisposable (or other termination method).
/// </para>
/// <para>
/// Second approach is to declare interface which will be implemented by all observers. Subject then will hold list of subscribers as interface implementations and 
/// call its handling methods when need to notify. Additionally, subject need to declare add/remove (subscribe/unsubscribe) methods as part of public API.
/// <br/>In C# this implementation of the pattern
/// is pretty rare and inconvenient, it may be useful in complex systems, where for example there are different sources (subject) of the same change, so observers need to
/// watch different objects, but react (handle) only once. In that case, some Mediator object may be used as interlayer between subject and observers, it can get notification
/// from subjects about what observers need to be notified (instead of subject notifies observers directly) and call them itself, but only once. In my opinion, such case
/// is more like design problem, and you should keep only one common source of notification, but there may be exceptions.
/// </para>
/// <para>
/// There are two models for observers to gather changes information. Push - subject will pass changes information as argument (it can be argument of interface method or
/// event/delegate handler argument). Pull - observers will get notified about changes and access subject themselves to read and handle changes information (of course in
/// that case subject need to publicly expose useful fields/properties). First approach is great for strictly-determined type of changes, second is great when there are
/// various type of observers and therefore thy need to handle different information about changes.
/// </para>
/// </summary>
public class ObserverPattern
{
    public static void Start()
    {
        Wallet wallet = new();
        using EventObserver eventObserver = new(wallet);
        using InterfaceObserver interfaceObserver = new(wallet);

        wallet.ChangeBalance(100);
        wallet.ChangeBalance(-50);
    }
}

// Subject, which raise events to notify observers.
public class Wallet
{
    public decimal Balance { get; private set; }

    // Event-based approach
    public event Action? OnWalletChangedPull; // Notify listeners and let them read subject themselves
    public event Action<WalletEventArgs>? OnWalletChangedPush; // Or pass changes information to them

    //Or event use hook-alike notification, where observers may decide is it allowed to perform operation. Keep in mind that only last subscriber's return value will be used.
    public event Func<WalletEventArgs, bool>? OnBeforeWalledChangedHook;

    // Interface-based approach
    private List<IWalletObserver> balanceObservers = new();

    public void AddObserver(IWalletObserver observer) => balanceObservers.Add(observer);
    public bool RemoveObserver(IWalletObserver observer) => balanceObservers.Remove(observer);


    public bool ChangeBalance(decimal valueToAdd)
    {
        WalletEventArgs walletEventArgs = new(Balance + valueToAdd, valueToAdd);
        if (OnBeforeWalledChangedHook?.Invoke(walletEventArgs) ?? false) return false; // If hook returns true (if there is one) - prevent changes. Just to show you hooks.

        Balance += valueToAdd;
        NotifyObservers(walletEventArgs);
        return true;
    }


    private void NotifyObservers(WalletEventArgs args)
    {
        OnWalletChangedPull?.Invoke(); // Notify pullers
        OnWalletChangedPush?.Invoke(args); // And pushers

        foreach (var observer in balanceObservers)
        {
            // Same for interface-based
            observer.OnBalanceChanged();
            observer.OnBalanceChanged(args);
        }
    }
}

public readonly struct WalletEventArgs
{
    public readonly decimal newBalance;
    public readonly decimal balanceChanges;

    public WalletEventArgs(decimal currentBalance, decimal balanceChanges)
    {
        this.newBalance = currentBalance;
        this.balanceChanges = balanceChanges;
    }
}


// Interface approach
public interface IWalletObserver
{
    public void OnBalanceChanged();
    public void OnBalanceChanged(WalletEventArgs walletEventArgs);
}


// Below listed different implementations of observers:

public class EventObserver : IDisposable
{
    private Wallet wallet;

    public EventObserver(Wallet wallet)
    {
        this.wallet = wallet;
        wallet.OnWalletChangedPull += OnWalletChangedPull;
        wallet.OnWalletChangedPush += OnWalletChangedPush;
        wallet.OnBeforeWalledChangedHook += OnBeforeWalletChanged;
    }

    private void OnWalletChangedPull()
    {
        Console.WriteLine($"[EVENT PULL OBSERVER]: Wallet is changed, current balance: {wallet.Balance}");
    }

    private void OnWalletChangedPush(WalletEventArgs args) 
    {
        Console.WriteLine($"[EVENT PUSH OBSERVER]: Wallet is changed, {args.balanceChanges} added, current balance: {args.newBalance}");
    }

    private bool OnBeforeWalletChanged(WalletEventArgs args)
    {
        Console.WriteLine($"[HOOK OBSERVER]: Wallet is about to change from {args.newBalance - args.balanceChanges} to {args.newBalance}");
        return false; // We do not prevent changes in this hook.
    }


    // Free reference to wallet, can be done with any termination method, IDisposable is just handy.
    public void Dispose() => wallet.OnBeforeWalledChangedHook -= OnBeforeWalletChanged;
}


public class InterfaceObserver : IWalletObserver, IDisposable
{
    public Wallet wallet;

    public InterfaceObserver(Wallet wallet)
    {
        this.wallet = wallet;
        wallet.AddObserver(this);
    }

    public void OnBalanceChanged()
    {
        Console.WriteLine($"[INTERFACE PULL OBSERVER]: Wallet is changed, current balance: {wallet.Balance}");
    }

    public void OnBalanceChanged(WalletEventArgs args)
    {
        Console.WriteLine($"[INTERFACE PUSH OBSERVER]: Wallet is changed, {args.balanceChanges} added, current balance: {args.newBalance}");
    }

    // For interface-based observers we can ue disposing too, or any other termination method.
    public void Dispose() => wallet.RemoveObserver(this);
}