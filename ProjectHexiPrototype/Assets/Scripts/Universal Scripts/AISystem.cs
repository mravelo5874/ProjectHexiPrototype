using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
    public static AISystem instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public EnemyData skeleton_data;

    public List<EnemyData> DetermineEnemies()
    {
        List<EnemyData> enemy_data = new List<EnemyData>();
        enemy_data.Add(skeleton_data);
        return enemy_data;
    }

    // TODO fix this 
    public List<HexType> GetSpecialHexTypes(int layer_index)
    {
        List<HexType> hex_types = new List<HexType>();
        // return init hex type if layer is 0
        if (layer_index == 0 || layer_index == 1)
        {
            return null;
        }
        // add special hex types based on layer index
        switch (layer_index)
        {
            default:
            case 2:
                hex_types.Add(HexType.Village);
                break;
            case 3:
                hex_types.Add(HexType.GoblinCamp);
                hex_types.Add(HexType.DungeonTomb);
                hex_types.Add(HexType.SpookySwamp);
                break;
            case 4:
                hex_types.Add(HexType.FinalCastle);
                break;
        }
        return hex_types;
    }

    // TODO fix this too
    public HexType DetermineHexType(int layer_index)
    {
        // return init hex type if layer is 0
        if (layer_index == 0)
        {
            return HexType.Init;
        }

        List<HexType> potential_hex_types = new List<HexType>();
        switch (layer_index)
        {
            default:
            case 1:
                potential_hex_types.Add(HexType.Field);
                potential_hex_types.Add(HexType.FlowerField);
                potential_hex_types.Add(HexType.Forest);
                break;
            case 2:
                potential_hex_types.Add(HexType.Field);
                potential_hex_types.Add(HexType.FlowerField);
                potential_hex_types.Add(HexType.Forest);
                potential_hex_types.Add(HexType.Pond);
                potential_hex_types.Add(HexType.Spring);
                potential_hex_types.Add(HexType.Cave);
                potential_hex_types.Add(HexType.DesertDunes);
                break;
            case 3:
                potential_hex_types.Add(HexType.Field);
                potential_hex_types.Add(HexType.FlowerField);
                potential_hex_types.Add(HexType.Forest);
                potential_hex_types.Add(HexType.Pond);
                potential_hex_types.Add(HexType.Spring);
                potential_hex_types.Add(HexType.Cave);
                potential_hex_types.Add(HexType.DesertDunes);
                potential_hex_types.Add(HexType.Quarry);
                break;
            case 4:
                potential_hex_types.Add(HexType.Field);
                potential_hex_types.Add(HexType.FlowerField);
                potential_hex_types.Add(HexType.Forest);
                potential_hex_types.Add(HexType.Pond);
                potential_hex_types.Add(HexType.Spring);
                potential_hex_types.Add(HexType.Cave);
                potential_hex_types.Add(HexType.DesertDunes);
                potential_hex_types.Add(HexType.Quarry);
                potential_hex_types.Add(HexType.Mountain);
                break;

        }
        // return random tile based on layer
        return potential_hex_types[Random.Range(0, potential_hex_types.Count)];
    }
}
