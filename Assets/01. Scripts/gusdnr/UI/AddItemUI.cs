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

		ThisRectTrm.localScale = MinScale;
	}

	public void InitData(Sprite ItemSprite, int AddCount = 1)
	{
		StringBuilder count = new StringBuilder(2);
		count.Append("X ").Append(AddCount);

		ThisRectTrm.DOScale(MaxScale, DurationValue)
			.OnStart(() =>
			{
				ItemImage.sprite = ItemSprite;
				AddCountText.text = count.ToString();
			})
			.OnComplete(() =>
			{
				Invoke(nameof(PushUI), LiveTime);
			})
			.SetEase(Ease.InOutBack);
	}

	private void PushUI()
	{
		ThisRectTrm.DOScale(MinScale, DurationValue)
			.OnComplete(() =>
			{
				mngs.PoolMng.Push(this, PoolName);
			})
			.SetEase(Ease.OutExpo);
	}
}
