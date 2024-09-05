using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardInfo
{
    public int abilityID;
    public Sprite powerupImage;
    public string powerupName;
    public string powerupDescription;
}


public class PlayerAbilityCard : MonoBehaviour
{
    private int abilityID;
    [SerializeField]
    private Image powerupImage;
    [SerializeField]
    private TextMeshProUGUI powerupName;
    [SerializeField]
    private TextMeshProUGUI powerupDescription;

    public void SetCard(CardInfo info)
    {
        abilityID = info.abilityID;
        powerupImage.sprite = info.powerupImage;
        powerupName.text = info.powerupName;
        powerupDescription.text = info.powerupDescription;
    }

    public void SelectCard()
    {
        PlayerManager.Instance.PowerupSelectedAbility(abilityID);
    }

}
