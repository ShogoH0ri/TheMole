using UnityEngine;

public class SampleScene : MonoBehaviour
{
    public static SampleScene instance;
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
        MusicManager.Instance.PlayMusic("Training");
    }
}
