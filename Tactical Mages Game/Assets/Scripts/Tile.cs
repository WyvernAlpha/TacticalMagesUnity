using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Pawn pawn;
    public Image tileImage;
    [Tooltip("The cost of movement points to move onto this tile.")]
    public int cost = 0;
    // This keeps tracks of the neighboring tiles surrounding this tile.
    [HideInInspector]
    public List<Tile> neighborTiles = new List<Tile>();

    // The layer mask to hit our tiles.
    [SerializeField]
    LayerMask layerMask;

    // Start is called before the first frame update
    
    void Start()
    {
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
        for (int i = 0; i < 4; i ++)
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

    public void HighlightTilesToMoveTo(int maxCost)
    {
        for(int i = 0; i < neighborTiles.Count; i++)
        {
            if(neighborTiles[i].cost < maxCost)
            {
                neighborTiles[i].tileImage.color = Color.blue;
                neighborTiles[i].HighlightTilesToMoveTo(maxCost - neighborTiles[i].cost);
            }
            else
                neighborTiles[i].tileImage.color = Color.white;
        }
    }

}
