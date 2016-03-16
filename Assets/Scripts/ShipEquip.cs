using UnityEngine;
using System.Collections;

public class ShipEquip : MonoBehaviour {
    public Transform[] AuxJoint = new Transform[4];
    public Transform WeaponJoint1;
    public Transform WeaponJoint2;

    public void EquipPlayer()
    {
        Debug.Log("EquippingPlayer");
        PlayerStats stats = PlayerStats.instance;
        if (stats.wS1Name != "")
        {
            stats.weaponSlot1 = Instantiate(Resources.Load(stats.wS1Name)) as GameObject;
            stats.weaponSlot1.transform.position = WeaponJoint1.position;
            stats.weaponSlot1.transform.parent = transform;
            stats.weaponSlot1.GetComponent<IWeapon>().SetAmmoNum(stats.wS1Num);
        }

        if (stats.wS2Name != "")
        {
            stats.weaponSlot2 = Instantiate(Resources.Load(stats.wS2Name)) as GameObject;
            stats.weaponSlot2.transform.position = WeaponJoint2.position;
            stats.weaponSlot2.transform.parent = transform;
            stats.weaponSlot2.GetComponent<IWeapon>().SetAmmoNum(stats.wS2Num);
        }

        for (int i = 0; i < 4; i++)
            {
                if(stats.auxName[i] != "") 
                {
                    stats.auxSlot[i] = Instantiate(Resources.Load(stats.auxName[i])) as GameObject;
                    stats.auxSlot[i].transform.position = AuxJoint[i].position;
                    stats.auxSlot[i].transform.parent = transform;
                    stats.auxSlot[i].GetComponent<IWeapon>().SetAmmoNum(stats.auxNum[i]);
                }
                
            }
    }
}
