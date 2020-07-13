using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture
{
    public Tile tile { get; protected set; }

    public string objectType { get; protected set; }

    float movementCost;

    int width;
    int height;

    public bool LinksToNeighbour { get; protected set; }

    Action<Furniture> cbOnChanged;

    Func<Tile, bool> funcPositionValidation;

    protected Furniture() { }

    static public Furniture CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height = 1, bool LinksToNeighbour = false)
    {
        Furniture obj = new Furniture();

        obj.objectType = objectType;
        obj.movementCost = movementCost;
        obj.width = width;
        obj.height = height;
        obj.LinksToNeighbour = LinksToNeighbour;

        obj.funcPositionValidation = obj.__IsValidPosition;

        return obj;
    }

    static public Furniture PlaceInstance(Furniture proto, Tile tile)
    {
        Furniture obj = new Furniture();
        obj.objectType = proto.objectType;
        obj.movementCost = proto.movementCost;
        obj.width = proto.width;
        obj.height = proto.height;
        obj.LinksToNeighbour = proto.LinksToNeighbour;

        obj.tile = tile;

        if (tile.PlaceFurniture(obj)==false)
        {
            return null; 
        }

        return obj;
    }

    public void RegisterOnChangedCallback(Action<Furniture> callbackFunc)
    {
        cbOnChanged += callbackFunc;
    }

    public void UnregisterOnChangedCallback(Action<Furniture> callbackFunc)
    {
        cbOnChanged -= callbackFunc;
    }

    public bool IsValidPosition(Tile t)
    {
        return funcPositionValidation(t);
    }

    public bool __IsValidPosition(Tile t)
    {
        if (t.Type != TileType.Floor)
        {
            return false;
        }

        // Make sure tile doesn't already have furniture
        if (t.furniture != null)
        {
            return false;
        }

        return true;
    }

    public bool __IsValidPosition_Door(Tile t)
    {
        if (__IsValidPosition(t) == false)
            return false;

        // Make sure we have a pair of E/W walls or N/S walls

        return true;
    }
}
