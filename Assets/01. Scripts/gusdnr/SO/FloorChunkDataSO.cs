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
	[Header("Boss Chunks")]
	public ChunkSO BossChunk;

	public ChunkSO RetrunSelectChunk(List<ChunkSO> InitChunkList)
	{
		if (InitChunkList == null || InitChunkList.Count == 0)
		{
			Logger.LogError("InitChunkList is empty or null!");
			return null; // 또는 기본값을 반환
		}
		return InitChunkList[Random.Range(0, InitChunkList.Count - 1)];
	}
}
