using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BuggedUpCode
{
    /// <summary>
    ///      ** (25 points) ** 
    /// This script should handle a UI Main menu buttons for play and quit.
    /// When clicking the play button, the 'Game' scene should be loaded.
    /// When clicking the quit button, we close for the game to close - in editor we exit play mode.
    /// Fix the issues so that the buttons work correctly.
    /// Feel free to - Refactor the code. 
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playBt;
        [SerializeField] private Button quitBt;

        private void Start()
        {
            //playBt = GetComponent<Button>();
            //quitBt = GetComponent<Button>();
            playBt.onClick.AddListener(Play);
            quitBt.onClick.AddListener(Quit);
        }

        public void Play()
        {
            SceneManager.LoadScene(0);
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void OnApplicationQuit()
        {
            Debug.Log("Closing the game");
        }
    }
}
