using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    [SerializeField]
    private int width = 101;
    [SerializeField]
    private int height = 101;

    private List<Tiles> tileMap = new();


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
            tileMap.Add(new Tiles());
            tileMap[i].Init(true);
        }
    }


}
