using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<PlayerScript> PlayerScriptList;
    public Dictionary<PlayerScript, PlayerData> PlayerScriptDictionary = new Dictionary<PlayerScript, PlayerData>();
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
}

public struct PlayerData
{
    public Gamepad gamepad;
    public int userId;

    public PlayerData(Gamepad gamepad, int userId)
    {
        this.gamepad = gamepad;
        this.userId = userId;
    }
}