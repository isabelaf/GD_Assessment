using Assets.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class TreasureSceneManager : MonoBehaviour
    {
        private CharacterTreasureScript character;

        public bool IsGameOver { get; private set; }
        private bool? isWin;

        private int popup;

        void Start()
        {
            Physics.gravity = new Vector3(0f, -25f, 0f);

            InitCharacters();

            popup = 1;
        }

        void OnGUI()
        {
            switch (popup)
            {
                case 1:
                    PopupService.ShowConfirmationPopup(
                        1,
                        Resources.Load<TextAsset>(ResourceFiles.TreasureSceneInstructions).text,
                        new List<(string, System.Action)>{ (PopupButtons.OK, StartGame) });
                    break;
                case 2:
                    PopupService.ShowConfirmationPopup(
                        2,
                        $"{(isWin.Value ? TreasureSceneMessages.WinMessage : TreasureSceneMessages.LoseMessage)}\n{TreasureSceneMessages.GameOverMessage}",
                        new List<(string, System.Action)>
                        { 
                            (PopupButtons.Exit, () => { Application.Quit(0); }),
                            (PopupButtons.PlayAgain, () => { SceneManager.LoadScene(Scenes.FightScene, LoadSceneMode.Single); })
                        },
                        true);
                    break;
                default:
                    break;

            }
        }

        public void CharacterCollision(Collision collision)
        {
            if (IsGameOver)
                return;

            if (collision.gameObject.name == Characters.Enemy)
            {
                IsGameOver = true;
                character.Die();
                isWin = false;
            }

            if (collision.gameObject.name == SceneObjects.TreasureSceneTreasure)
            {
                IsGameOver = true;
                isWin = true;
            }

            if (IsGameOver)
                popup = 2;
        }

        private void InitCharacters()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.FightSceneWinner)) // testing case
            {
                GameObject.Find(Characters.Witch).SetActive(false);
                GameObject.Find(Characters.Enemy).SetActive(false);

                return;
            }

            gameObject.scene.GetRootGameObjects().First(go => go.name == PlayerPrefs.GetString(PlayerPrefsKeys.FightSceneWinner)).SetActive(true);
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.FightSceneWinner);
        }

        private void StartGame()
        {
            character = GameObject.FindGameObjectWithTag(Tags.Character).GetComponent<CharacterTreasureScript>();
            character.enabled = true;

            GameObject.Find(Characters.Enemy).GetComponent<EnemyScript>().enabled = true;

            var cameraScript = Camera.main.GetComponent<CameraLightScript>();
            cameraScript.transform.rotation = Quaternion.Euler(25, 0, 0);
            cameraScript.Target = character.transform;
            cameraScript.enabled = true;

            foreach (var light in gameObject.scene.GetRootGameObjects().Where(go => go.CompareTag(Tags.Light) && !go.activeSelf))
                light.SetActive(true);

            popup = 0;
        }
    }
}
