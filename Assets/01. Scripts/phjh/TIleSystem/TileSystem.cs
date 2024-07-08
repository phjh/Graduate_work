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
            Vector3 pos = new Vector3Int(tileblocks.Count + 1, 0, 1);
            //풀매니저로 바궈줄거
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
