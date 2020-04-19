using Assets.Helpers;
using UnityEngine;

namespace Assets.Controllers
{
    public class ChestController : MonoBehaviour
    {
        private Transform top;

        private bool isOpen = false;
        private bool isOpening = false;

        void Start()
        {
            top = transform.Find("top");
        }

        void Update()
        {
            if (isOpening)
            {
                top.Rotate(Vector3.right);

                if (top.rotation.eulerAngles.x >= 60)
                    isOpening = false;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!isOpen && collision.gameObject.CompareTag(Tags.Character))
            {
                isOpening = true;
                isOpen = true;
            }
        }
    }
}
