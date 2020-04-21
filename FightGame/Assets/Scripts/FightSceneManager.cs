using Assets.Helpers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FightSceneManager : MonoBehaviour
    {
        private CharacterFightScript[] characters;

        private Text scoreText;

        public bool IsGameOver { get; private set; }
        private string winner;
        private int round;
        private int[] scores;

        private int popup = 0;

        PopupService popupService;

        void Start()
        {
            Physics.gravity = new Vector3(0f, -80f, 0f);

            characters = new CharacterFightScript[2];
            characters[0] = GameObject.Find(Characters.Witch).GetComponent<CharacterFightScript>();
            characters[1] = GameObject.Find(Characters.Warrior).GetComponent<CharacterFightScript>();

            scoreText = GameObject.Find(SceneObjects.FightSceneScore).GetComponent<Text>();

            InitRound();
            SetScoreText();

            if (round == 1)
                popup = 1;
            else
                EnableCharacters();

            popupService = new PopupService();
        }

        void OnGUI()
        {
            switch (popup)
            {
                case 1:
                    popupService.ShowPopup(
                        popup,
                        Resources.Load<TextAsset>(ResourceFiles.FightSceneInstructions).text,
                        new List<(string, System.Action)> { (PopupButtons.OK, StartGame) });
                    break;
                case 2:
                    popupService.ShowPopup(
                        popup,
                        FightSceneMessages.NextRoundMessage,
                        new List<(string, System.Action)> { (PopupButtons.NextRound, NextRound) });
                        break;
                case 3:
                    popupService.ShowPopup(
                        popup, 
                        FightSceneMessages.GameOverMessage(winner), 
                        new List<(string, System.Action)> { (PopupButtons.NextLevel, NextLevel) });
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

                for (var i = 0; i < characters.Length; i++)
                {
                    if (characters[i].name != characterName)
                    {
                        scores[i]++;
                        break;
                    }
                }

                for (var i = 0; i < scores.Length; i++)
                {
                    if (scores[i] == 2)
                    {
                        winner = characters[i].name;
                        break;
                    }
                }

                SetScoreText();

                if (!string.IsNullOrEmpty(winner))
                {
                    popup = 3;
                }
                else
                {
                    round++;
                    popup = 2;
                }
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

        private void InitRound()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKeys.FightSceneRound))
            {
                round = PlayerPrefs.GetInt(PlayerPrefsKeys.FightSceneRound);
                scores = new int[] { PlayerPrefs.GetInt(PlayerPrefsKeys.FightSceneWitchScore), PlayerPrefs.GetInt(PlayerPrefsKeys.FightSceneWarriorScore) };

                PlayerPrefs.DeleteAll();
            }
            else
            {
                round = 1;
                scores = new int[] { 0, 0 };
            }
        }

        private void SetScoreText()
        {
            scoreText.text = FightSceneMessages.ScoreTextMessage(round, scores[0], scores[1]);
        }

        private void StartGame()
        {
            EnableCharacters();

            popup = 0;
        }

        private void NextRound()
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.FightSceneRound, round);
            PlayerPrefs.SetInt(PlayerPrefsKeys.FightSceneWitchScore, scores[0]);
            PlayerPrefs.SetInt(PlayerPrefsKeys.FightSceneWarriorScore, scores[1]);

            GameManager.LoadScene(Scenes.FightScene);
        }

        private void NextLevel()
        {
            PlayerPrefs.SetString(PlayerPrefsKeys.FightSceneWinner, winner);
            GameManager.LoadScene(Scenes.TreasureScene);
        }

        private void EnableCharacters()
        {
            foreach (var character in characters)
                character.enabled = true;
        }
    }
}