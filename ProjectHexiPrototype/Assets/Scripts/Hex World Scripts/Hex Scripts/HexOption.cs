using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexOption
{
    None, // default option
    
    // NATURE TILES
    CutDownTree, // only on forest tiles
    FishForFish, // only on pond tiles

    // FRIENDLY TILES
    UpgradeCard,
    EnterShop,
    CollectLoot,

    // ENEMY TILES
    EnterCastle, // only on castle tiles
    EnterCamp, // only on camp tiles
}
