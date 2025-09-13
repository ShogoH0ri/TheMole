using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject audioManagerPrefab;
    [SerializeField] private string firstSceneName = "MainMenu"; // �ŏ��ɑJ�ڂ���V�[����

    private void Awake()
    {
        // ���łɑ��݂��Ă����牽�����Ȃ�
        if (GameObject.FindWithTag("AudioManager") == null)
        {
            var am = Instantiate(audioManagerPrefab);
            am.name = audioManagerPrefab.name; // (Clone)�h�~
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
