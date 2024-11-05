using UnityEngine;

namespace ClassNature {
    public class Mover : MonoBehaviour {

        [SerializeField] float moveSpeed;
        [SerializeField] bool x;
        [SerializeField] bool y;
        [SerializeField] bool z;

        void LateUpdate() {
            transform.position = new Vector3(
                x ? transform.position.x + moveSpeed * Time.deltaTime : transform.position.x,
                y ? transform.position.y + moveSpeed * Time.deltaTime : transform.position.y,
                z ? transform.position.z + moveSpeed * Time.deltaTime : transform.position.z);
        }
    }
}
