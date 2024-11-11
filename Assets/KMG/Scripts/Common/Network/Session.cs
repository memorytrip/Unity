namespace Common.Network
{
    public class Session
    {
        public enum State
        {
            Connect,
            Disconnect,
        }

        public State state;
        public User user;
        public string token;
    }
}