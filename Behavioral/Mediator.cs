namespace Patterns.GOF
{
    public class MediatorPattern
    {
        public static void Start()
        {
            AuthDialogWindow authDialogWindow = new();
            
            // Enable other UI elements
            authDialogWindow.showElements.Press();

            // Type username
            authDialogWindow.textField.Type("username1234");

            // Enable input field's secret mode
            authDialogWindow.hideInputText.Check();

            // Type password
            authDialogWindow.textField.Type("myPassword");
        }
    }


    // Mediator interface
    public interface IDialogMediator
    {
        public void OnUIEvent(UIElement sender, EventData eventData);
    }


    // UI base of UI elements that'll communicate via Mediator
    public abstract class UIElement
    {
        public bool visible;
        protected IDialogMediator mediator;

        public UIElement(IDialogMediator mediator, bool visible)
        {
            this.mediator = mediator;
            this.visible = visible;
        }
    }


    // Mediator implementation. Like in many other examples, acts as mediator and UI components "parent container" at the same time (because it is really handy).
    // But of course could be completely separate and communication-only object
    public class AuthDialogWindow : IDialogMediator
    {
        public Button showElements;
        public Checkbox hideInputText;
        public TextField textField;


        // Create dialog window's sub-elements 
        public AuthDialogWindow()
        {
            showElements = new(this, visible: true);
            hideInputText = new(this, initialState: false, visible: false);
            textField = new(this, visible: false);
        }


        public void OnUIEvent(UIElement sender, EventData eventData)
        {
            switch (eventData.eventType)
            {
                case EventData.EventType.Press:
                    hideInputText.visible = true;
                    textField.visible = true;
                    break;

                case EventData.EventType.Check:
                    textField.secretMode = eventData.boolValue;
                    break;

                case EventData.EventType.Type:
                    Console.WriteLine(eventData.textValue);
                    break;

            }
        }
    }


    // Couple of simple UI components implementations
    public class Button : UIElement
    {
        public Button(IDialogMediator mediator, bool visible) : base(mediator, visible) {}

        public void Press()
        {
            if (!visible) return;

            mediator.OnUIEvent(this, new EventData(EventData.EventType.Press));
        }
    }


    public class Checkbox : UIElement
    {
        private bool state;

        public Checkbox(IDialogMediator mediator, bool initialState, bool visible) : base(mediator, visible) => state = initialState;

        public void Check()
        {
            if (!visible) return;

            state = !state;
            mediator.OnUIEvent(this, new(EventData.EventType.Check, state));
        }
    }


    public class TextField : UIElement
    {
        public bool secretMode;

        public TextField(IDialogMediator mediator, bool visible) : base(mediator, visible) {}

        public void Type(string consoleInput)
        {
            if (!visible) return;

            string message = secretMode ? string.Concat(consoleInput.Select(_ => '*')) : consoleInput;
            EventData eventData = new(EventData.EventType.Type, textValue: message);
            mediator.OnUIEvent(this, eventData);
        }
    }


    // Determines type of UI interaction and its data, just to make example more down-to-earth, not related to the pattern
    public readonly struct EventData
    {
        public enum EventType { Check, Type, Press }
        public readonly EventType eventType;
        public readonly bool boolValue;
        public readonly string? textValue;

        public EventData(EventType eventType, bool boolValue = false, string? textValue = null)
        {
            this.eventType = eventType;
            this.boolValue = boolValue;
            this.textValue = textValue;
        }
    }
}