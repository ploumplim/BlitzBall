using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Blitz Balls/Wall Spell", fileName = "Wall Spell")]
public class WallSpell : Spell
{
    public GameObject wallPrefab;
    public float timeToDestroyWall;

    public float wallSpawnDistanceToPlayer;
    
    
    public override void DoSpell(Transform player)
    {
        Debug.Log("Create Wall");
        CreateWall(player);
    }
    
    public void CreateWall(Transform player)
    {
        // Implementation for creating a wall in front of the player
        GameObject SpellWall = Instantiate(wallPrefab, player.position + player.forward * wallSpawnDistanceToPlayer, player.rotation);
        SpellWall.name = "SpellWall";
        SpellWall.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        
        Destroy(SpellWall, timeToDestroyWall);
    }
    
    
}
