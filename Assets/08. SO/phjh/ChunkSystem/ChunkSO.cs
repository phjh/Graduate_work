using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChunkSO", menuName = "SO/Map/ChunkSO")]
public class ChunkSO : ScriptableObject
{
    public string chunkName;
    public TextAsset excelData;

    [Range(10, 50)]
    public int chunkOreCounts;
    public Vector3 chunkPos;

    public List<List<int>> chunkData;


    public ChunkSO CreateChunk()
    {
        ChunkSO chunk = Instantiate(this);
        chunk.chunkData = new List<List<int>>();
        chunk.chunkPos = Vector3.zero;
        return chunk;
    }

}
