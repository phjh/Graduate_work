using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    [SerializeField]
    private int width = 101;
    [SerializeField]
    private int height = 101;

    [SerializeField]
    public List<Blocks> tileblocks;

    #region Ÿ��������

    [SerializeField]
    private List<GameObject> blocks;

    

    #endregion

    private void Start()
    {
        InitMap();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 pos = new Vector3Int(tileblocks.Count + 1, 0, 1);
            //Ǯ�Ŵ����� �ٱ��ٰ�
            Blocks block = Instantiate(blocks[0]).GetComponent<Blocks>();
            block.Init(pos, block.gameObject, this.transform, this);
            tileblocks.Add(block);
        }
    }

    private void InitMap()
    {
        tileblocks.Clear();
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
