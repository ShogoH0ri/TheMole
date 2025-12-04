using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Interactable
{
    [SerializeField]
    private GameObject door;

    private bool doorOpen;

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);  //アニメーションでドアが開く処理
        SoundManager.Instance.PlaySound3D("OpeningDoor", door.transform.position);  //ドアが開く音
    }
}
