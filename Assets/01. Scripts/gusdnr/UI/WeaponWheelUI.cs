using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelUI : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponDataSO ThisWheelData;

    [Header("Wheel Values")]
    public Image WeaponIconImage;
    public Color DefaultColor;
    public Color SelectedColor;
    
    private Button WheelBtn; 
    private Image WheelImage;

    private Managers mngs;

    private bool alreadySelected = false;

	private void OnEnable()
	{
		if (WheelBtn == null) TryGetComponent(out WheelBtn);
        if (WheelImage == null) TryGetComponent(out WheelImage);

		mngs = Managers.GetInstance();

		alreadySelected = false;
		WeaponIconImage.sprite = this.ThisWheelData.WeaponIcon;
	}

	public void ActiveWheel(bool isActives)
    {
		WheelBtn.interactable = isActives;
        if(isActives == false) return;

        if(alreadySelected == true) WheelImage.color = SelectedColor;
        else if(alreadySelected == false) WheelImage.color = DefaultColor;
	}

    public void SelectThisWeapon()
    {
        mngs?.PlayerMng.SetPlayerWeapon(ThisWheelData);
    }

    public void Selected()
    {
        alreadySelected = true;
    }

    public void Deselected()
    {
        alreadySelected = false;
    }
}
