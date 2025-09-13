using UnityEngine;

public class MainGame : MonoBehaviour
{
    public static MainGame instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        MusicManager.Instance.PlayMusic("MainGame");
    }
}
