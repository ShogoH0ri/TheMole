using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuMainGame : MonoBehaviour
{
    [SerializeField]
    public string ReloadLevel;

    public void Start()
    {
        MusicManager.Instance.PlayMusic("GameOver");
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(ReloadLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                  Application.Quit();//ゲームプレイ終了
#endif
    }
}
