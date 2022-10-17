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

    // private vars
    private MyObject my_object;
    private HexCell current_cell;
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
        current_cell = cell;
        can_move = false;
        HexWorldManager.instance.ClearHexOptions(); // clear current cell hex options
        my_object.MoveToTransform(current_cell.transform, ENTITY_MOVE_DURATION, true, false);
        yield return new WaitForSeconds(ENTITY_MOVE_DURATION);
        HexWorldManager.instance.ConsumeWorldEnergy(); // consume one world energy
        HexWorldManager.instance.SetHexOptions(cell); // set current cell hex options
        can_move = true;
    }
}
