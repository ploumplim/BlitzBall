using System;
using TMPro;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    private enum PointMethod
    {
        Addition, 
        Substraction
    }
    
    [Header("Components")]
    [SerializeField] private Collider goalCollider;
    [SerializeField] private TextMeshPro goalText;
    [SerializeField] private GameObject goalVisuals;
    
    [Header("Settings")]
    [Tooltip("How to apply points when a goal is scored.")][SerializeField] private PointMethod pointMethod = PointMethod.Substraction; // How to apply points when a goal is scored
    [Tooltip("Points to add or subtract when a goal is scored.")][SerializeField] private int scorePoints = 1; // Points to add or subtract when a goal is scored
    [Tooltip("The current value of this goal.")][SerializeField] private int currentScore = 5; // Current score for this goal
    [Tooltip("The maximum score for this goal. If score is 0 and subtracted, it loops back to ten and vice versa.")][SerializeField] private int maxScore = 10; // Maximum score for this goal
    
    [Space(5)]
    [SerializeField] private float inactiveTimeOut = 2f; // Time before the goal is reset after scoring
    [SerializeField] private Color inactiveColor = Color.gray; // Color to set when the goal is inactive
    
    // Private variables
    private float inactiveTimer = 0f; // Timer for inactive time
    private Material goalMaterial; // the goal material for color changes
    private Color currentMaterialColor; // Current color of the goal material

    public void Start()
    {
        // Initialize the goal material instance
        goalMaterial = goalVisuals.GetComponent<MeshRenderer>().material;
        currentMaterialColor = goalMaterial.color;
        
        UpdateGoalText();
    }
    private void Update()
    {
        // Check if the goal is inactive and reset it after the timeout
        if (inactiveTimer > 0f)
        {
            inactiveTimer -= Time.deltaTime;
            if (inactiveTimer <= 0f)
            {
                ResetGoal();
            }
        }
    }

    public void TriggerEntered(Collider other = null)
    {
        Debug.Log("Trigger Started");
        
        // Check if the collider is not null and has a tag
        if (other == null)
        {
            Debug.LogWarning("Trigger entered with a null collider.");
            return;
        }
        
        switch (other.gameObject.tag)
        {
            case "Ball":
                Debug.Log("Goal Triggered by: " + other.gameObject.name);
                ApplyPoints();
                UpdateGoalText();
                DeactivateGoal();
                break;
        }

    }

    private void ApplyPoints()
    {
        switch (pointMethod)
        {
            case PointMethod.Addition:
                currentScore += scorePoints;
                break;
            case PointMethod.Substraction:
                currentScore -= scorePoints;
                break;
        }
        
        // If the score goes over the maximum value, reset it to zero. If the score goes below zero, reset it to the maximum value.
        if (currentScore > maxScore)
        {
            currentScore = 0;
        }
        else if (currentScore < 0)
        {
            currentScore = maxScore;
        }
    }
    
    private void UpdateGoalText()
    {
        // Update the goal text with the current score
        if (goalText != null)
        {
            goalText.text = currentScore + " / " + maxScore;
        }
        else
        {
            Debug.LogWarning("Goal Text is not assigned in the inspector.");
        }
    }


    
    // GOAL ACTIVATION METHODS
    
    private void DeactivateGoal()
    {
        inactiveTimer = inactiveTimeOut;
        goalCollider.enabled = false;
        goalMaterial.color = inactiveColor; // Set to inactive color
    }
    private void ResetGoal()
    {
        // Reset the goal state
        inactiveTimer = 0f; // Reset the inactive timer
        goalCollider.enabled = true;
        goalMaterial.color = currentMaterialColor; // Reset to original color
        
    }
}
