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
    [SerializeField] private Material goalMaterial;
    
    [Header("Settings")]
    [Tooltip("How to apply points when a goal is scored.")][SerializeField] private PointMethod pointMethod = PointMethod.Substraction; // How to apply points when a goal is scored
    [Tooltip("Points to add or subtract when a goal is scored.")][SerializeField] private int scorePoints = 1; // Points to add or subtract when a goal is scored
    [Tooltip("The current value of this goal.")][SerializeField] private int currentScore = 5; // Current score for this goal
    
    [Space(10)]
        [SerializeField] private float inactiveTimeOut = 2f; // Time before the goal is reset after scoring
    
    
}
