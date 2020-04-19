using Assets.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class FightSceneManager : MonoBehaviour
    {
        private CharacterFightScript[] characters;

        public bool IsGameOver { get; private set; }
        private string loser;

        private int popup;

        void Start()
        {
            Physics.gravity = new Vector3(0f, -80f, 0f);

            characters = new CharacterFightScript[2];
            characters[0] = GameObject.Find(Characters.Witch).GetComponent<CharacterFightScript>();
            characters[1] = GameObject.Find(Characters.Warrior).GetComponent<CharacterFightScript>();

            popup = 1;
        }

        void OnGUI()
        {
            switch (popup)
            {
                case 1:
                    PopupService.ShowConfirmationPopup(
                        1,
                        Resources.Load<TextAsset>(ResourceFiles.FightSceneInstructions).text,
                        new List<(string, System.Action)> { (PopupButtons.OK, StartGame) });
                    break;
                case 2:
                    PopupService.ShowConfirmationPopup(
                        2, 
                        FightSceneMessages.GameOverMessage(characters.First(c => c.name != loser).name), 
                        new List<(string, System.Action)> { (PopupButtons.NextLevel, ContinueGame) },
                        true);
                    break;
                default:
                    break;

            }
        }

        public Dictionary<CharacterAction, List<KeyCode>> GetCharacterInputs(string characterName)
        {
            var inputs = new Dictionary<CharacterAction, List<KeyCode>>();
            switch (characterName)
            {
                case Characters.Witch:
                    inputs.Add(CharacterAction.Dodge, new List<KeyCode> { KeyCode.W });
                    inputs.Add(CharacterAction.Attack, GetCharacterAttackKeyCodes(characterName));
                    break;
                case Characters.Warrior:
                    inputs.Add(CharacterAction.Dodge, new List<KeyCode> { KeyCode.UpArrow });
                    inputs.Add(CharacterAction.Attack, GetCharacterAttackKeyCodes(characterName));
                    break;
                default:
                    break;
            }

            return inputs;
        }

        public (SimpleHealthBar, SimpleHealthBar) GetCharacterHealthBar(string characterName)
        {
            var characterHealthBar = GameObject.Find(HealthBar.HealthBarName(characterName.Split('_')[1])).transform;
            return (characterHealthBar.Find(HealthBar.HPHealthBar).GetComponent<SimpleHealthBar>(), characterHealthBar.Find(HealthBar.StaminaHealthBar).GetComponent<SimpleHealthBar>());
        }

        public Dictionary<KeyCode, CharacterAttack> GetCharacterAttacks(string characterName)
        {
            var attackDirection = GetCharacterAttackDirection(characterName);
            var keyCodes = GetCharacterAttackKeyCodes(characterName);

            return new Dictionary<KeyCode, CharacterAttack>
        {
            {
                keyCodes[0],
                new CharacterAttack
                {
                    Name = CharacterAttacks.Magic,
                    Damage = 25,
                    Direction = attackDirection,
                    NeededStamina = 20
                }
            },
            {
                keyCodes[1],
                new CharacterAttack
                {
                    Name = CharacterAttacks.Shuriken,
                    Damage = 15,
                    Direction = attackDirection,
                    NeededStamina = 5
                }
            }
        };
        }

        public void CharacterDeath(string characterName)
        {
            if (!IsGameOver)
            {
                IsGameOver = true;
                loser = characterName;
                popup = 2;
            }
        }

        private Vector3 GetCharacterAttackDirection(string characterName)
        {
            var attackDirection = new Vector3();

            switch (characterName)
            {
                case Characters.Witch:
                    attackDirection = Vector3.left;
                    break;
                case Characters.Warrior:
                    attackDirection = Vector3.right;
                    break;
                default:
                    break;
            }

            return attackDirection;
        }

        private List<KeyCode> GetCharacterAttackKeyCodes(string characterName)
        {
            switch (characterName)
            {
                case Characters.Witch:
                    return new List<KeyCode> { KeyCode.A, KeyCode.S };
                case Characters.Warrior:
                    return new List<KeyCode> { KeyCode.LeftArrow, KeyCode.DownArrow };
                default:
                    return new List<KeyCode>();
            }
        }

        private void StartGame()
        {
            foreach (var character in characters)
                character.enabled = true;

            popup = 0;
        }

        private void ContinueGame()
        {
            PlayerPrefs.SetString(PlayerPrefsKeys.FightSceneLoser, loser);
            SceneManager.LoadScene(Scenes.TreasureScene, LoadSceneMode.Single);
        }
    }
}