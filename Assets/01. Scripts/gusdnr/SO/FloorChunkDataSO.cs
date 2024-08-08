using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FloorChunkData", menuName = "SO/Data/Floor Chunk")]
public class FloorChunkDataSO : ScriptableObject
{
    [Header("Fixed Chuncks")]
    public List<ChunkSO> FirstChunks = new List<ChunkSO>();
    public List<ChunkSO> ThirdChunks = new List<ChunkSO>();
    public List<ChunkSO> SixthChunks = new List<ChunkSO>();
    public List<ChunkSO> NinthChunks = new List<ChunkSO>();
    [Header("Random Chuncks")] 
    public List<ChunkSO> RandomChunks = new List<ChunkSO>();

    public ChunkSO RetrunSelectChunk(List<ChunkSO> InitChunkList)
    {
        return InitChunkList[Random.Range(0, InitChunkList.Count - 1)];
    }
}
