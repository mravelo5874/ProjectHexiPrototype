using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexType
{
    // default tile types
    None,
    Init,

    // NATURE TILES
    Plain,
    Forest,
    Pond,
    Mountain,

    // FRIENDLY TILES
    Village,
    Quarry,
    Chest,

    // ENEMY TILES
    Castle,
    Camp,
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
            case HexType.Plain:
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
            case HexType.Chest:
                options.Add(new HexOption(HexOption.OptionType.CollectLoot, 0));
                break;
            case HexType.Castle:
                options.Add(new HexOption(HexOption.OptionType.EnterCastle, 0));
                break;
            case HexType.Camp:
                options.Add(new HexOption(HexOption.OptionType.EnterCamp, 0));
                break;
        }
        return options;
    }
}
