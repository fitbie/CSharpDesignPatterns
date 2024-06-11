
using System.Net.Mail;

namespace Patterns.GOF;

/// <summary>
/// State pattern allows object to change its behavior based on its state. Imagine an some Context object that can be in several states and depending on them 
/// the logic of the object's methods (i.e. behavior) changes. In such case, each method need to use many if and switch operators to achieve desired effect.
/// However, if we can incapsulate state-depending logic into separate objects and change them, as Context's state need to be changed. Then, calls to Context can be
/// delegated to the its' current State.
/// <br/>This is very similar to <see cref="StrategyPattern"/> pattern, however usually there are few differences. First of all, states usually are component or aggregates of Context object,
/// i.e. source object that keeps references to states and using them, meanwhile strategies are often passed as argument. Also State object itself usually 
/// keeps reference to Context object to read its values, call methods, and more important - even to change Context's state to next, that means some State implementations
/// know about other states and conditions, under which Context object should switch to these states, and yes, this sounds a lot like Finite State Machine.
/// <br/>And most important - states implement almost everything that context does, being responsible for many different tasks that are redirected to them from the context.
/// </summary>
public class StatePattern
{
    public static void Start()
    {
        MailAddress myMail = new("fitbiestudio@gmail.com");
        TicketContext ticketContext = new(myMail, default);

        // User
        ticketContext.Close();
        ticketContext.Publish("abracadabra, woopsie, mistyped");
        ticketContext.Close();
        ticketContext.Publish("Hello, i have a problem with my software. What should i do?");

        // Tech support
        ticketContext.Reply("Hello! Have you tried to reboot your system? \n\tBest regards, some tech dude");

        // User
        ticketContext.Close();

        // Tech support
        ticketContext.Reply("Hello! It's been a while, does reboot helped? \n\tBest regards, some tech dude");
    }
}


// Context object using States. In this example we're emulating some support tickets system. User may open and close tickets, so they need to move from
// Draft -> Opened -> Closed -> Draft(re-opened). Also there is Reply() method for tech support employees. 
// In such design, where user may try to open already opened ticket, or tech support to reply to closed ticket and etc. - States are very useful, because they
// help developers to get rif of enormous switch statements and spaghetti code.
public class TicketContext
{
    TicketState currentState;
    public Guid TicketID { get; }
    public MailAddress UserAddress { get; }


    public TicketContext(MailAddress userAddress, TicketState? initialState)
    {
        currentState = initialState ?? new DraftTicketState();
        TicketID = Guid.NewGuid();
        UserAddress = userAddress;
    }


    public void Publish(string text) => currentState.Open(this, text);
    public void Close() => currentState.Close(this);
    public void Reply(string text) => currentState.Reply(this, text);
    public void ChangeState(TicketState newState) => currentState = newState;
}



// Abstract base for all future states. As we can see, states are "mirroring" their Context (TicketContext) keeper API.
public abstract class TicketState
{
    public abstract void Open(TicketContext context, string text);
    public abstract void Close(TicketContext context);
    public abstract void Reply(TicketContext context, string text);
    protected virtual void ChangeState(TicketContext context, TicketState newState) => context.ChangeState(newState);
}


// Draft (unpublished) state
public class DraftTicketState : TicketState
{
    public override void Close(TicketContext context)
    {
        Console.WriteLine($"Ticket {context.TicketID} draft was closed without publishing.\n");
    }

    public override void Open(TicketContext context, string text)
    {
        // Some backend work

        // Change context state to next.
        ChangeState(context, new OpenedTicketState());

        Console.WriteLine(@$"Ticket {context.TicketID} was opened successfully! We will try to get back to you as soon as possible. 
Check you email {context.UserAddress} for the details.
Your message: 
    {text}" + Environment.NewLine);

    }


    public override void Reply(TicketContext context, string text)
    {
        Console.WriteLine($"Ticket #{context.TicketID} was not opened yet.\n");
    }
}


// Opened ticket
public class OpenedTicketState : TicketState
{
   public override void Close(TicketContext context)
    {
        char input;
        do
        {
            Console.WriteLine($"Are you sure you want to close ticket {context.TicketID}? Please, type y for yes and n for no:");
        }
        while ((input = Console.ReadKey().KeyChar) is not ('y' or 'n'));

        if (input == 'y') 
        {
            Console.WriteLine($"Ticket {context.TicketID} was closed successfully!\n");
            ChangeState(context, new ClosedTicketState());
        }
        else Console.WriteLine($"Aborting Ticket {context.TicketID} closing..\n");

    }

    public override void Open(TicketContext context, string text)
    {
        Console.WriteLine($"Ticket #{context.TicketID} is already opened. Thank you for your patience, we will try to respond as best we can.\n");
    }


    public override void Reply(TicketContext context, string text)
    {
        // SmtpClient.Send, blah-blah

        Console.WriteLine($"[SYSTEM]: Reply was successfully sent to the user! Reply text: \n\t{text}\n");
    } 
}


// Closed (removed from database) ticket
public class ClosedTicketState : TicketState
{
    public override void Close(TicketContext context)
    {
        Console.WriteLine($"Ticket #{context.TicketID} is already closed!\n");
    }

    public override void Open(TicketContext context, string text)
    {
        Console.WriteLine($"Ticket #{context.TicketID} is closed! Reopening:");
        var newState = new DraftTicketState();
        newState.Open(context, text);
    }

    public override void Reply(TicketContext context, string text)
    {
        // Don't flash user email in real life, kids
        Console.WriteLine($"Ticket #{context.TicketID} was closed by user or system. You can contact user directly via email: {context.UserAddress}.\n");
    }
}