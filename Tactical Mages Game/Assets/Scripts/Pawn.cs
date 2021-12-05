using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : MonoBehaviour
{
    [Tooltip("The pawn data object for this pawn.")]
    [SerializeField]
    private PawnData pawnData;

    [SerializeField]
    private Image pawnImage;

    // Start is called before the first frame update
    void Start()
    {
        pawnImage.sprite = pawnData.characterSprite;
        pawnImage.color = pawnData.spriteTint;
        pawnImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pawnData.spriteWidth);
        pawnImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pawnData.spriteHeight);
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

    public void Attack(Pawn opponentCharacter)
    {
        //TODO: Turn to face opponent

        //TODO: Attack Animation & Sound

        //Deal damage to opponent
        opponentCharacter.TakeDamage(pawnData.attackDamage);
    }

    public void TakeDamage(int damageTaken)
    {
        //Subtract damage from health.
        pawnData.health -= damageTaken;

        //Die if health is depleted.
        if (pawnData.health <= 0)
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
