using Assets.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterTreasureScript : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody rigidBody;

        private TreasureSceneManager sceneManager;

        private bool isDead;

        private Dictionary<KeyCode, Vector3> rotations;
        private Dictionary<KeyCode, int> movements;
        private readonly float movementSpeed = 5f;

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (isDead)
                return;

            Rotate();
            Move();
            Jump();
        }

        void OnCollisionEnter(Collision collision)
        {
            sceneManager.CharacterCollision(collision);
        }

        public void Die()
        {
            isDead = true;
            animator.SetBool(CharacterAnimatorParameters.Death, true);
        }

        private void Init()
        {
            animator = gameObject.GetComponent<Animator>();
            rigidBody = gameObject.GetComponent<Rigidbody>();

            sceneManager = GameObject.Find(SceneObjects.SceneManager).GetComponent<TreasureSceneManager>();

            rotations = new Dictionary<KeyCode, Vector3>
            {
                { KeyCode.RightArrow, Vector3.up },
                { KeyCode.LeftArrow, Vector3.down }
            };

            movements = new Dictionary<KeyCode, int>
            {
                { KeyCode.UpArrow, 1 },
                { KeyCode.DownArrow, -1 }
            };
        }

        private void Rotate()
        {
            foreach (var rotation in rotations)
            {
                if (Input.GetKey(rotation.Key))
                {
                    transform.Rotate(rotation.Value * movementSpeed, Space.World);
                    return;
                }

            }
        }

        private void Move()
        {
            foreach (var movement in movements)
            {
                if (Input.GetKey(movement.Key))
                {
                    rigidBody.velocity = movement.Value * transform.forward * movementSpeed;
                    animator.SetBool(CharacterAnimatorParameters.Movement, true);

                    return;
                }
            }

            animator.SetBool(CharacterAnimatorParameters.Movement, false);
        }

        private void Jump()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                rigidBody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            }
        }
    }
}
