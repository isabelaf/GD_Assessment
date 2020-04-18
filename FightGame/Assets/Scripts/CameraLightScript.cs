using UnityEngine;

namespace Assets.Scripts
{
    public class CameraLightScript : MonoBehaviour
    {
        public Transform Target { get; set; }

        [SerializeField]
        private Vector3 offsetPosition = Vector3.zero;

        [SerializeField]
        private Space offsetPositionSpace = Space.Self;

        [SerializeField]
        private bool lookAt = true;

        void Update()
        {
            if (offsetPositionSpace == Space.Self)
            {
                transform.position = Target.TransformPoint(offsetPosition);
            }
            else
            {
                transform.position = Target.position + offsetPosition;
            }

            if (lookAt)
            {
                transform.LookAt(Target);
            }
            else
            {
                transform.rotation = Target.rotation;
            }
        }
    }
}
