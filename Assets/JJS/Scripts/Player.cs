namespace Jinsol
{
    using UnityEngine;

    // TODO: 개인적 테스트용 클래스. 프로젝트에서 사용 안함
    public class Player : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rigidbody.WakeUp();
        }
    }
}