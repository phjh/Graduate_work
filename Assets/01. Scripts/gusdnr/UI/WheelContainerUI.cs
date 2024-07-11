using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelContainerUI : MonoBehaviour
{
	[Header("Wheel Container Value")]
	[SerializeField] private float ActiveDuration = 0.5f;
	[SerializeField] private float MinPosX;
	[SerializeField] private float MaxPosX;
	[SerializeField] private List<Button> WheelBtnElements = new List<Button>();

	private RectTransform containerTrm;

	private bool alreadyActiveWheel = false;
	private bool alreadyWorkingActive = false;

	private void Awake()
	{
		alreadyActiveWheel = false;
		alreadyWorkingActive = false;

		TryGetComponent(out containerTrm);

		MinPosX = containerTrm.position.x; 

		WheelBtnElements.ForEach(btn => btn.interactable = alreadyActiveWheel);
	}

	public void ActiveWeaponWheel()
	{
		if (alreadyWorkingActive == true) return;

		if (containerTrm == null) TryGetComponent(out containerTrm);
		
		containerTrm.DOAnchorPos3DX(alreadyActiveWheel ? MinPosX : MaxPosX, ActiveDuration)
				.OnStart(() =>
				{
					alreadyWorkingActive = true;

					alreadyActiveWheel = !alreadyActiveWheel;
					WheelBtnElements.ForEach(btn => btn.interactable = alreadyActiveWheel);
				})
				.OnComplete(() =>
				{
					alreadyWorkingActive = false;
				})
				.SetEase(Ease.InOutQuart);
	}
}
