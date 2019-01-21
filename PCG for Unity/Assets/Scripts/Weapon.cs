using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public bool inPlayerInventory = false;

    private Player player;
    private WeaponComponents[] weaponComps;
    private bool weaponUsed = false;

    public void AquireWeapon()
    {
        player = GetComponentInParent<Player>();
        weaponComps = GetComponentsInChildren<WeaponComponents>();
    }

    private void Update()
    {
        if(inPlayerInventory)
        {
            transform.position = player.transform.position;
            if(weaponUsed)
            {
                float degreeY = 0, degreeZ = -90f, degreeMaxZ = 275;
                Vector3 returnVector = Vector3.zero;

                if(!player.isFacingRight)
                {
                    degreeY = 180;
                    returnVector = new Vector3(0, 180, 0);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degreeY, degreeZ), Time.deltaTime * 20f);
                if(transform.eulerAngles.z <degreeMaxZ)
                {
                    transform.eulerAngles = returnVector;
                    weaponUsed = false;
                    EnableSprtieRender(false);
                }
            }
        }
    }

    public void  UseWeapon()
    {
        EnableSprtieRender(true);
        weaponUsed = true;
    }

    public void EnableSprtieRender(bool isEnabeld)
    {
        foreach(WeaponComponents comp in weaponComps)
        {
            comp.GetSpriteRenderer().enabled = isEnabeld;
        }
    }

    public Sprite GetComponentImage(int index)
    {
        return weaponComps[index].GetSpriteRenderer().sprite;
    }
}
