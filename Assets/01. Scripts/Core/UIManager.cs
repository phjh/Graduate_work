using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : ManagerBase<UIManager>
{
	#region UI Variables

	[Header("UI Canvases")]
	public Canvas MainCanvas;
	[Tooltip("[0 : Title] [1 : InGame] [2 : Result] [3 : Setting]")]
	public Canvas[] UICanvases;

	[Header("UI Elements")]
	[SerializeField] private Image FadePanel;
	[SerializeField][Range(0.0f, 1.0f)] private float fadeDuration = 0.5f;
	[SerializeField] private Image LoadProcessBar;

	public bool IsWorkingLoading
	{
		get;
		private set;
	} = false;

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
		if (ActiveScene().name == sceneName || IsWorkingLoading) return;
		IsWorkingLoading = true;

		FadePanel.DOFade(1.0f, fadeDuration)
			.OnStart(() =>
			{
				FadePanel.raycastTarget = true;
			})
			.OnComplete(() =>
			{
				StartCoroutine(TransitionScene(sceneName));
				LoadProcessBar.gameObject.SetActive(true);
			});
	}

	private IEnumerator TransitionScene(string sceneName)
	{
		Scene beforeScene = ActiveScene();
		yield return LoadSceneAsync(sceneName);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(string sceneName)
	{
		IsWorkingLoading = true;

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = false;
		float percentage = 0f;

		while (!asyncLoad.isDone)
		{
			yield return null;

			Logger.Log($"Loading : [{percentage}]%");

			LoadProcessBar.fillAmount = 1f - asyncLoad.progress;
			if (percentage < 90f)
			{
				percentage = Mathf.MoveTowards(percentage, asyncLoad.progress * 100, Time.deltaTime * 10f);
			}
			else
			{
				percentage = Mathf.MoveTowards(percentage, 100f, Time.deltaTime * 10f);

				if (percentage >= 99f) asyncLoad.allowSceneActivation = true;
			}
		}

		FadePanel.DOFade(0.0f, fadeDuration)
			.OnComplete((TweenCallback)(() =>
			{
				FadePanel.raycastTarget = false;
				this.IsWorkingLoading = false;
			}));
	}

	#endregion

	#region Load Scene Int

	public void SetSceneIndex(int index)
	{
		if (ActiveScene().buildIndex == index || IsWorkingLoading) return;
		IsWorkingLoading = true;

		FadePanel.DOFade(1.0f, fadeDuration)
			.OnStart(() =>
			{
				FadePanel.raycastTarget = true;
			})
			.OnComplete(() =>
			{
				StartCoroutine(TransitionScene(index));
				LoadProcessBar.gameObject.SetActive(true);
			});
	}

	private IEnumerator TransitionScene(int index)
	{
		Scene beforeScene = ActiveScene();
		yield return LoadSceneAsync(index);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(int index)
	{
		IsWorkingLoading = true;

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = true;
		float percentage = 0f;

		while (!asyncLoad.isDone)
		{
			yield return null;

			Logger.Log($"Loading : [{percentage}]%");
			LoadProcessBar.fillAmount = 1f - asyncLoad.progress;
			if (percentage < 90f)
			{
				percentage = Mathf.MoveTowards(percentage, asyncLoad.progress * 100, Time.deltaTime * 10f);
			}
			else
			{
				percentage = Mathf.MoveTowards(percentage, 100f, Time.deltaTime * 10f);

				if (percentage >= 99f) asyncLoad.allowSceneActivation = true;
			}
		}

		FadePanel.DOFade(0.0f, fadeDuration)
			.OnComplete((TweenCallback)(() =>
			{
				FadePanel.raycastTarget = false;
				this.IsWorkingLoading = false;
			}));
	}

	#endregion

	private IEnumerator UnloadSceneAsync(int index)
	{
		LoadProcessBar.gameObject.SetActive(false);
	
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(index);
		while (!asyncUnload.isDone)
		{
			Logger.Log($"UnLoading : [{asyncUnload.progress * 100}]%");
			yield return null;
		}
		Debug.Log($"UnLoad Complete : {SceneManager.GetSceneByBuildIndex(index).name}");

		IsWorkingLoading = false;
	}

	public void QuitGame()
	{
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
		Application.Quit();
	#endif
	}

	#endregion

}
