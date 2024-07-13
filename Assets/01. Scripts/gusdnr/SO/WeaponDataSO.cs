using UnityEngine;

[System.Serializable]
public enum WeaponEnum
{
    None = -1,
	Pickax = 0,
    Drill = 1,
	Excavator = 2,
    End
}

[CreateAssetMenu(fileName = "New WeaponData", menuName = "SO/Data/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public WeaponEnum weapon;
    public Sprite WeaponIcon;
}
