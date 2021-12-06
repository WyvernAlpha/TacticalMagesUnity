using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Tile : MonoBehaviour
{
    public Pawn pawn;
    public Image tileImage;

    Color baseColor;
    [SerializeField]
    Color canMoveColor;
    [SerializeField]
    Color attackColor;
    [Tooltip("The cost of movement points to move onto this tile.")]
    public int cost = 0;
    public int gCost = 0;
    public int hCost = 0;

    public int fCost { get { return (gCost + cost - 1 ) + (hCost + cost - 1) ; } }

    [SerializeField]
    bool canMoveChecked = false;

    // This keeps tracks of the neighboring tiles surrounding this tile.
    
    public List<Tile> neighborTiles = new List<Tile>();

    // The layer mask to hit our tiles.
    [SerializeField]
    LayerMask layerMask;

    // Start is called before the first frame update

    void Start()
    {
        baseColor = tileImage.color;
        // Sets our neighbor tiles.
        GetNeighborTiles();
    }
    /// <summary>
    /// Sets the list of tiles by grabing the 4 tiles around this tile.
    /// </summary>
    void GetNeighborTiles()
    {
        // Intialize our hit info.
        RaycastHit hit;
        // Intialize our starting direction vector 3.
        Vector3 direction = new Vector3(-1, 0, 0);
        // For each of the four directions...
        for (int i = 0; i < 4; i++)
        {
            // Rotate the vector 3 by 90 degrees.
            direction = Quaternion.AngleAxis(90, Vector3.up) * direction;
            // Send out a raycast in specified direction from the center of this tile 1.1 meters out.
            if (Physics.Raycast(transform.position, direction, out hit, 1.1f, layerMask))
            {
                // When we hit the tile, add the tile to our list of tiles.
                neighborTiles.Add(hit.collider.GetComponent<Tile>());
            }
        }
    }

    
    public void GetTilesList(int maxCost, int playerID)
    {
        // This starting tile has been checked to be moveable to.
        canMoveChecked = true;
        // For each of the tile's neighboring tiles...
        for (int i = 0; i < neighborTiles.Count; i++)
        {
            try
            {
                // If the neighbor has been checked already AND can afford to move there...
                if ((neighborTiles[i].canMoveChecked == false || !TacticalController.instance.selectableTiles.Contains(neighborTiles[i].neighborTiles[i])) && (maxCost - neighborTiles[i].cost) >= 0)
                {
                    if (neighborTiles[i].pawn != null)
                    {
                        Player currentPlayer = GameManager.instance.GetPlayer(playerID);
                        if (!currentPlayer.Pawns.Contains(neighborTiles[i].pawn.gameObject))
                        {
                            TacticalController.instance.selectableTiles.Add(neighborTiles[i]);
                            neighborTiles[i].tileImage.color = attackColor;
                        }

                    }
                    else
                    {
                        neighborTiles[i].tileImage.color = canMoveColor;
                        TacticalController.instance.selectableTiles.Add(neighborTiles[i]);

                        List<Tile> tiles = new List<Tile>();
                        neighborTiles[i].GetTilesList((maxCost - neighborTiles[i].cost), playerID);


                    }
                }
            }
            catch { continue; };
        }
    }

    public void Unselect()
    {
        canMoveChecked = false;
        tileImage.color = baseColor;
    }
}
