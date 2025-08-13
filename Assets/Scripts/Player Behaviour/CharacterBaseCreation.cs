using System;
using UnityEngine;

public class CharacterBaseCreation : MonoBehaviour
{
    public enum HeightType
    {
        Light,
        Medium,
        Big
    }
    
    public HeightType heightType;
    public CharacterPreset lightPreset;
    public CharacterPreset mediumPreset;
    public CharacterPreset bigPreset;

    public Spell spell01;
    public Spell spell02;

    public void Awake()
    {
        PlayerScript playerScript = GetComponent<PlayerScript>();
        if (playerScript == null)
        {
            playerScript = gameObject.AddComponent<PlayerScript>();
        }

        switch (heightType)
        {
            case HeightType.Light: playerScript.ApplyPreset(lightPreset); break;
            case HeightType.Medium: playerScript.ApplyPreset(mediumPreset); break;
            case HeightType.Big: playerScript.ApplyPreset(bigPreset); break;
        }
    }
}
