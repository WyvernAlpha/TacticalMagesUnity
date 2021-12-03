using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Type", menuName ="Tactical Mages/Character")]
public class Character : ScriptableObject
{
    [Tooltip("The name of this character.")]
    public string characterName = "New Character";
    
    public enum CharacterTypes
    {
        Earth   =   0,
        Water   =   1,
        Fire    =   2,
        Air     =   3
    }
    [Tooltip("The character's elemental type.")]
    public CharacterTypes Type;

    [Tooltip("The default color to represent this character type.")]
    public Color DefaultColor = Color.white;

    [Tooltip("The sprite to be shown on buttons and anywhere this character is represented.")]
    public Sprite DefaultSprite;
    [Tooltip("The color to adjust the character sprite.")]
    public Color SpriteColor = Color.white;

    [Tooltip("The starting prefab pieces with which a character of this type will begin the game.")]
    public GameObject[] PawnSet;
}
