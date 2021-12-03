using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Pawn", menuName = "Tactical Mages/Pawn")]
public class PawnData : ScriptableObject
{
    [Tooltip("The sprite image to use for this character.")]
    public Sprite characterSprite;

    [Tooltip("The width of the pawn sprite. Default 0.6f")]
    public float spriteWidth = 0.6f;
    [Tooltip("The height of the pawn sprite. Default 0.6f")]
    public float spriteHeight = 0.6f;

    [Tooltip("Color tint adjustment to character sprite.")]
    public Color spriteTint = Color.white;

    [Tooltip("How much health does this character have?")]
    public int health = 3;

    [Tooltip("How much damage does one attack deal?")]
    public int attackDamage = 1;

    [Tooltip("How many squares can this character move in a turn?")]
    public int movementRange = 1;

    //[Tooltip("How many tiles per second can this character move?")]
    //public float movementSpeed = 1.0f;

}
