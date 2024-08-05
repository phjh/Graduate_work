using UnityEngine;

[System.Serializable]
public enum WeaponEnum
{
    None = -1,
	Pickaxe = 0,
    Drill = 1,
	Excavator = 2,
    End
}

[CreateAssetMenu(fileName = "New WeaponData", menuName = "SO/Data/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Weapon Data Values")]
    public WeaponEnum weapon;
    public Sprite WeaponIcon;
	public float damageFactor;
	public PoolableMono bullet;
    [Header("Weapon Visual")]
    public PlayerWeapon playerWeapon;
}
