using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUI : MonoBehaviour
{
	public bool isActive() { return gameObject.activeSelf; }

    public abstract void InitializePopup();

    public virtual void TogglePopup(bool value)
    {
		if (gameObject.activeSelf == value)
		{
			return;
		}

		gameObject.SetActive(value);

		if (value)
		{
			InitializePopup();
		}
	}

    public virtual void ActivatePopup()
    {
		if(isActive()) { return; }
		gameObject.SetActive(true);
		InitializePopup();
    }

    public virtual void DeactivatePopup()
    {
		if(!isActive()) { return; }
		gameObject.SetActive(false);
	}
}
