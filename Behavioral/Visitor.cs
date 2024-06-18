namespace Patterns.GOF;

/// <summary>
/// Represent an operation to be performed on instances of a set of classes. 
/// Visitor lets a new operation be defined without changing the classes of the elements on which it operates.
/// <para>
/// Suppose we want to add additional behavior to the, say, <c>Client</c> class.
/// To achieve this, the Visitor class is introduced, which defines "extensible" behavior in the <c>Visit(Client)</c> method. 
/// <br/>The <c>Accept(Visitor v)</c> method is added to the extensible Client class, which ends up as call to  mentioned <c>Visitor.Visit(Client)</c> 
/// and passes <see langword="this"/> (i.e. itself) as an argument.
/// </para>
/// <para>
/// The question is - why do we need to extend the Client interface with the <c>Accept(Visitor v)</c> method, if we already create an instance of the visitor, 
/// and therefore we can pass the client to it. The answer is simple - polymorphism and dispatching.
/// <br/>Let's look at my example below - we have classes <see cref="Client"/> and derived from it <see cref="BusinessClient"/>. 
/// There is <see cref="ClientVisitor"/> with <see cref="ClientVisitor.Visit(Client)"/> and <see cref="ClientVisitor.Visit(BusinessClient)"/> methods.
/// Imagine if we're initializing Client local variable, but assigning instance of BusinessClient there. If we pass it to ClientVisitor instance directly - what version
/// of Visitor.Visit method will be called? Of course, <see cref="ClientVisitor.Visit(Client)"/>, because dispatching here determines by type of local variable, not the
/// actual type of object assigned to it. 
/// <br/>Therefore, we need a way to call proper overload of method - that is why we're adding <see cref="Client.Accept(ClientVisitor)"/> method, inside it each client will
/// pass itself to the visitor, e.g. BusinessClient will override (<see cref="BusinessClient.Accept(ClientVisitor)"/>) method and pass itself too, but as BusinessClient,
/// which leads to proper call dispatching to the <see cref="ClientVisitor.Visit(BusinessClient)"/>.
/// </para>
/// </summary>
public class VisitorPattern
{
    public static void Start()
    {
        Client client; // Thanks to the OOP we can assign any Client or more derived type of object here
        ClientIDVisitor clientVisitor = new();

        client = new Client(11);
        client.Accept(clientVisitor); // Will result in "Visitor.Visit(Client client)" call, that may perform any extensive logic without need in changing Client's interface

        client = new BusinessClient(15, 3);
        client.Accept(clientVisitor); // will result in "Visit(BusinessClient businessClient)" call thanks to BusinessClient's Accept override

        // If we're tried to to this instead of Accept, it'll always ends up as ClientVisitor.Visit(Client client), even if it is BusinessClient
        clientVisitor.Visit(client);
     }
}


// Client class we're gonna to extend with Visitor
public class Client
{
    public int ID { get; init; }

    public Client(int id) => ID = id;
    public virtual void Accept(ClientVisitor visitor) => visitor.Visit(this); // Dispatched into Client overload call
}


// Derived client to show polymorphism
public class BusinessClient : Client
{
    public int BusinessID { get; init; }

    public BusinessClient(int id, int businessId) : base(id) => BusinessID = businessId;
    public override void Accept(ClientVisitor visitor) => visitor.Visit(this); // Dispatched into BusinessClient overload call
}


// Base Visitor class, extending Client.
public abstract class ClientVisitor
{
    public abstract void Visit(Client client);
    public abstract void Visit(BusinessClient businessClient);
}


// Visitor class, writing Client's data in console, data depends on Client type, so clients need call dispatching to right overload, which is performed in the Accept.
public class ClientIDVisitor : ClientVisitor
{
    public override void Visit(Client client)
    {
        Console.WriteLine($"Client's ID: {client.ID}");
    }

    public override void Visit(BusinessClient businessClient)
    {
        Console.WriteLine($"Business client's ID: {businessClient.ID}, Business ID: {businessClient.BusinessID}");
    }
}