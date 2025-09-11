using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerBox : MonoBehaviour
{
    public GameObject MessageUI;

    private void Start()
    {
        MessageUI.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MessageUI.SetActive(true);
            StartCoroutine(WaitForSec());
        }
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        MessageUI.SetActive(false);
        //Destroy(MessageUI);
        //Destroy(gameObject);
    }
}
