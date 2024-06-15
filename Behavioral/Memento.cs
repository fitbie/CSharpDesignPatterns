namespace Patterns.GOF
{
    /// <summary>
    /// Memento pattern allows recording object's state without exposing its incapsulated fields.<br/>
    /// Usually it is implemented via 3 entities:
    /// <br/>1) Originator - source object we wish to record (snapshot). It is responsible for memento (snapshot) objet creating, which means it can pass its 
    ///    incapsulated state (i.e. private fields, props, etc.) as memento's ctor argument. In addition, originator takes memento object as argument to restore recorded state. 
    /// <br/>2) Memento - a.k.a. snapshot, object representing recorded state of originator. I strictly recommend to keep memento incapsulated too, and not forget about
    ///    reference types semantics, i.e. memento should be immutable during its lifetime. Also keep in mind that sometimes data which can be recorded by memento has lifetime.
    /// <br/>3) Caretaker - usually this is some "history recorder" object, which purpose is to store sequence of mementos in order to pass it to pass it to originator when needed.
    ///    Sometimes Caretaker implemented as separate Command-pattern object (like in my example), which is performing both call to change Originator state and call to record
    ///    its' memento. But as we already know - patterns may and should be implemented differently, depending on your problem.
    /// </summary>
    public class MementoPattern
    {
        public static void Start()
        {
            Warehouse wh = new();
            WarehousePutCommand command = new(wh);
            command.PutItem((1, 0));
            command.PutItem((1, 1));
            command.PutItem((1, 2));
            command.PutItem((1, 3));
            command.PutItem((0, 1));
            command.PutItem((3, 3));

            Console.WriteLine("Initial warehouse state");
            wh.PrintWarehouse();

            Console.WriteLine("After first undo (applying last memento object):");
            command.Undo();
            wh.PrintWarehouse();
            
            Console.WriteLine("After second undo:");
            command.Undo();
            wh.PrintWarehouse();
        }
    }

    public class Warehouse
    {
        private bool[,] occupancy = new bool[4,4];

        public void PutItem((int X, int Y) position)
        {
            // Some validate arguments code

            occupancy[position.X, position.Y] = true;
        }


        // Create new instance of memento (i.e. "recording" or "snapshotting" Originator state). 
        // Because this implemented by originator, it can pass its incapsulated state (private occupancy field in this case), without need to expose it as public API.
        public WarehouseMemento GetSnapshot()
        {
            WarehouseMemento warehouseMemento = new(occupancy);
            return warehouseMemento;
        }


        // Applying recorded previously memento (snapshot) to Originator. Here it is simply assign cloned memento's array.
        public void ApplySnapshot(WarehouseMemento snapshot)
        {
            occupancy = (bool[,])snapshot.occupancy.Clone(); // We perform cloning to prevent changes in the memento itself, because bool[,] is reference type.
            Console.WriteLine("Warehouse state was restored to {0}", snapshot.snapshotTime);
        }

        // Just to show values in the console
        public void PrintWarehouse()
        {
            for (int row = 0; row < occupancy.GetLength(0); row++)
            {
                Console.Write('|');
                for (int col = 0; col < occupancy.GetLength(1); col++)
                {
                    char symbol = occupancy[row, col] ? '*' : ' ';
                    Console.Write(symbol);
                    Console.Write('|');
                }
                Console.Write("\n");
            }
        }
    }


    public readonly struct WarehouseMemento
    {
        public readonly bool[,] occupancy;
        public readonly DateTime snapshotTime;

        public WarehouseMemento(bool[,] occupancy)
        {
            this.occupancy = (bool[,])occupancy.Clone(); // We perform cloning to prevent changes in the memento itself, because bool[,] is reference type.
            snapshotTime = DateTime.Now; // Just to inform sna
        }
    }


    // Example of so called "CareTaker" class, which purpose is to get, store, and then pass (if needed to restore) Memento object from and to Originator
    public class WarehousePutCommand
    {
        private Warehouse warehouse;
        Stack<WarehouseMemento> history;

        public WarehousePutCommand(Warehouse warehouse)
        {
            this.warehouse = warehouse;
            history = new();
        }

        public void PutItem((int X, int Y) position)
        {
            history.Push(warehouse.GetSnapshot());
            warehouse.PutItem(position);
        }

        public void Undo()
        {
            if (history.TryPop(out var memento))
            {
                warehouse.ApplySnapshot(memento);
            }
        }
    }
}