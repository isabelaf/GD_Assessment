using Assets.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TreasureSceneManager : MonoBehaviour
    {
        private CharacterTreasureScript character;

        public bool IsGameOver { get; private set; }

        void Start()
        {
            Physics.gravity = new Vector3(0f, -25f, 0f);

            InitCharacters();

            var characterGameObject = GameObject.FindGameObjectWithTag(Tags.Character);

            character = characterGameObject.GetComponent<CharacterTreasureScript>();

            Camera.main.GetComponent<CameraLightScript>().Target = characterGameObject.transform;
            Light.GetLights(LightType.Directional, 0)[0].GetComponent<CameraLightScript>().Target = characterGameObject.transform;
        }

        void Update()
        {
            if (IsGameOver)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    SceneManager.LoadScene(Scenes.FightScene, LoadSceneMode.Single);
                }

                if (Input.GetKeyUp(KeyCode.Escape))
                    Application.Quit(0);
            }
        }

        public void CharacterCollision(Collision collision)
        {
            if (IsGameOver)
                return;

            bool isWin = false;

            if (collision.gameObject.GetComponent<EnemyScript>() != null)
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
                ShowGameOverMessage(isWin);
        }

        private void InitCharacters()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.FightSceneLoser)) // testing case
            {
                GameObject.Find(Characters.Witch).SetActive(false);
                GameObject.Find(Characters.Enemy).SetActive(false);

                return;
            }

            GameObject.Find(PlayerPrefs.GetString(PlayerPrefsKeys.FightSceneLoser)).SetActive(false);
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.FightSceneLoser);
        }

        private void ShowGameOverMessage(bool isWin)
        {
            GameObject.Find(SceneObjects.GameOverText).GetComponent<Text>().text = $"{(isWin ? TreasureSceneMessages.WinMessage : TreasureSceneMessages.LoseMessage)}\n{TreasureSceneMessages.GameOverMessage}"; ;
        }
    }
}
