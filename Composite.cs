namespace GOF
{
    public class CompositeGOF
    {
        public void Composite()
        {
            Node root = new CompositeNode(null, new Leaf(10), new Leaf(15), new Leaf(111));
            root.Print();
            root.GetChild(2)?.Print();
            root.GetChild(5)?.Print();
        }
    }


    public abstract class Node
    {
        public int? Value { get; init; }

        public Node(int? value)
        {
            Value = value;
        }

        public virtual void Print() => Console.WriteLine(Value);

        public virtual bool AddChild(Node node) => false;
        public virtual bool RemoveChild(int index) => false;
        public virtual Node? GetChild(int index) => null;
    }


    public class CompositeNode : Node
    {
        protected List<Node> children;

        public CompositeNode(int? value, params Node[] nodes) : base(value)
        {
            children = new();
            foreach (var node in nodes) { AddChild(node); }
        }

        public override void Print()
        {
            foreach (var node in children) { node.Print(); }
        }

        public override bool AddChild(Node node)
        {
            children.Add(node);
            return true;
        }

        public override Node? GetChild(int index)
        {
            return index >= 0 && index < children.Count ? 
            children[index] :
            null;
        }

        public override bool RemoveChild(int index)
        {
            return children.Remove(children[index]);
        }
    }


    public class Leaf : Node
    {
        public Leaf(int? value) : base(value) {}
    }

}