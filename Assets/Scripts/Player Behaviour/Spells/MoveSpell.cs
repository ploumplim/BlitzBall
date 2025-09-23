using UnityEngine;

[CreateAssetMenu(menuName = "Blitz Balls/Move Spell", fileName = "Move Spell")]
public class MoveSpell : Spell
{
    public override void DoSpell(Transform player)
    {
        Debug.Log("Move Spell");
    }
}