using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    public GameObject keyPrefab;

    void Update()
    {
        transform.Rotate(0,0.5f,0);
    }
    protected override void Interact()
    {
        PlayerMotor player = FindObjectOfType<PlayerMotor>();
        if (player != null)
        {
            player.EquipKey();
            Destroy(gameObject);
        }
    }
}
