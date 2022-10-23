using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEntity : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float ENTITY_MOVE_DURATION = 1f;

    void Awake()
    {
        my_object = GetComponent<MyObject>();
    }

    private MyObject my_object;
    public MyObject GetMyObject() { return my_object; } // public getter
    private HexCell current_cell;
    public HexCell GetCurrentHexCell() { return current_cell; } // public getter
    private bool can_move = false;

    public void SetStartHexCell(HexCell cell)
    {
        current_cell = cell;
        transform.localPosition = cell.transform.localPosition;
        can_move = true;
    }

    public void GoToHexCell(HexCell cell)
    {
        // return if player is not allowed to move
        if (!can_move)
        {
            return;
        }

        bool is_adjacent = false;
        // check if hex cell is adjacent to current cell
        List<Vector3Int> neighbor_coords = HexMetrics.GetNeighborCoordinates(current_cell.GetHexCoordinates());
        foreach (Vector3Int coords in neighbor_coords)
        {
            if (cell.GetHexCoordinates() == coords)
            {
                is_adjacent = true;
            }
        }

        if (is_adjacent)
        {
            StartCoroutine(GoToHexCellRoutine(cell));
        }
    }

    private IEnumerator GoToHexCellRoutine(HexCell cell)
    {
        can_move = false;
        HexWorldManager.instance.ClearAdjacentHexCellOutlines(current_cell); // clear hex cell outlines
        HexWorldManager.instance.ClearHexOptions(); // clear current cell hex options
        HexWorldManager.instance.MovePlayerToHexCell(cell, HexMetrics.GetDirectionBetweenHexCells(current_cell, cell));
        current_cell.Discarded = true; // discard current hex cell
        current_cell = cell; // set new target hex cell
        yield return new WaitForSeconds(ENTITY_MOVE_DURATION);
        HexWorldManager.instance.ConsumeWorldEnergy(); // consume one world energy
        HexWorldManager.instance.SetHexOptions(cell); // set current cell hex options
        if (!HexWorldManager.instance.EnemyInCell(cell) && !cell.Discarded) // only enter cell if enemy is not present and cell is not discarded
        {
            HexWorldManager.instance.EnterHexCell();
        }
        else if (!HexWorldManager.instance.EnemyInCell(cell)) // else show available adjacent outlines
        {
            HexWorldManager.instance.ShowAvailableAdjacentHexCellOutlines(cell);
        }
        HexWorldManager.instance.UpdateUI();
        can_move = true;
    }

    public void Delete()
    {
        StartCoroutine(DeleteRoutine());
    }
    private IEnumerator DeleteRoutine()
    {
        my_object.SquishyChangeScale(1.1f, 0f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
}
