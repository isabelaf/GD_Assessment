using Assets.Helpers;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyScript : MonoBehaviour
    {
        private Rigidbody rigidBody;

        private TreasureSceneManager sceneManager;

        private Transform target;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();

            sceneManager = GameObject.Find(SceneObjects.SceneManager).GetComponent<TreasureSceneManager>();
            target = GameObject.FindGameObjectWithTag(Tags.Character).transform;
        }

        void Update()
        {
            if (sceneManager.IsGameOver)
                return;

            Move();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<CharacterTreasureScript>() != null)
                rigidBody.isKinematic = true;
        }

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag(Tags.Obstacle))
                transform.Translate(Vector3.up * Time.deltaTime);
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.06f);
            transform.LookAt(target);
        }
    }
}
