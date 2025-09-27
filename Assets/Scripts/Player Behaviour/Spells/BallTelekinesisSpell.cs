using UnityEngine;

[CreateAssetMenu(menuName = "Spell/BallTelekinesisSpell", fileName = "BallTelekinesisSpell")]
public class BallTelekinesisSpell : Spell
{
    
    // Implementation for Ball Telekinesis Spell
    
    // Fais moi un debugLog qui lors de l'appel de la fonction DoSpell affiche "Ball Telekinesis Spell"
    // puis fais moi un debugLog qui indique le vecteur de direction de la balle lors de l'inptut
    
    public override void DoSpell(Transform player)
    {
        Debug.Log("Ball Telekinesis Spell");
        Debug.Log("Vecteur de direction de la balle : " + GameManager.Instance.Ball.transform.forward);
        Debug.Log("Vecteur de direction du joueur : " + player.forward);

        Rigidbody ballRb = GameManager.Instance.Ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            float currentSpeed = ballRb.linearVelocity.magnitude;
            ballRb.linearVelocity = player.forward.normalized * currentSpeed;
        }
        
        
    }
}
