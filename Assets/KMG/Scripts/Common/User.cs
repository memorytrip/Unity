namespace Common
{
    public class User
    {
        public string nickName;
        public string email;
        public string avartarId;
        private int _credit;
        public int credit
        {
            get => _credit;
            set
            {
                _credit = value;
                UIManager.Instance.creditUI.SetText(_credit.ToString());
            }
        }
    }
}