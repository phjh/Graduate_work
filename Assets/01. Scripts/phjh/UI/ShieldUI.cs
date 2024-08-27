using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    [SerializeField]
    List<Slider> shields;

    public void UpdateShieldInfo(float value)
    {
        SetUI(value);
    }

    private void SetUI(float value)
    {
        foreach (var shield in shields)
        {
            if (value <= 0)
            {
                shield.value = 0;
                continue;
            }

            shield.value = Mathf.Clamp01(value);
            value--;
        }
    }

}
