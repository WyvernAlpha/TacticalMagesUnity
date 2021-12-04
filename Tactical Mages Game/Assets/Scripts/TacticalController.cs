using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalController : MonoBehaviour
{
    public static TacticalController instance;
    public bool isTurn = false;

    [SerializeField]
    Pawn selectedPawn = null;
    Pawn previousPawn = null;

    public HashSet<Tile> selectableTiles = new HashSet<Tile>();
    public int hashSetLength;

    [SerializeField]
    Tile selectedTile = null;
    Tile previousTile = null;
    [SerializeField]
    LayerMask layerMask;

    private void Awake()
    {
        //Singleton of TacticalController instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Additional GameManager has been found and destroyed.");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    public void StartTurn(Player playerID)
    {
        Debug.Log("Turn started.");
        isTurn = true;
        StartCoroutine(TakeTurn(playerID));
    }


    IEnumerator TakeTurn(Player playerID)
    {
        while (isTurn)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.GetComponent<Tile>())
                    {
                        previousTile = selectedTile;
                        selectedTile = hit.collider.GetComponent<Tile>();
                        previousPawn = selectedPawn;
                        selectedPawn = hit.collider.GetComponent<Tile>().pawn;
                        if (selectedPawn != null && playerID.Pawns.Contains(selectedPawn.gameObject))
                        {
                            ClearPrevSelectables();
                            selectedPawn.ShowMovement();
                        }
                        else if (selectedPawn == null && previousPawn != null && selectableTiles.Contains(selectedTile))
                        {
                            //Move pawn()
                            ClearPrevSelectables();
                            MovePawn(previousPawn.gameObject, selectedTile.transform.position);

                            GameManager.instance.EndTurn();
                        }

                    }
                }


            }
            if (Input.GetKeyDown(KeyCode.F))
                ClearPrevSelectables();
             
            //Attack / Wait phase() 
            //GameManger -> CheckForVictory() 
            //End turn() 
            yield return null;
        }
    }

    

    void ClearPrevSelectables()
    {
        foreach(Tile tile in selectableTiles)
        {
            tile.Unselect();
        }
        selectableTiles.Clear();
        hashSetLength = selectableTiles.Count;
    }

    void MovePawn(GameObject pawn, Vector3 position)
    {
        pawn.transform.position = new Vector3(position.x, 0.15f, position.z);
    }
    
}
