using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject audioManagerPrefab;
    [SerializeField] private string firstSceneName = "MainMenu"; // Å‰‚É‘JˆÚ‚·‚éƒV[ƒ“–¼

    private void Awake()
    {
        // ‚·‚Å‚É‘¶İ‚µ‚Ä‚¢‚½‚ç‰½‚à‚µ‚È‚¢
        if (GameObject.FindWithTag("AudioManager") == null)
        {
            var am = Instantiate(audioManagerPrefab);
            am.name = audioManagerPrefab.name; // (Clone)–h~
            DontDestroyOnLoad(am);
        }
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(firstSceneName))
        {
            SceneManager.LoadSceneAsync(firstSceneName);
        }
    }
}
