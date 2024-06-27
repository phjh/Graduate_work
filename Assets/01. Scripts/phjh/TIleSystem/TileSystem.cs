using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    [SerializeField]
    private int width = 101;
    [SerializeField]
    private int height = 101;

    private Dictionary<Vector3Int, Lazy<Blocks>> tileMap = new();

    #region Ÿ��������

    [SerializeField]
    private List<Sprite> mapSprites;


    #endregion

    private void Start()
    {
        InitMap();
    }


    private void InitMap()
    {
        tileMap.Clear();
        int loops = width * height;
        for (int i = 0; i < loops; i++)
        {
            //���⼭ Ÿ�� �����ؼ� 1���� ����Ʈ�� ������ ���ش�.
            ScanTileAndSetting();

            //1�� ������ ������ 2�������� �������� ������Ʈ�����͵��� ��ġ�Ѵ�.
            SetRandomObjectsSetting();

        }
    }

    private int ListConverter(int width, int height)
    {
        //101width -> 1height
        // 101���� = 1����
        //���� ���� �� ����
        return width + height * this.width;
    }

    private void ScanTileAndSetting()
    {
        
    }

    private void SetRandomObjectsSetting()
    {

    }


}
