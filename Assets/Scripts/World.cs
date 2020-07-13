using UnityEngine;
using System.Collections.Generic;
using System;

public class World
{
    Tile[,] tiles;
    List<Character> characters;

    Dictionary<string, Furniture> furniturePrototypes;


    public int Width { get; protected set; }


    public int Height { get; protected set; }

    Action<Furniture> cbFurnitureCreated;
    Action<Tile> cbTileChanged;


    public JobQueue jobQueue;


    public World(int width = 100, int height = 100)
    {
        jobQueue = new JobQueue();

        Width = width;
        Height = height;

        tiles = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
                tiles[x, y].RegisterTileTypeChangedCallback(OnTileChanged);
            }
        }

        Debug.Log("World created with " + (Width * Height) + " tiles.");

        CreateFurniturePrototypes();

        characters = new List<Character>();
        Character c = new Character(tiles[Width / 2, Height / 2]); 
    }

    void CreateFurniturePrototypes()
    {
        furniturePrototypes = new Dictionary<string, Furniture>();

        furniturePrototypes.Add("Wall",
            Furniture.CreatePrototype(
                                "Wall",
                                0,  // Impassable
                                1,  // Width
                                1,  // Height
                                true // Links to neighbours and "sort of" becomes part of a large object
                            )
        );
    }


    public void RandomizeTiles(float empty, float treeRock, float wall)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float rnd = UnityEngine.Random.Range(0, 1001);

                if (rnd <= empty)
                {
                    tiles[x, y].Type = TileType.Empty;
                }

                else if (rnd <= treeRock)
                {
                    if (rnd % 2 == 0)
                        tiles[x, y].Type = TileType.Tree;
                    else
                        tiles[x, y].Type = TileType.Rock;
                }

                else
                {
                    tiles[x, y].Type = TileType.Floor;
                }

            }
        }
    }
    public void RandomizeTiles()
    {
        Debug.Log("RandomizeTiles");
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    tiles[x, y].Type = TileType.Empty;
                }
                else
                {
                    tiles[x, y].Type = TileType.Floor;
                }
            }
        }

    }

    public Tile GetTileAt(int x, int y)
    {
        if (x > Width || x < 0 || y > Height || y < 0)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }

        Debug.Log(tiles[x, y].Type);
        return tiles[x, y];
    }


    public void PlaceFurniture(string objectType, Tile t)
    {
        //Debug.Log("PlaceInstalledObject");
        // TODO: This function assumes 1x1 tiles -- change this later!

        if (furniturePrototypes.ContainsKey(objectType) == false)
        {
            Debug.LogError("furniturePrototypes doesn't contain a proto for key: " + objectType);
            return;
        }

        Furniture obj = Furniture.PlaceInstance(furniturePrototypes[objectType], t);

        if (obj == null)
        {
            // Failed to place object -- most likely there was already something there.
            return;
        }

        if (cbFurnitureCreated != null)
        {
            cbFurnitureCreated(obj);
        }
    }

    public void RegisterFurnitureCreated(Action<Furniture> callbackfunc)
    {
        cbFurnitureCreated += callbackfunc;
    }

    public void UnregisterFurnitureCreated(Action<Furniture> callbackfunc)
    {
        cbFurnitureCreated -= callbackfunc;
    }

    public void RegisterTileChanged(Action<Tile> callbackfunc)
    {
        cbTileChanged += callbackfunc;
    }

    public void UnregisterTileChanged(Action<Tile> callbackfunc)
    {
        cbTileChanged -= callbackfunc;
    }

    void OnTileChanged(Tile t)
    {
        if (cbTileChanged == null)
            return;

        cbTileChanged(t);
    }

    public bool IsFurniturePlacementValid(string furnitureType, Tile t)
    {
        return furniturePrototypes[furnitureType].IsValidPosition(t);
    }

    public Furniture GetFurniturePrototype(string objectType)
    {
        return furniturePrototypes[objectType];
    }
}















