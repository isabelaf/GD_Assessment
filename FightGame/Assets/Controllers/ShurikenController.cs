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
            var deltaRotation = Quaternion.Euler(new Vector3(0, 500, 0) * Time.deltaTime);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
    }
}