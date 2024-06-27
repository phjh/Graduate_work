using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DropedItem : MonoBehaviour
{
    [Header("Item UI")]
    [SerializeField] private Transform ItemInfoCanvas;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDescription;

    [Header("UI Variables")]
    [SerializeField][Range(0.0f, 1.0f)] private float infoDuration = 0.3f;
    
    public ItemDataSO ItemData
    {
        get
        {
            if(ItemData == null)
            {   
                Logger.LogError($"{this.name}'s Data is Null. Can't Get Data");
                return null;
            }
            return ItemData;
        }
        set
        {
            ItemData = value;
        }
    }

    private SpriteRenderer ItemRenderer;

	private void Start()
	{
		if(TryGetComponent(out ItemRenderer) == false)
        {
           ItemRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        }
	}

	public void InitializeItemData(ItemDataSO InitData)
    {
        if (InitData.CheckingInitData() == false)
        {
            Logger.LogError($"{InitData.name}'s Data is null! Please checking Inspector");
            return;
        }

        ItemData = InitData;
    }

    private void SetItemObject()
    {
        ItemRenderer.sprite = ItemData.Image;

		ItemName.text = string.IsNullOrEmpty(ItemData.Name) ? "No Name" : ItemData.Name;
		ItemDescription.text = string.IsNullOrEmpty(ItemData.Description) ? "No Desc" : ItemData.Description;
	}

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            ItemInfoCanvas.DOScale(Vector3.one, infoDuration).SetEase(Ease.OutBack)
                .OnStart(() =>
                {
                    SetItemObject();
                });

        }
	}

	private void OnCollisionExit(Collision collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
			ItemInfoCanvas.DOScale(Vector3.zero, infoDuration).SetEase(Ease.OutExpo);
		}
	}
}
