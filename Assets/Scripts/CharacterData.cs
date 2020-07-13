using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData:ScriptableObject
{
    public float X
    {
        get
        {
            return Mathf.Lerp(currentTile.X, destinationTile.X, movementPercentage);
        }
    }
    public float Y
    {
        get
        {
            return Mathf.Lerp(currentTile.Y, destinationTile.Y, movementPercentage);
        }
    }

    Tile currentTile;
    Tile destinationTile; //if we aren't moving destTile = currentTile
    float movementPercentage;

    float speed = 2f;

    public Character(Tile tile)
    {
        currentTile = destinationTile = tile;
    }

    public void Update()
    {
        if (currentTile == destinationTile)
            return;

        float distanceToTravel = Mathf.Sqrt(Mathf.Pow(currentTile.X - destinationTile.X, 2) + Mathf.Pow(currentTile.Y - destinationTile.Y, 2));

        float distanceThisFrame = speed * Time.deltaTime;

        float percentageThisFrame = distanceToTravel / distanceToTravel;

        movementPercentage += percentageThisFrame;

        if (movementPercentage>=1)
        {
            currentTile = destinationTile;
            movementPercentage = 0;
        }
    }

    public void SetDestination(Tile tile)
    {
        destinationTile = tile;
    }
}
