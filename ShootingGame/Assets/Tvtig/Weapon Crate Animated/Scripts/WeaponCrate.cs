using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponCrate : Interactable
{
    [SerializeField]
    private VisualEffect _visualEffect;

    private Animator _animator;

    [SerializeField]
    private GameObject Box;

    private bool boxOpen;

    [SerializeField]
    private Collider Cratecollider;


    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    protected override void Interact()
    {
        boxOpen = !boxOpen;
        Box.GetComponent<Animator>().SetBool("Open", boxOpen);
        Cratecollider.enabled = false;
        //SoundManager.Instance.PlaySound3D("OpeningDoor", Box.transform.position);
    }

    private void OnLidLifted()
    {
        _visualEffect.SendEvent("OnPlay");
    }
}
