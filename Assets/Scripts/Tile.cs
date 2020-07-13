using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Empty, Floor, Tree, Rock };

public class Tile
{
    private TileType _type = TileType.Empty;
    public TileType Type
    {
        get
        {
            return _type;
        }
        set
        {
            TileType oldType = _type;
            _type = value;

            if (cbTileTypeChanged != null && oldType != _type)
                cbTileTypeChanged(this);
        }
    }


    Inventory inventory;

    public Furniture furniture { get; protected set; }

    public Job pendingFurnitureJob;

    World world;
    private int x;
    private int y;
    public int X { get => this.x; set => this.x = value; }
    public int Y { get => y; set => y = value; }

    Action<Tile> cbTileTypeChanged;



    LooseObject looseObject;
    InstalledObject installedObject;



    //public Tile()
    //{
    //    world = WorldController.Instance.World;
    //    this.x = 0;
    //    this.y = 0;
    //}

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.Y = y;
    }

    public void RegisterTileTypeChangedCallback(Action<Tile> callback) { cbTileTypeChanged += callback; }

    public void UnregisterTileTypeChangedCallback(Action<Tile> callback) { cbTileTypeChanged -= callback; }


    public bool PlaceFurniture(Furniture objInstance)
    {
        if (objInstance == null)
        {
            furniture = null;
            return true;
        }

        if (furniture!=null)
        {
            Debug.LogError("Tile already has InstalledObject");
            return false;
        }

        furniture = objInstance;
        return true;
    }

    public bool IsNeighbour(Tile tile)
    {
        if (Math.Abs(this.X - tile.X) == 1 || Math.Abs(this.Y-tile.Y)==1)
            return true;

        if (this.Y == tile.y && (this.X == tile.X + 1 || this.x == tile.x - 1))
            return true;

        return false;
    }
}
