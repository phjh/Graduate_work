using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : ManagerBase<UIManager>
{
	#region UI Variables

	[Tooltip("[0 : Title] [1 : InGame] [2 : Result] [3 : Setting] [4 : Loading]")]
	[Header("UI Canvases")]
	public Canvas MainCanvas;
	public Canvas[] UICanvases;

	[Header("UI Elements")]
	[SerializeField] private Image FadePanel;
	[SerializeField] private float fadeDuration = 0.5f;

	private Dictionary<string, PopupUI> popups = new Dictionary<string, PopupUI>();
	private int beforeSelected = -1;

	#endregion

	#region Scene Variables

	public Scene ActiveScene() { return SceneManager.GetActiveScene(); }

	public int ActiveSceneIndex() { return ActiveScene().buildIndex; }

	public bool CheckLoadCurrentScene(int index)
	{
		return ActiveScene() == SceneManager.GetSceneByBuildIndex(index);
	}

	public bool CheckLoadCurrentScene(string name)
	{
		return ActiveScene() == SceneManager.GetSceneByName(name);
	}

	private bool isLoadingStart = false;

	#endregion

	public override void InitManager()
	{
		base.InitManager();
		GameObject CanvasContainer = new GameObject();
		CanvasContainer.name = "CanvasContainer";
		DontDestroyOnLoad(CanvasContainer);

		SetUpUICanvases(CanvasContainer);
		SetUpPopupUIs();
	}

	#region UI Methods

	private void SetUpUICanvases(GameObject CanvasContainer)
	{
		if (UICanvases.Length < 0) return;
		foreach (Canvas cv in UICanvases)
		{
			cv.transform.parent = CanvasContainer.transform;
			cv.enabled = false;
		}

		MainCanvas.transform.parent = CanvasContainer.transform;
		MainCanvas.enabled = true;
	}

	private void SetUpPopupUIs()
	{
		foreach (PopupUI popup in FindObjectsOfType<PopupUI>())
		{
			popups.Add(popup.GetType().Name.Replace(typeof(PopupUI).Name, ""), popup);
			popup.TogglePopup(false);
		}

	}

	public void EnableSelectCanvas(int index)
	{
		DisableSelectCanvas(beforeSelected);

		if(index > UICanvases.Length) return;
		if (UICanvases[index] == null) return;

		UICanvases[index].enabled = true;

		beforeSelected = index;
	}

	public void DisableSelectCanvas(int index)
	{
		if(index >= 0 && index < UICanvases.Length)
		{
			if (UICanvases[index] == null || UICanvases[index].enabled == false) return;

			UICanvases[index].enabled = false;
		}
		else return;
	}

	#endregion

	#region Scene Methods

	#region Load Scene String

	public void SetSceneName(string sceneName)
	{
		if (ActiveScene().name == sceneName || isLoadingStart) return;
		StartCoroutine(TransitionScene(sceneName));
	}

	private IEnumerator TransitionScene(string sceneName)
	{
		Scene beforeScene = ActiveScene();
		yield return LoadSceneAsync(sceneName);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(string sceneName)
	{
		isLoadingStart = true;

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = false;

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
				asyncLoad.allowSceneActivation = true;

			Debug.Log($"Loading : [{asyncLoad.progress * 100}]%");
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
	}

	#endregion

	#region Load Scene Int

	public void SetSceneIndex(int index)
	{
		if (ActiveScene().buildIndex == index || isLoadingStart) return;
		StartCoroutine(TransitionScene(index));
	}

	private IEnumerator TransitionScene(int index)
	{
		Scene beforeScene = ActiveScene();
		yield return LoadSceneAsync(index);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(int index)
	{
		isLoadingStart = true;

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = false;

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
				asyncLoad.allowSceneActivation = true;

			Debug.Log($"Loading : [{asyncLoad.progress * 100}]%");
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
	}

	#endregion

	private IEnumerator UnloadSceneAsync(int index)
	{
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(index);
		while (!asyncUnload.isDone)
		{
			Debug.Log($"UnLoading : [{asyncUnload.progress * 100}]%");
			yield return null;
		}
		Debug.Log($"UnLoad Complete : {SceneManager.GetSceneByBuildIndex(index).name}");

		isLoadingStart = false;
	}

	#endregion

}
