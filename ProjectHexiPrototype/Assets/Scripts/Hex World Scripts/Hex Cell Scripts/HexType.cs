using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexType
{
    // default tile types
    None,
    Init,

    // NATURE TILES
    Field,
    FlowerField,
    Forest,
    Pond,
    Spring,
    Mountain,
    Cave,
    DesertDunes,

    // FRIENDLY / NEUTRAL TILES
    Farm,
    Village,
    Peddler,
    Quarry,
    Ruins,
    Obelisk,
    Statue,
    Temple,

    // ENEMY TILES
    GoblinCamp,
    DungeonTomb,
    SpookySwamp,
    FinalCastle
}

public static class HexTypeExtensions
{
    public static float GetTerrainIndex(this HexType type)
    {
        switch (type)
        {
            default:
                return 0f;
            case HexType.Init:
                return 1f;
            case HexType.Quarry:
                return 2f;
            case HexType.Mountain:
                return 3f;
        }
        // return (float)Random.Range(0, 2);
    }

    public static List<HexOption> GetHexOptions(this HexType type)
    {
        List<HexOption> options = new List<HexOption>();
        switch (type)
        {
            default:
            case HexType.None:
                break;
            case HexType.Init:
                options.Add(new HexOption(HexOption.OptionType.StartHex, 0));
                break;
            case HexType.Field:
                break;
            case HexType.Forest:
                options.Add(new HexOption(HexOption.OptionType.CutDownTree, 1));
                options.Add(new HexOption(HexOption.OptionType.CutDownTree, 1));
                options.Add(new HexOption(HexOption.OptionType.CutDownTree, 1));
                break;
            case HexType.Pond:
                options.Add(new HexOption(HexOption.OptionType.FishForFish, 2));
                break;
            case HexType.Mountain:
                break;
            case HexType.Village:
                options.Add(new HexOption(HexOption.OptionType.UpgradeCard, 2));
                options.Add(new HexOption(HexOption.OptionType.EnterShop, 0));
                break;
            case HexType.Quarry:
                options.Add(new HexOption(HexOption.OptionType.MineForLoot, 1));
                options.Add(new HexOption(HexOption.OptionType.MineForLoot, 1));
                options.Add(new HexOption(HexOption.OptionType.MineForLoot, 1));
                break;
            case HexType.FinalCastle:
                options.Add(new HexOption(HexOption.OptionType.EnterCastle, 0));
                break;
            case HexType.GoblinCamp:
                options.Add(new HexOption(HexOption.OptionType.EnterCamp, 0));
                break;
        }
        return options;
    }

    public static int GetElevation(this HexType type)
    {
        switch (type)
        {
            default:
            case HexType.None:
            case HexType.Field:
            case HexType.Pond:
            case HexType.Init:
                return 0;
            case HexType.Forest:
            case HexType.Village:
                return Random.Range(0, 2);
            case HexType.Quarry:
            case HexType.FinalCastle:
            case HexType.GoblinCamp:
                return Random.Range(1, 3);
            case HexType.Mountain:
                return 3;
        }
    }
}
