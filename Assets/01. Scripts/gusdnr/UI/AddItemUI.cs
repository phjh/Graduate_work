using DG.Tweening;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddItemUI : PoolableMono
{
	[Header("UI Elements")]
	[SerializeField] private RectTransform ThisRectTrm;
	public Image ItemImage;
	public TextMeshProUGUI AddCountText;

	[Header("Animaton Value")]
	[SerializeField] private Vector3 MinScale = Vector2.zero;
	[SerializeField] private Vector3 MaxScale = Vector2.one;
	[SerializeField] private float DurationValue = 0.5f;
	[SerializeField] private float LiveTime = 1f;

	private Managers mngs;
	private StatType ThisStatType;
	private int CurrentAddingCount = 0;

	private StringBuilder CurrentCountString = new StringBuilder(2);

	public override void ResetPoolableMono()
	{
		if(mngs == null) mngs = Managers.GetInstance();

		Logger.Assert(ThisRectTrm != null, "This RectTrm is Null");
		Logger.Assert(ItemImage != null, "Item Image is Null");
		Logger.Assert(AddCountText != null, "Add CountText is Null");
	}

	public override void EnablePoolableMono()
	{
		ItemImage.sprite = null;
		AddCountText.text = string.Empty;

		CurrentAddingCount = 0;
		CurrentCountString = new StringBuilder(2);
		ThisRectTrm.localScale = MinScale;
	}

	public void InitData(ItemDataSO ItemData, int AddCount)
	{
		SetAddingCountString(AddCount);
		ThisStatType = ItemData.AddingStatType;
		ThisRectTrm.DOScale(MaxScale, DurationValue)
			.OnStart(() =>
			{
				ItemImage.sprite = ItemData.Image;
			})
			.OnComplete(() =>
			{
				Invoke(nameof(PushUI), LiveTime);
			})
			.SetEase(Ease.InOutBack);
	}

	public void SetAddingCountString(int AddCount)
	{
		CurrentAddingCount = CurrentAddingCount + AddCount;

		CurrentCountString.Clear();
		CurrentCountString.Append("X ").Append(CurrentAddingCount);

		AddCountText.text = CurrentCountString.ToString();
	}

	private void PushUI()
	{
		ThisRectTrm.DOScale(MinScale, DurationValue)
			.OnComplete(() =>
			{
				mngs.UIMng.AddUIs.Remove(ThisStatType);
				mngs.PoolMng.Push(this, PoolName);
			})
			.SetEase(Ease.OutExpo);
	}
}
