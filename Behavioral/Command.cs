// Lazy implementation of commands, without Stack undo pool.

namespace GOF.Command
{

    public class GOFCommand
    {
        public void InvokeCommand()
        {
            MultiCommand multiCommand = new(new PrintCommand("Yolo"), new BeepCommand());
            multiCommand.Execute();
            multiCommand.UnExecute();

            multiCommand = new(new BeepCommand(), new PrintCommand("Hola"));
            multiCommand.Execute();
            multiCommand.UnExecute();
        }
    }


    public abstract class Command
    {
        public abstract void Execute();
        public abstract void UnExecute();
    }


    public class MultiCommand : Command
    {
        private Command[] commands;

        public MultiCommand(params Command[] commands)
        {
            this.commands = commands;
        }

        public override void Execute()
        {
            foreach (var c in commands)
            {
                c.Execute();
            }
        }

        public override void UnExecute()
        {
            for (int i = commands.Length - 1; i >= 0; i--)
            {
                commands[i].UnExecute();
            }
        }
    }


    public class PrintCommand : Command
    {
        string message;

        public PrintCommand(string message)
        {
            this.message = message;
        }

        public override void Execute()
        {
            Console.WriteLine(message);
        }

        public override void UnExecute()
        {
            Console.Clear();
        }
    }


    public class BeepCommand : Command
    {
        public override void Execute()
        {
            Console.Beep();
        }

        public override void UnExecute()
        {
            Console.WriteLine("UnBeeped!");
        }
    }

}