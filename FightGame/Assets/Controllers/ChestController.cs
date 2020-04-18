using Assets.Helpers;
using UnityEngine;

namespace Assets.Controllers
{
    public class ChestController : MonoBehaviour
    {
        private bool isOpen = false;

        private void OnCollisionEnter(Collision collision)
        {
            if (!isOpen && collision.gameObject.CompareTag(Tags.Character))
            {
                isOpen = true;
                transform.Find("top").transform.Rotate(new Vector3(60, 0, 0));
            }
        }
    }
}
