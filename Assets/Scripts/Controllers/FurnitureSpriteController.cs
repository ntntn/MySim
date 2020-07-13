using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FurnitureSpriteController : MonoBehaviour
{    
    Dictionary<Furniture, GameObject> furnitureGameObjectMap;

    Dictionary<string, Sprite> furnitureSprites;

    World world { get { return WorldController.Instance.world; } }

    [SerializeField]
    [Range(0, 100)]
    private float emptyChance;
    [SerializeField]
    [Range(0, 100)]
    private float treeChance;
    [SerializeField]
    [Range(0, 100)]
    private float wallChance;



    void Start()
    {
        LoadSprites();

        furnitureGameObjectMap = new Dictionary<Furniture, GameObject>();  

        world.RegisterFurnitureCreated(OnFurnitureCreated);
    }


    void LoadSprites()
    {
        furnitureSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Furniture/");

        Debug.Log("LOADED RESOURCES");
        foreach (Sprite s in sprites)
        {
            Debug.Log(s);
            furnitureSprites.Add(s.name, s);
        }
    }
         

    public void OnFurnitureCreated(Furniture furniture)
    {
        GameObject furniture_go = new GameObject();

        furnitureGameObjectMap.Add(furniture, furniture_go);

        furniture_go.name = furniture.objectType + "_" + furniture.tile.X + "_" + furniture.tile.Y;
        furniture_go.transform.position = new Vector3(furniture.tile.X, furniture.tile.Y, 0);
        furniture_go.transform.SetParent(this.transform, true);

        furniture_go.AddComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furniture);  // FIXME
        furniture_go.AddComponent<ShadowEffect>();

        furniture.RegisterOnChangedCallback(OnFurnitureChanged);
    }

    void OnFurnitureChanged (Furniture obj)
    {
        GameObject furn_go = furnitureGameObjectMap[obj];
        furn_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(obj);
    }


    public Sprite GetSpriteForFurniture(Furniture obj)
    {
        string spriteName = obj.objectType;

        if (!obj.LinksToNeighbour)
        {
            return furnitureSprites[spriteName];
        }
        
        return furnitureSprites[spriteName]; 
    }

    public Sprite GetSpriteForFurniture(string objectType)
    {
        if (furnitureSprites.ContainsKey(objectType))
        {
            return furnitureSprites[objectType];
        }

        if (furnitureSprites.ContainsKey(objectType + "_"))
        {
            return furnitureSprites[objectType + "_"];
        }

        Debug.LogError("GetSpriteForFurniture -- No sprites with name: " + objectType);
        return null;
    }

    void OnFurnitureChanged(InstalledObject obj)
    {
        Debug.Log("onchangedinstalledgo");
    }
}