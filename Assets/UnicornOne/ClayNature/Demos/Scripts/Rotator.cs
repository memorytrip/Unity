using UnityEngine;

namespace ClassNature {
    public class Rotator : MonoBehaviour {
        [SerializeField] float rotationSpeed;
        [SerializeField] bool x;
        [SerializeField] bool y;
        [SerializeField] bool z;

        void Update() {
            transform.Rotate(
                x ? rotationSpeed * Time.deltaTime : 0,
                y ? rotationSpeed * Time.deltaTime : 0,
                z ? rotationSpeed * Time.deltaTime : 0);
        }
    }
}
