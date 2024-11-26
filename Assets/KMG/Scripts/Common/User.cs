using UnityEngine;

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
            get
            {
                _credit = PlayerPrefs.GetInt("Credit", 0);
                return _credit;
            }
            set
            {
                _credit = value;
                PlayerPrefs.SetInt("Credit", _credit);
                UIManager.Instance.creditUI.SetText(_credit.ToString());
            }
        }
    }
}