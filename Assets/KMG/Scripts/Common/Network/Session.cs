namespace Common.Network
{
    /**
     * TODO: 세션 정보 (접속상태, 유저 정보, 토큰 등) 저장하기
     */
    public class Session
    {
        public enum State
        {
            Connect,
            Disconnect,
        }

        public State state;
        public User user;
    }
}