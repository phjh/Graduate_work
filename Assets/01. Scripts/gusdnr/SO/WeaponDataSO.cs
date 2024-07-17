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
    [Header("Lobby Values")]
    public WeaponEnum weapon;
    public Sprite WeaponIcon;

    [Header("InGame Values")]
	public float damageFactor;
	public PoolableMono bullet;

    public GameObject Weapon;
    public Transform leftHandTrm;
    public Transform rightHandTrm;



}
