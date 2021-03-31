using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverConstructor
{
    
    public static TerrainData GenerateRiver(TerrainData data, TerrainSettings settings)
    {
        // Find all potential river starting places
        List<TileData> potentialStarts = new List<TileData>();
        foreach (var tile in data.tiles)
        {
            if (tile.type == TileType.CoastStraight)
            {
                potentialStarts.Add(tile);
            }
        }
        if (potentialStarts.Count < settings.numberOfRivers)
        {
            return data;
        }
        for (int i = 0; i < settings.numberOfRivers; i++)
        {
            TileData start = potentialStarts[Random.Range(0, potentialStarts.Count)]; // pick random start
            potentialStarts.Remove(start);
            TileData oceanTile = data.GetEdgeAdjacentOceanTiles(start.xCoordinate, start.zCoordinate)[0];
            start.type = TileType.RiverMouth;
            // Construct river
            List<TileData> riverPath = new List<TileData>();
            riverPath.Add(start);
            int nextXCoordinate = start.xCoordinate + (start.xCoordinate - oceanTile.xCoordinate);
            int nextZCoordinate = start.zCoordinate + (start.zCoordinate - oceanTile.zCoordinate);
            TileData currentTile = data.GetTileAtCoordinates(nextXCoordinate, nextZCoordinate);
            currentTile.type = TileType.RiverStraight;
            TileData nextTile = null;
            for (int j = 0; j < settings.riverMaxLength; j++)
            {
                List<TileData> potentialNextTiles = data.GetEdgeAdjacentLandTiles(currentTile.xCoordinate, currentTile.zCoordinate);
                potentialNextTiles.Remove(riverPath[j]);
                // Remove all left turn tiles
                for (int k = 0; k < potentialNextTiles.Count; k++)
                {
                    Vector2 pastDirection = new Vector2(riverPath[j].xCoordinate - currentTile.xCoordinate, riverPath[j].zCoordinate - currentTile.zCoordinate);
                    Vector2 futureDirection = new Vector2(potentialNextTiles[k].xCoordinate - currentTile.xCoordinate, potentialNextTiles[k].zCoordinate - currentTile.zCoordinate);
                    if (Vector2.SignedAngle(pastDirection, futureDirection) == -90)
                    {
                        potentialNextTiles.Remove(potentialNextTiles[k]);
                    }
                }
                // If no more potential tiles, end river
                if (potentialNextTiles.Count == 0 || j == settings.riverMaxLength - 1)
                {
                    currentTile.type = TileType.RiverEnd;
                }
                else // otherwise make either straight or right turn
                {
                    nextTile = potentialNextTiles[Random.Range(0, potentialNextTiles.Count)];
                    Vector2 directionOne = new Vector2(riverPath[j].xCoordinate - currentTile.xCoordinate, riverPath[j].zCoordinate - currentTile.zCoordinate);
                    Vector2 directionTwo = new Vector2(nextTile.xCoordinate - currentTile.xCoordinate, nextTile.zCoordinate - currentTile.zCoordinate);
                    if (Vector2.SignedAngle(directionOne, directionTwo) == 90)
                    {
                        currentTile.type = TileType.RiverBendRight;
                    }
                    else
                    {
                        currentTile.type = TileType.RiverStraight;
                    }
                }

                // correctly orient tile
                Vector3 facingDirection = new Vector3(riverPath[j].xCoordinate - currentTile.xCoordinate, 0, riverPath[j].zCoordinate - currentTile.zCoordinate);
                facingDirection = facingDirection.normalized;
                if (facingDirection == Vector3.left)
                {
                    currentTile.Rotate(0, 90, 0);
                    currentTile.AddPosition(0, 0, settings.tileSize);
                }
                else if (facingDirection == Vector3.forward)
                {
                    currentTile.Rotate(0, 180, 0);
                    currentTile.AddPosition(settings.tileSize, 0, settings.tileSize);
                }
                else if (facingDirection == Vector3.right)
                {
                    currentTile.Rotate(0, -90, 0);
                    currentTile.AddPosition(settings.tileSize, 0, 0);
                }
                riverPath.Add(currentTile);
                currentTile = nextTile;
            }
        }
        return data;
    }

}
