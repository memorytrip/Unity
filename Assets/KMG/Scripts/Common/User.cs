using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Common
{
    public class User
    {
        public string nickName;
        public string email;
        public string avartarId;
        private int _credit;
        
        public async UniTaskVoid RefreshCredit()
        {
            string response = await DataManager.Get($"/api/credit/{email}");
            CreditDTO credit = JsonConvert.DeserializeObject<CreditDTO>(response);
            _credit = credit.credit;
            UIManager.Instance.creditUI.SetText(_credit.ToString());
        }

        public async UniTaskVoid AddCredit(int amount)
        {
            CreditDTO credit = new CreditDTO();
            credit.email = email;
            credit.credit = amount;
            string request = JsonConvert.SerializeObject(credit);
            await DataManager.Post($"/api/credit/add", request);
            RefreshCredit().Forget();
        }
    }

    class CreditDTO
    {
        public string email;
        public int credit;
    }
}