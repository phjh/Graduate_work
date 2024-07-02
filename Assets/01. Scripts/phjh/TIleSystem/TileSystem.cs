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

    #region 타일정보들

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
            //풀매니저로 바궈줄거
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
            //여기서 타일 감지해서 1차로 리스트에 세팅을 해준다.
            ScanTileAndSetting();

            //1차 세팅이 끝나고 2차적으로 랜덤으로 오브젝트같은것들을 배치한다.
            SetRandomObjectsSetting();

        }
    }

    private int ListConverter(int width, int height)
    {
        //101width -> 1height
        // 101가로 = 1세로
        //대충 가로 후 세로
        return width + height * this.width;
    }

    private void ScanTileAndSetting()
    {
        
    }

    private void SetRandomObjectsSetting()
    {

    }


}
