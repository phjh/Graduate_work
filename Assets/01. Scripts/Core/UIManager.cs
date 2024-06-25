using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase<UIManager>
{
	[Tooltip("[0 : Title] [1 : InGame] [2 : Result] [3 : Setting] [4 : Loading]")]
	[Header("UI Canvases")]
	public Canvas[] UICanvases;

	private int beforeSelected = -1;

	public override void InitManager()
	{
		base.InitManager();
		GameObject CanvasContainer = new GameObject();
		CanvasContainer.name = "CanvasContainer";
		DontDestroyOnLoad(CanvasContainer);

		SetUpUICanvases(CanvasContainer);
	}

	private void SetUpUICanvases(GameObject CanvasContainer)
	{
		if (UICanvases.Length < 0) return;
		foreach (Canvas cv in UICanvases)
		{
			cv.transform.parent = CanvasContainer.transform;
			cv.enabled = false;
		}
	}

	public void EnableSelectCanvas(int index)
	{
		if (beforeSelected >= 0 && beforeSelected < UICanvases.Length) UICanvases[beforeSelected].enabled = false;

		if(index > UICanvases.Length) return;
		if (UICanvases[index] == null) return;
		UICanvases[index].enabled = true;

		beforeSelected = index;
	}
}
