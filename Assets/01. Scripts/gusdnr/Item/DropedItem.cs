using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DropedItem : PoolableMono
{
    [Header("Item UI")]
    [SerializeField] private Transform ItemInfoCanvas;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDescription;

    [Header("UI Variables")]
    [SerializeField][Range(0.0f, 1.0f)] private float infoDuration = 0.3f;
    [SerializeField] private Vector3 MinScale = Vector3.zero;
    [SerializeField] private Vector3 MaxScale = Vector3.one;
    
    private Managers mngs;

    public ItemDataSO ItemData;

    private SpriteRenderer ItemRenderer;

	public void InitializeItemData(ItemDataSO InitData)
    {
        if (InitData.CheckingInitData() == false)
        {
            Logger.LogError("error");
            Logger.LogError($"{InitData.name}'s Data is null! Please checking Inspector");
            return;
        }

        ItemData = InitData;
        ItemData.SetRandomAddingValue();

        SetItemObject();
	}

	private void SetItemObject()
    {
        ItemRenderer.sprite = ItemData.Image;

		ItemName.text = string.IsNullOrEmpty(ItemData.Name) ? "No Name" : ItemData.Name;
		ItemDescription.text = string.IsNullOrEmpty(ItemData.Description) ? "No Desc" : ItemData.Description;
	}

	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ItemInfoCanvas.DOScale(MaxScale, infoDuration).SetEase(Ease.OutBack);
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            mngs.PlayerMng.AddOreInDictionary(ItemData.AddingStatType, ItemData.ItemAddingValue);
            mngs.UIMng.PopupAddItemUI(ItemData.Image);
            mngs.PoolMng.Push(this, PoolName);
            return;
        }
        if(other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.C))
        {
            mngs.PlayerMng.Player._attack.FuelRecharge(25);
            mngs.PoolMng.Push(this, PoolName);
        }
    }

    private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.CompareTag("Player"))
        {
			ItemInfoCanvas.DOScale(MinScale, infoDuration).SetEase(Ease.OutExpo);
		}
	}

	public override void ResetPoolableMono()
	{
		if (TryGetComponent(out ItemRenderer) == false)
		{
			ItemRenderer = this.gameObject.AddComponent<SpriteRenderer>();
		}

		if (mngs == null) mngs = Managers.GetInstance();
		ItemInfoCanvas.localScale = MinScale;
	}

	public override void EnablePoolableMono()
	{
		if (ItemInfoCanvas.TryGetComponent(out Canvas itemCanvas)) itemCanvas.worldCamera = Camera.main;
	}
}
