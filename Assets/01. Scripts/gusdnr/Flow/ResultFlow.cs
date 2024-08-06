using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Spine.Unity.Editor.SkeletonBaker.BoneWeightContainer;


public class ResultFlow : SceneFlowBase
{
	#region Structs

	[System.Serializable]
	struct GameResultPair
	{
		public string ResultText;
		public Sprite ResultImage;
	}

	[System.Serializable]
	struct OreResultPair
	{
		public StatType ThisOreType;
		public TextMeshProUGUI Text;
	}

	#endregion

	[Header("Game Result UI Elements")]
	[SerializeField] private GameResultPair[] ResultPairs;
	[SerializeField] private Image ResultImg;
	[SerializeField] private TextMeshProUGUI ResultText;
	[Range(0f, 3f)][SerializeField] private float ResultDuration = 1.0f;

	[Header("Ore Result UI Elements")]
	[SerializeField] private OreResultPair[] OrePairs;
	[SerializeField] private TextMeshProUGUI OreSum;
	[Range(0f, 3f)][SerializeField] private float OreDuration = 0.3f;

	[Header("Time Result UI Elements")]
	[SerializeField] private TextMeshProUGUI FloorText;
	[SerializeField] private TextMeshProUGUI TimeText;

	[Header("Buttons")]
	[SerializeField] private Button ReStartBtn;
	[SerializeField] private Button QuitBtn;

	private int OreCount = 0;
	private int OreCountSum = 0;

	public override void ActiveFlowBase()
	{
		if(mngs.FlowMng.isGameClear == false) ResultText.text = ResultPairs[0].ResultText;
		else if(mngs.FlowMng.isGameClear == true) ResultText.text = ResultPairs[1].ResultText;
		TMPDOText(ResultText, ResultDuration);

		OreCount = OreCountSum = 0;

		foreach (OreResultPair pair in OrePairs)
		{
			OreCount = (int)(mngs?.PlayerMng?.RetrunOreCount(pair.ThisOreType));
			pair.Text.text = IntToString(OreCount);
			TMPDOText(pair.Text, OreDuration);
			OreCountSum = OreCountSum + OreCount;
		}

		OreSum.text = IntToString(OreCountSum);
		TMPDOText(OreSum, OreDuration);

		ReStartBtn?.onClick.RemoveAllListeners();
		ReStartBtn?.onClick.AddListener(() => mngs.UIMng.SetSceneName(NextSceneName));

		QuitBtn?.onClick.RemoveAllListeners();
		QuitBtn?.onClick.AddListener(() => mngs.UIMng.QuitGame());
	}

	private string IntToString(int value)
	{
		return string.Format("{00}", value);
	}

	private void TMPDOText(TextMeshProUGUI tmp, float duration)
	{
		tmp.maxVisibleCharacters = 0;
		DOTween.To(x => tmp.maxVisibleCharacters = (int)x, 0f, tmp.text.Length, duration);
	}
}
