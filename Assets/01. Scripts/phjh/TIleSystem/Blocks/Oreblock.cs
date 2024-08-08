using UnityEngine;

public class OreBlock : Blocks
{
	[System.Serializable]
	public struct MeshPair
	{
		public Mesh Mesh;
		public Material Material;
	}
	
	[Header("Block Value")]
	public int MaxTimesToBreak = 7;
	public int ExposeToBreak = 3;
	public ItemDataSO OreData;

	[Header("Mesh Values")]
	[SerializeField] private MeshPair OrePair;
	[SerializeField] private MeshPair BreakablePair;

	private MeshFilter OreMeshFilter;
	private MeshRenderer OreMeshRenderer;

	private int TimesToBreak;
	private bool alreadyExposed = false;

	//이외 모션 및 여러가지 여기서 처리 예정

	public override void SetBlock()
	{
		if (OreMeshFilter == null) TryGetComponent(out  OreMeshFilter);
		if (OreMeshRenderer == null) TryGetComponent(out OreMeshRenderer);

		SetMesh(BreakablePair);

		alreadyExposed = false;

		blockType = BlockType.OreBlock;
		TimesToBreak = MaxTimesToBreak;
        transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	public override void BlockEvent(Vector3 pos, int breaks = 1)
    {
		TimesToBreak -= breaks;
		MiningEffect(pos);
		
		if (TimesToBreak <= 0)
		{
			DropOre();
			DeleteBlock();
		}

		if (alreadyExposed == false && TimesToBreak <= ExposeToBreak)
		{
			alreadyExposed = true;
			SetMesh(OrePair);
            transform.rotation = Quaternion.Euler(45, 0, 0);
        }
	}

	private void SetMesh(MeshPair InitMesh)
	{
		OreMeshFilter.mesh = InitMesh.Mesh;
		OreMeshRenderer.material = InitMesh.Material;
	}

	private void DropOre()
	{
		int dropOreCount = Random.Range(1, 3);
        for (int c = 0; c < dropOreCount ; c++)
		{
			Vector3 randdir = Random.onUnitSphere/5;
			if (PoolManager.Instance.Pop("DropedItem", transform.position + new Vector3(randdir.x, 0.5f, randdir.z)).TryGetComponent(out DropedItem item)) 
				item.InitializeItemData(OreData);
		}
	}

}
