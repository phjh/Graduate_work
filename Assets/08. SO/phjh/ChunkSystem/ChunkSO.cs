using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChunkSO", menuName = "SO/Data/ChunkData")]
public class ChunkSO : ScriptableObject
{
    [Header("Chunk Inspector")]
    public string chunkName;
    public TextAsset excelData;
    [Range(0f, 1f)] public float PlaceOreBlockEncounter;

    [HideInInspector] public Vector3 BaseChunkPos;
    public List<List<int>> chunkData;

    public ChunkSO CreateCloneChunk(Vector3 StartPos)
    {
        ChunkSO CloneChunk = Instantiate(this);
        CloneChunk.excelData = excelData;
        CloneChunk.chunkData = new List<List<int>>();
        CloneChunk.PlaceOreBlockEncounter = PlaceOreBlockEncounter;
        CloneChunk.BaseChunkPos = StartPos;
        return CloneChunk;
    }

}
