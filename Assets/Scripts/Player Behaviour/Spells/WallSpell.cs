using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Blitz Balls/Wall Spell", fileName = "Wall Spell")]
public class WallSpell : Spell
{
    public GameObject wallPrefab;
    public GameObject previewWallPrefab;
    public float timeToDestroyWall;

    public float wallSpawnDistanceToPlayer;
    
    
    public override void DoSpell(Transform player)
    {
        Debug.Log("Create Wall");
        CreateWall(player);
    }

    public override void DoPreviewSpell(Transform player)
    {
        CreatePreviewWall(player);
    }

    public void CreateWall(Transform player)
    {
        // Implementation for creating a wall in front of the player
        GameObject SpellWall = Instantiate(wallPrefab, player.position + player.forward * wallSpawnDistanceToPlayer, player.rotation);
        SpellWall.name = "SpellWall";
        SpellWall.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        
        Destroy(SpellWall, timeToDestroyWall);
    }
    
    public void CreatePreviewWall(Transform player)
    {
        Debug.Log("Create Preview Wall");
        // Implementation for creating a preview wall in front of the player
        GameObject PreviewWall = Instantiate(previewWallPrefab, player.position + player.forward * wallSpawnDistanceToPlayer, player.rotation);
        PreviewWall.name = "PreviewWall";
        PreviewWall.GetComponentInChildren<MeshRenderer>().material.color = new Color(0f, 1f, 0f, 0.5f);
    }
    
    
}
