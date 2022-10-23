using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HexTileData")]
public class HexTileData : ScriptableObject // used to store visual data for each hex type
{
    public HexType hex_type;
    public Texture2D tile_texture;
}