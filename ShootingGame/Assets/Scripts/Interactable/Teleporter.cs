using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private string SceneName;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ChangeScene();
        }
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
