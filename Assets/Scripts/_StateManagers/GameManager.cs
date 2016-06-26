using UnityEngine;
using System.Collections;

namespace StateManager {
    public class GameManager : MonoBehaviour {
        public static GameManager instance = null;

        public enum GameState {
            Default,     // should never happen
            Alive,       // Game is running
            UIopen,      // (Ingame) UI is open
            GameOver,    // Player is kill
            GameStart,   // Game is booting up
            Menu,        // Ingame Menu open
            Options,     // Player is in Options Menu

        }

        public GameState state;

        // Singleton to have only one GameManager running at a time.
        void Awake() {
            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

        }

        // Update is called once per frame
        void Update() {
            switch (state) {
                case GameState.GameStart:
                    // Init();
                    break;
                case GameState.GameOver:
                    // GameOver();
                    break;

            }
        }
    }
}
