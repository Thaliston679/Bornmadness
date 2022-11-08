using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot RightHandSlot;
        WeaponHolderSlot LeftHandSlot;

        private void Awake()
        {
            WeaponHolderSlot[] WeaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in WeaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    LeftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    RightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                LeftHandSlot.LoadWeaponModel(weaponItem);
            }
            else 
            {
                RightHandSlot.LoadWeaponModel(weaponItem);
            }
        }
    }
}
