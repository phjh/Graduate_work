using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelUI : MonoBehaviour
{
    [Header("Wheel Values")]
    [SerializeField] private Sprite WeaponSprite;
    public Image WeaponImage;
    public static int weaponID;

    private bool alreadySelected = false;

    public void Selected()
    {
        alreadySelected = true;
    }

    public void Deselected()
    {
        alreadySelected = false;
    }
}
