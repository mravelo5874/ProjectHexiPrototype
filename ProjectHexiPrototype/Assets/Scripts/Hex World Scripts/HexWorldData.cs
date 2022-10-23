using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexWorldData
{
    // hex grid data
    public HexCell[] hex_cells;
    public int world_radius;
    // enemy data
    public HexEntity[] enemies;
    public bool return_from_combat;
    public Vector3 combat_enemy_position;
    public Vector3 combat_enemy_rotation;
    // player data
    public HexEntity player;
    public Vector3 player_position;
    public Vector3 player_rotation;
    public int world_energy;
}
