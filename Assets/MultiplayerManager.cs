using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance { get; private set; }
    public List<Gamepad> GamepadList;
    public GameObject PlayerPrefab;
    

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
    
    void Start()
    {
        GamepadList = Gamepad.all.ToList();
        SpawnPlayersForGamepads();
    }

    private void Update()
    {
        //CheckGamepadActions();
    }

    public void SpawnPlayersForGamepads()
    {
        foreach (Gamepad gamepad in GamepadList)
        {
            PlayerInput playerInput = PlayerInput.Instantiate(
                PlayerPrefab,
                controlScheme: "Gamepad",
                pairWithDevice: gamepad,
                splitScreenIndex: -1
            );

            playerInput.gameObject.name = $"Base Player {playerInput.playerIndex}";
            
            PlayerScript playerScript = playerInput.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                // playerScript.gamepad = gamepad;
                GameManager.Instance.PlayerScriptList.Add(playerScript);

                int playerIndex = playerInput.playerIndex; 
                GameManager.Instance.PlayerScriptDictionary[playerScript] = new PlayerData(gamepad, playerIndex);
                
                playerInput.SwitchCurrentControlScheme(gamepad);
                
                Debug.Log($"Ajout au dictionnaire : PlayerScript = {playerScript.name}, Gamepad = {gamepad.deviceId}, PlayerIndex = {playerIndex}");
            }
        }
    }

    // public void CheckGamepadActions()
    // {
    //     foreach (var kvp in GameManager.Instance.PlayerScriptDictionary)
    //     {
    //         var playerScript = kvp.Key;
    //         var playerInput = playerScript.GetComponent<PlayerInput>();
    //         if (playerInput == null) continue;
    //
    //         foreach (var action in playerInput.actions)
    //         {
    //             if (action == null) continue;
    //             if (action.triggered)
    //             {
    //                 //Debug.Log($"[{playerScript.name}] Action déclenchée : {action.name} (par Gamepad {kvp.Value.gamepad.deviceId})");
    //             }
    //         }
    //     }
    // }
    
}