using Assets.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterFightScript : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody rigidBody;

        private FightSceneManager sceneManager;

        public int HP { get; private set; }
        public int Stamina { get; private set; }

        private bool isDead;

        private Dictionary<CharacterAction, List<KeyCode>> inputs;
        private Dictionary<KeyCode, CharacterAttack> attacks;
        private SimpleHealthBar hpBar;
        private SimpleHealthBar staminaBar;

        void Start()
        {
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();

            sceneManager = GameObject.Find(SceneObjects.SceneManager).GetComponent<FightSceneManager>();

            HP = HealthBar.MaxHP;
            Stamina = HealthBar.MaxStamina;

            inputs = sceneManager.GetCharacterInputs(gameObject.name);
            attacks = sceneManager.GetCharacterAttacks(gameObject.name);
            (hpBar, staminaBar) = sceneManager.GetCharacterHealthBar(gameObject.name);
        }

        void Update()
        {
            if (isDead)
                return;

            var pressedKey = SetAnimation();

            if (animator.GetBool(CharacterAction.Dodge.ToString()))
            {
                Dodge();
            }
            else if (animator.GetBool(CharacterAction.Attack.ToString()))
            {
                Attack(attacks[pressedKey.Value]);
            }

            if (HP <= 0 || Stamina <= 0)
                Die();
        }

        private KeyCode? SetAnimation()
        {
            KeyCode? pressedKey = null;

            foreach (var input in inputs)
            {
                var upKey = input.Value.FirstOrDefault(kc => Input.GetKeyUp(kc));
                animator.SetBool(input.Key.ToString(), upKey != default);

                if (upKey != default)
                    pressedKey = upKey;
            }

            return pressedKey;
        }

        private void Dodge()
        {
            rigidBody.AddForce(Vector3.up * 50f, ForceMode.Impulse);
        }

        private void Attack(CharacterAttack characterAttack)
        {
            if (sceneManager.IsGameOver || Stamina < characterAttack.NeededStamina)
                return;

            MakeAttack(characterAttack);

            Stamina -= characterAttack.NeededStamina;
            staminaBar.UpdateBar(Stamina, HealthBar.MaxStamina);
        }

        private void MakeAttack(CharacterAttack characterAttack)
        {
            var attack = Instantiate(GameObject.Find(characterAttack.Name));

            attack.transform.position = new Vector3(transform.position.x + 3f * characterAttack.Direction.x, 2f, transform.position.z);

            attack.GetComponent<CharacterAttackScript>().Init(gameObject.name, characterAttack.Damage, characterAttack.Direction);
        }

        private void Die()
        {
            isDead = true;
            animator.SetBool(CharacterAnimatorParameters.Death, true);

            sceneManager.CharacterDeath(gameObject.name);
        }

        public void GetHit(int damage)
        {
            HP -= damage;
            hpBar.UpdateBar(HP, HealthBar.MaxHP);
        }
    }
}