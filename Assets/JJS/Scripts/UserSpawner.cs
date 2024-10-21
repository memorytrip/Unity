namespace JJS.Test
{
    using UnityEngine;

    public class UserSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject userPrefab;

        private void Start()
        {
            Instantiate(userPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}