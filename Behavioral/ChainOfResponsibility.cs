using System.Reflection;
namespace GOF.COR;

public class ChainOfResponsibility
{
    public static void COR()
    {
        Application simpleApp = new();
        DialogueWindow dw = new(simpleApp);
        Button button = new(dw);

        IHelpHandler helpHandler = button;
        helpHandler.Handle("Button");
        helpHandler.Handle("DialogueWindow");
        helpHandler.Handle("InvalidInput");
    }
}


public class Application : IHelpHandler
{
    public IHelpHandler Successor => throw new NotImplementedException();

    public void Handle(string key)
    {
        Console.WriteLine($"SimpleApp, belongs to \"{Assembly.GetExecutingAssembly().FullName}\" assembly");
    }
}

public class DialogueWindow : Widget
{
    public DialogueWindow(IHelpHandler successor) : base(successor) {}

    public override void Handle(string key)
    {
        if (key == "DialogueWindow") { Console.WriteLine("This is a dialogue window"); }
        else Successor?.Handle(key);
    }
}

public class Button : Widget
{
    public Button(IHelpHandler successor) : base(successor) {}

    public override void Handle(string key)
    {
        if (key == "Button") { Console.WriteLine("This is a button"); }
        else Successor?.Handle(key);
    }
}

public abstract class Widget : IHelpHandler
{
    public Widget(IHelpHandler successor)
    {
        Successor = successor;
    }

    public IHelpHandler Successor { get; private set; }

    public abstract void Handle(string key);
}


public interface IHelpHandler
{
    public void Handle(string key);
    public IHelpHandler Successor { get; }
}