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
        testInputFromController();
    }

    public void SpawnPlayersForGamepads()
    {
        foreach (var gamepad in GamepadList)
        {
            // Instancie le prefab du joueur
            GameObject player = Instantiate(PlayerPrefab);
            PlayerScript playerScript = player.GetComponent<PlayerScript>();

            if (playerScript != null)
            {
                // Associe le gamepad au PlayerScript
                playerScript.gamepad = gamepad;
                GameManager.Instance.PlayerScriptList.Add(playerScript);

                // Récupère le PlayerInput et son userId
                var playerInput = player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
                int userId = playerInput != null ? (int)playerInput.user.id : -1;

                // Ajoute au dictionnaire le PlayerScript et ses données associées
                GameManager.Instance.PlayerScriptDictionary[playerScript] = new PlayerData(gamepad, userId);

                // Log pour debug
                //Debug.Log($"Ajout au dictionnaire : PlayerScript = {playerScript.name}, Gamepad = {gamepad.deviceId}, UserId = {userId}");

                // Lie le contrôle au joueur
                LinkPlayerToControl();
            }
        }
    }

    public void LinkPlayerToControl()
    {
        foreach (var playerScript in GameManager.Instance.PlayerScriptList)
        {
            if (GameManager.Instance.PlayerScriptDictionary.TryGetValue(playerScript, out var playerData))
            {
                var playerInput = playerScript.GetComponent<PlayerInput>();
                if (playerInput != null)
                {
                    playerInput.SwitchCurrentControlScheme(playerData.gamepad);
                    Debug.Log($"PlayerInput de {playerScript.name} associé au Gamepad {playerData.gamepad.deviceId} et UserId {playerData.userId}");
                }
            }
        }
    }

    public void testInputFromController()
    {
        foreach (var kvp in GameManager.Instance.PlayerScriptDictionary)
        {
            var playerScript = kvp.Key;
            var playerData = kvp.Value;
            var gamepad = playerData.gamepad;

            if (gamepad == null) continue;

            // Exemple pour quelques boutons courants
            if (gamepad.buttonSouth.wasPressedThisFrame)
                Debug.Log($"[{playerScript.name}] a pressé A (buttonSouth) sur Gamepad {gamepad.deviceId} (UserId {playerData.userId})");
            if (gamepad.buttonNorth.wasPressedThisFrame)
                Debug.Log($"[{playerScript.name}] a pressé Y (buttonNorth) sur Gamepad {gamepad.deviceId} (UserId {playerData.userId})");
            if (gamepad.buttonWest.wasPressedThisFrame)
                Debug.Log($"[{playerScript.name}] a pressé X (buttonWest) sur Gamepad {gamepad.deviceId} (UserId {playerData.userId})");
            if (gamepad.buttonEast.wasPressedThisFrame)
                Debug.Log($"[{playerScript.name}] a pressé B (buttonEast) sur Gamepad {gamepad.deviceId} (UserId {playerData.userId})");
            if (gamepad.startButton.wasPressedThisFrame)
                Debug.Log($"[{playerScript.name}] a pressé Start sur Gamepad {gamepad.deviceId} (UserId {playerData.userId})");
        }
    }
    
}