using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable
{
    public GameObject weaponPrefab;
    protected override void Interact()
    {
        PlayerMotor player = FindObjectOfType<PlayerMotor>();
        if (player != null)
        {
            player.EquipWeapon();
            Destroy(gameObject);
        }
    }
}
