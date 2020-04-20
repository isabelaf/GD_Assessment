using UnityEngine;

namespace Assets.Controllers
{
    public class ShurikenController : MonoBehaviour
    {
        private Rigidbody rigidBody;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(new Vector3(0, 500, 0) * Time.deltaTime));
        }
    }
}