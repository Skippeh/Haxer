namespace ServerCSharp
{
    public abstract class Packet
    {
        public string[] RequiredParameters { get; private set; }
        protected Server Server { get; private set; }

        protected Packet(Server server, params string[] requiredParameters)
        {
            Server = server;
            RequiredParameters = requiredParameters.Length > 0 ? requiredParameters : null;
        }

        public abstract void Handle(Client client, Message message);
    }
}