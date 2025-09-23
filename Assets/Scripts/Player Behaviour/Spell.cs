using UnityEngine;

public class Spell : ScriptableObject
{
    public float spellCooldown;
    public virtual void DoSpell(Transform player) {}
}