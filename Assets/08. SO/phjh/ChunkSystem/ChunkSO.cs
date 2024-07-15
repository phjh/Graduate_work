using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChunkSO", menuName = "SO/Data/ChunkData")]
public class ChunkSO : ScriptableObject
{
    [Header("Chunk Inspector")]
    public string chunkName;
    public TextAsset excelData;
    [Range(1, 30)] public int PlaceOreBlockInCounter;

    [HideInInspector] public Vector3 BaseChunkPos;
    public List<List<int>> chunkData;

    public ChunkSO CreateCloneChunk(Vector3 StartPos)
    {
        ChunkSO CloneChunk = Instantiate(this);
        CloneChunk.excelData = excelData;
        CloneChunk.chunkData = new List<List<int>>();
        CloneChunk.PlaceOreBlockInCounter = PlaceOreBlockInCounter;
        CloneChunk.BaseChunkPos = StartPos;
        return CloneChunk;
    }

}
