using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunawayFromDead {
	public class MainMenuController : MonoBehaviour {
        public void RestartGame()
        {
            SceneManager.LoadScene("Show");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
