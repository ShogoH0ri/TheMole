using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;

    void OnTriggerEnter(Collider other)
    {
        PlayerMotor player = other.GetComponent<PlayerMotor>();
        if (player != null && player.hasKey)
        {
            doorOpen = !doorOpen;
            door.GetComponent<Animator>().SetBool("IsOpen", true);
        }
    }
}
