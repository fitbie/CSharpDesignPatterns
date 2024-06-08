namespace GOF.Decorator
{
    public class DecoratorGOF
    {
        public static void Decorator()
        {
            NodeDecorator decorator = new(new ActionNode());
            decorator.Execute();

            decorator.SetAttached(new ConditionNode());
            decorator.Execute();
        }
    }


    public abstract class Node
    {
        public Node() {}

        public abstract void Execute();
    }


    public class NodeDecorator(Node attached) : Node
    {
        public void SetAttached(Node node)
        {
            attached = node;
        }

        public override void Execute()
        {
            attached.Execute();
            Console.Write(" from decorator \n");
        }
    }


    public class ActionNode : Node
    {
        public override void Execute()
        {
            Console.WriteLine("Action");
        }
    }


    public class ConditionNode : Node
    {
        public override void Execute()
        {
            Console.WriteLine("Condition");
        }
    }

}