namespace BlackJack
{
    public class FrontendEvent
    {
        public string eventName { get; }
        public String[] args { get; }

        public FrontendEvent(String eventname, params String[] args )
        {
            this.eventName = eventname;
            this.args = args;   
        }

    }
}
