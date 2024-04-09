namespace GOF.Visitor
{
    public class VisitorGOF
    {
        public void Visitor()
        {
            Client client = new(11);
            BusinessClient business = new(15, 3);
            ClientIDVisitor clientIDVisitor = new();

            client.Accept(clientIDVisitor);
            business.Accept(clientIDVisitor);
        }
    }


    public class Client
    {
        public int ID { get; init; }

        public Client(int id) => ID = id;

        public virtual void Accept(ClientVisitor visitor) => visitor.Visit(this);
    }


    public class BusinessClient : Client
    {
        public int BusinessID { get; init; }

        public BusinessClient(int id, int businessId) : base(id) => BusinessID = businessId;

        public override void Accept(ClientVisitor visitor) => visitor.Visit(this);
    }


    public abstract class ClientVisitor
    {
        public abstract void Visit(Client client);
        public abstract void Visit(BusinessClient businessClient);
    }


    public class ClientIDVisitor : ClientVisitor
    {
        public override void Visit(Client client)
        {
            Console.WriteLine($"Client's ID: {client.ID}");
        }

        public override void Visit(BusinessClient businessClient)
        {
            Console.WriteLine($"Business client's ID: {businessClient.ID}, business id: {businessClient.BusinessID}");
        }
    }

}