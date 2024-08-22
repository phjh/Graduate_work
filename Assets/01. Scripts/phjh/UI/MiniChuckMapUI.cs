using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniChuckMapUI : MonoBehaviour
{
    public static MiniChuckMapUI Instance;

    [SerializeField]
    private Material HighlightedMat;
    private Material BaseMat;

    private Transform _playerTrm;
    private InputReader _input;

    [SerializeField]
    List<RawImage> images;

    private int _beforeChunk = 4;

    private void OnEnable()
    {
        if (Instance == null) Instance = this;
        
    }

    public void ChunkMiniMapUIInit(Transform trm, InputReader input)
    {
        _playerTrm = trm;
        _input = input;
        gameObject.SetActive(true);
        _input.MovementEvent += SetPlayerChunk;
        BaseMat = images[0].material;
    }

    private void SetPlayerChunk(Vector2 value)
    {
        Vector3 pos = _playerTrm.position - Vector3.one;
        int nowChunk = 0;
        nowChunk = (int)(pos.x / 25) + (int)(pos.z / 25) * 3;
        if (_beforeChunk != nowChunk)
        {
            images[_beforeChunk].material = BaseMat;
            _beforeChunk = nowChunk;
            images[nowChunk].material = HighlightedMat;
        }
        if((pos.x >= 25.5f && pos.x < 48.5f)&&(pos.z >= 25.5f && pos.z < 48.5f))
        {
            MapManager.Instance.ActvieDangerZone(1);
            TimeManager.Instance.Addtime(120);
        }
    }

}
