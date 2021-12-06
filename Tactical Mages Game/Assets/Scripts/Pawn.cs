using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : MonoBehaviour
{
    [Tooltip("The pawn data object for this pawn.")]
    [SerializeField]
    private PawnData pawnData;
    
    private int health = 3;
    private int attackDamage = 1;
    private float attackRotationSpeed = 1.0f;
    private float attackMovementSpeed = 1.5f;
    private int movementRange = 1;

    [SerializeField]
    private Image pawnImage;

    // Start is called before the first frame update
    void Start()
    {
        pawnImage.sprite = pawnData.characterSprite;
        pawnImage.color = pawnData.spriteTint;
        pawnImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pawnData.spriteWidth);
        pawnImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pawnData.spriteHeight);

        health = pawnData.health;
        attackDamage = pawnData.attackDamage;
        attackRotationSpeed = pawnData.attackRotationSpeed;
        attackMovementSpeed = pawnData.attackMovementSpeed;
        movementRange = pawnData.movementRange;
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 targetLocation)
    {
        //TODO: Move this character to the target location

        //Set this position to the target location (Short and temporary solution before lerp)
        //transform.position = targetLocation;
    }

    public void Attack(Pawn opponentPawn)
    {
        StartCoroutine(DoAttack(opponentPawn));
    }

    private IEnumerator DoAttack(Pawn opponentPawn)
    {
        //Store starting rotation
        Quaternion originalRotation = transform.rotation;

        //Turn to face opponent
        bool isFacingOpponent = false;
        Vector3 rotationToOpponent = opponentPawn.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationToOpponent);
        while (!isFacingOpponent)
        {
            if (transform.rotation == targetRotation)
            {
                isFacingOpponent = true;
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, attackRotationSpeed);               
            }
            
            yield return null;
        }

        //Advance to Attack
        Vector3 originalPosition = transform.position;
        bool isAdvancing = true;
        while (isAdvancing)
        {            
            //Move pawn towards opponent
            transform.position = Vector3.MoveTowards(transform.position, opponentPawn.transform.position, attackMovementSpeed * Time.deltaTime);

            //If target is reached, play hit sound and break while loop
            if (transform.position == opponentPawn.transform.position)
            {                
                isAdvancing = false;

                AudioManager.instance.PlayOneShot(pawnData.attackSound, pawnData.attackSoundVolume);                
            }

            yield return null;
        }

        //Return to Original Tile
        bool isRetreating = true;
        while (isRetreating)
        {
            //Move pawn back to original position
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, attackMovementSpeed * Time.deltaTime);

            //If position is reached, break while loop
            if (transform.position == originalPosition)
            {
                isRetreating = false;
            }

            yield return null;
        }

        //Return to Original Rotation     
        while (isFacingOpponent)
        { 
            if (transform.rotation == originalRotation)
            {
                isFacingOpponent = false;
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, attackRotationSpeed);
            }

            yield return null;
        }

        //Deal damage to opponent
        opponentPawn.TakeDamage(attackDamage);

        yield return null;
    }

    public void TakeDamage(int damageTaken)
    {
        //Subtract damage from health.
        health -= damageTaken;
        Debug.Log($"{name} has a health of {health}");

        //Die if health is depleted.
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //TODO: Sound & Death Animation

        Debug.Log($"{this.name} has been killed.");
        Destroy(this.gameObject);
    }
}
