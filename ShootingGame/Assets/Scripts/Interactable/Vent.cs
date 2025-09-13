using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : Interactable
{
    [SerializeField]
    private GameObject vent;

    private bool ventOpen;

    [SerializeField]
    private Collider Boxcollider;

    protected override void Interact()
    {
        ventOpen = !ventOpen;
        vent.GetComponent<Animator>().SetBool("OpenVent", ventOpen);
        Boxcollider.enabled = false;
        SoundManager.Instance.PlaySound3D("OpeningDoor", vent.transform.position);
    }
}
