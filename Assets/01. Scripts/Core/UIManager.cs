using DG.Tweening;
using System;
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
	[Tooltip("[0 : Intro] [1 : InGame] [2 : Result] [3 : Setting]")]
	public Canvas[] UICanvases;

	[Header("UI Elements")]
	[SerializeField] private Image FadePanel;
	[SerializeField][Range(0.0f, 1.0f)] private float fadeDuration = 0.5f;
	[SerializeField] private GameObject LoadProcessObject;
	[SerializeField] private RectTransform LoadProcessFill;
	[SerializeField] private GameObject OptionWindow;

	private Vector2 FillAmountSize = Vector2.up;

	public event Action OnCompleteLoadScene;

	public bool IsWorkingLoading
	{
		get;
		private set;
	} = false;

	private bool IsOpenOption = false;

	private Dictionary<string, PopupUI> popups = new Dictionary<string, PopupUI>();

	private Canvas CurrentActiveCanvas = null;
	private AsyncOperation asyncLoad;

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
		CanvasContainer.AddComponent<RectTransform>();
		DontDestroyOnLoad(CanvasContainer);

		beforeSelected = -1;

		SetUpUICanvases(CanvasContainer);
		SetUpPopupUIs();

        IsOpenOption = false;

        FadePanel.DOFade(0.0f, fadeDuration)
			.OnStart(() =>
			{
				this.IsWorkingLoading = false;
				LoadProcessObject.SetActive(IsWorkingLoading);
			})
			.OnComplete(() =>
			{
				FadePanel.raycastTarget = false;

				OnCompleteLoadScene?.Invoke();
			});
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
		CurrentActiveCanvas = UICanvases[index];
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
				DisableSelectCanvas(beforeSelected);
				LoadProcessFill.gameObject.SetActive(true);
			});
	}

	private IEnumerator TransitionScene(string sceneName)
	{
		Scene beforeScene = ActiveScene();
		if(sceneName == "InGame") mngs.InItInGameManagers();
		yield return LoadSceneAsync(sceneName);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(string sceneName)
	{
		FillAmountSize = new Vector3(0, 1, 1);
		
		IsWorkingLoading = true;
		LoadProcessFill.localScale = FillAmountSize;
		LoadProcessObject.SetActive(IsWorkingLoading);

		asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = false;
		float percentage = 0f;

		while (!asyncLoad.isDone)
		{
			yield return null;

			Logger.Log($"Loading : [{percentage}]%");
			FillAmountSize.x = Mathf.Clamp(percentage * 0.01f, 0f, 1f);
			LoadProcessFill.localScale = FillAmountSize;
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
			.OnStart(() =>
			{
				this.IsWorkingLoading = false;
				LoadProcessObject.SetActive(IsWorkingLoading);
				LoadProcessFill.localScale = new Vector3(0, 1, 1);
			})
			.OnComplete(() =>
			{
				FadePanel.raycastTarget = false;

				OnCompleteLoadScene?.Invoke();
			});
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
				DisableSelectCanvas(beforeSelected);
				LoadProcessFill.gameObject.SetActive(true);
			});
	}

	private IEnumerator TransitionScene(int index)
	{
		Scene beforeScene = ActiveScene();
		if (index == 2) mngs.InItInGameManagers();
		yield return LoadSceneAsync(index);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(int index)
	{
		FillAmountSize = new Vector3(0, 1, 1);

		IsWorkingLoading = true;
		LoadProcessFill.localScale = FillAmountSize;
		LoadProcessObject.SetActive(IsWorkingLoading);

		asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = true;
		float percentage = 0f;

		while (!asyncLoad.isDone)
		{
			yield return null;

			Logger.Log($"Loading : [{percentage}]%");
			
			FillAmountSize.x = Mathf.Clamp(percentage * 0.01f, 0f, 1f);
			LoadProcessFill.localScale = FillAmountSize;
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
			.OnStart(() =>
			{
				this.IsWorkingLoading = false;
				LoadProcessObject.SetActive(IsWorkingLoading);
				LoadProcessFill.localScale = new Vector3(0, 1, 1);
			})
			.OnComplete(() =>
			{
				FadePanel.raycastTarget = false;

				OnCompleteLoadScene?.Invoke();
			});
	}

	#endregion

	private IEnumerator UnloadSceneAsync(int index)
	{
		LoadProcessFill.gameObject.SetActive(false);
	
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

	private RectTransform AddPopupContainer;
	public void PopupAddItemUI(Sprite ItemSprite, int AddCount = 1)
	{
		/*if(AddPopupContainer == null) */CurrentActiveCanvas.transform.Find("AddItemList").TryGetComponent(out AddPopupContainer);
		Logger.Assert(AddPopupContainer != null, "Item Popup Container is Null");
		if (mngs.PoolMng.Pop("AddItemUI").TryGetComponent(out AddItemUI uiCompoenet))
		{
			uiCompoenet.transform.parent = AddPopupContainer;

			uiCompoenet.InitData(ItemSprite, AddCount);
		}
		else return;
	}

	public void OpenOptionWindow(bool isOpen)
	{
		if (IsOpenOption == isOpen) return;
		IsOpenOption = isOpen;

		if(IsOpenOption == true)
		{
            FadePanel.DOFade(0.5f, fadeDuration)
            .OnStart(() =>
            {
                FadePanel.raycastTarget = true;
            })
            .OnComplete(() =>
            {
                OptionWindow.SetActive(true);
            });

            OptionWindow.SetActive(false);

        }
		else if(IsOpenOption == false)
		{
            FadePanel.DOFade(0.0f, fadeDuration)
            .OnStart(() =>
            {
                OptionWindow.SetActive(false);
            })
            .OnComplete(() =>
            {
                FadePanel.raycastTarget = false;
            });

        }
	}

	public void ResumeGame()
	{
		OpenOptionWindow(false);
    }
}
