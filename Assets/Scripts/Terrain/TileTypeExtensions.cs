﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileTypeExtensions
{
    public static bool IsRiverTile(this TileType type)
    {
        switch(type)
        {
            case TileType.RiverBendLeft:
            case TileType.RiverBendRight:
            case TileType.RiverEnd:
            case TileType.RiverMouth:
            case TileType.RiverStraight:
                return true;
            default: return false;
        }
    }

    public static bool IsCoastalTile(this TileType type)
    {
        switch (type)
        {
            case TileType.CoastInnerCorner:
            case TileType.CoastOuterCorner:
            case TileType.CoastStraight:
                return true;
            default: return false;
        }
    }
}