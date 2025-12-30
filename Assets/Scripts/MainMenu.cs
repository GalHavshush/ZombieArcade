using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;
    string newGameScene = "Game";

    public AudioClip mainMenuMusic;
    public AudioSource mainMenuChannel;

    public void Start()
    {
        mainMenuChannel.PlayOneShot(mainMenuMusic);

        // Set the high score text
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";
    }

    public void StartNewGame()
    {
        mainMenuChannel.Stop();

        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }




}
