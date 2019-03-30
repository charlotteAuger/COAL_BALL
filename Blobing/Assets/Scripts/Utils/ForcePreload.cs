using UnityEngine;
using UnityEngine.SceneManagement;

namespace EmptyGame.Misc {
    public class ForcePreload : MonoBehaviour {
        private void Awake() {
#if UNITY_EDITOR
            if (FindObjectOfType<Preload>() == null)
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
#endif
        }
    }
}