using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Blitz Balls/Wall Spell", fileName = "Wall Spell")]
public class WallSpell : Spell
{
    public GameObject wallPrefab;
    public GameObject previewWallPrefab;
    public float timeToDestroyWall;
    public GameObject previewWallPrefabInstance;
    public float wallSpawnDistanceToPlayer;
    
    
    public override void DoSpell(Transform player)
    {
        CreateWall(player);
    }

    public override void DoPreviewSpell(Transform player)
    {
        if (previewWallPrefabInstance == null)
        {
            CreatePreviewWall(player);
        }
        else
        {
            MovePreviewWallAlongPlayer(player);
        }
        
    }

    public void CreateWall(Transform player)
    {
        // Implementation for creating a wall in front of the player
        GameObject SpellWall = Instantiate(wallPrefab, player.position + player.forward * wallSpawnDistanceToPlayer, player.rotation);
        SpellWall.name = "SpellWall";
        SpellWall.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        Destroy(previewWallPrefabInstance);
        
        Destroy(SpellWall, timeToDestroyWall);
    }
    
    public void CreatePreviewWall(Transform player)
    {
        // Implementation for creating a preview wall in front of the player
        previewWallPrefabInstance = Instantiate(previewWallPrefab, player.position + player.forward * wallSpawnDistanceToPlayer, player.rotation);
        previewWallPrefabInstance.name = "PreviewWall";
        previewWallPrefabInstance.GetComponentInChildren<MeshRenderer>().material.color = new Color(0f, 1f, 0f, 0.5f);
    }
    
    public void MovePreviewWallAlongPlayer(Transform player)
    {
        if (previewWallPrefabInstance != null)
        {
            previewWallPrefabInstance.transform.position = player.position + player.forward * wallSpawnDistanceToPlayer;
            previewWallPrefabInstance.transform.rotation = player.rotation;
        }
    }
    
    
}
