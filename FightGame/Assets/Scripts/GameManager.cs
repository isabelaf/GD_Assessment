using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager
    {
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
