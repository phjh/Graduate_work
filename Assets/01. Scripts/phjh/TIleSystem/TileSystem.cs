using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    [SerializeField]
    private int width = 101;
    [SerializeField]
    private int height = 101;

    private Dictionary<Vector3Int, Blocks> tileMap = new();
    [SerializeField]
    public List<Vector3Int> pos;
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
            Vector3Int pos = new Vector3Int(tileMap.Count + 1, 0, 1);
            //Ǯ�Ŵ����� �ٱ��ٰ�
            GameObject obj = Instantiate(blocks[0]);
            tileMap.Add(new Vector3Int(tileMap.Count + 1, 0, 1), obj.GetComponent<Blocks>());
            tileblocks.Add(obj.GetComponent<Blocks>());
            tileMap[pos].Init(pos, obj, this.transform);
        }
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
