using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;

namespace EmptyGame.Misc
{
    public class Preload : SingletonMB<Preload>
    {
        private void Awake()
        {
            LaunchGame();
        }

        private void LaunchGame()
        {
            GameAnalytics.Initialize();

            if (SceneManager.sceneCount == 1)
                SceneManager.LoadScene(1);
        }
    }
}
