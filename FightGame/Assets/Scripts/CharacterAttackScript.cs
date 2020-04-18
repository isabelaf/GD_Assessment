using UnityEngine;

namespace Assets.Scripts
{
    class CharacterAttackScript : MonoBehaviour
    {
        public string Attacker { get; set; }
        public int Damage { get; set; }
        public Vector3 Direction { get; set; }

        private Rigidbody rigidBody;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            rigidBody.AddForce(50 * Direction);
        }

        void OnCollisionEnter(Collision collision)
        {
            var characterCollision = collision.gameObject.GetComponent<CharacterFightScript>();
            if (characterCollision != null)
            {
                characterCollision.GetHit(Damage);

                Destroy(gameObject);
            }

            var characterAttackCollision = collision.gameObject.GetComponent<CharacterAttackScript>();
            if (characterAttackCollision != null && characterAttackCollision.Attacker != Attacker)
            {
                if (Damage <= characterAttackCollision.Damage)
                    Destroy(gameObject);
                else if (Damage > characterAttackCollision.Damage)
                    Damage -= characterAttackCollision.Damage;
            }
        }

        public void Init(string attacker, int damage, Vector3 direction)
        {
            Attacker = attacker;
            Damage = damage;
            Direction = direction;
        }
    }
}
