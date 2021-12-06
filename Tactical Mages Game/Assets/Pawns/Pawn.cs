using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : MonoBehaviour
{
    [Tooltip("The pawn data object for this pawn.")]
    [SerializeField]
    private PawnData pawnData;

    [HideInInspector]
    public int playerID;

    [SerializeField]
    private Image pawnImage;

    [SerializeField, Tooltip("How fast you want this pawn to move across the tiles.")]
    float movementSpeed = 1;

    [SerializeField]
    LayerMask layerMask;
    Tile currentTile;

    // Start is called before the first frame update
    void Start()
    {
        pawnImage.sprite = pawnData.characterSprite;
        pawnImage.color = pawnData.spriteTint;
        pawnImage.rectTransform.sizeDelta = new Vector2(pawnData.spriteWidth, pawnData.spriteHeight);

        GetCurrentTile();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //public List<Tile> ShowMovement()
    //{
    //   // currentTile.HighlightTilesToMoveTo(pawnData.movementRange, playerID);
    //    return currentTile.GetTilesList(pawnData.movementRange, playerID);
    //}

    public void GetCurrentTile()
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit, 1, layerMask);

        if (hit.collider.GetComponent<Tile>())
        {
            currentTile = hit.collider.GetComponent<Tile>();
            currentTile.pawn = this;
        }
    }

    public void ShowMovement()
    {
        currentTile.GetTilesList(pawnData.movementRange, playerID);
    }

    public void Move(List<Transform> pathPositions)
    {
        //TODO: Move this character to the target location

        //Set this position to the target location (Short and temporary solution before lerp)
        //transform.position = targetLocation;
    }

    IEnumerator MoveTo(Vector3 position)
    {
        if (transform.position != new Vector3(position.x, 0.25f, position.z))
        {
            Vector3.MoveTowards(transform.position, position + Vector3.up, movementSpeed * Time.deltaTime);
            yield return null;
        }
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
