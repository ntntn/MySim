using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeController : MonoBehaviour
{
    bool buildModeIsObjects = false;
    [SerializeField]
    TileType buildModeTile = TileType.Floor;
    string buildModeObjectType;

    public void SetMode_BuildTree()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Tree;
    }
    public void SetMode_BuildRock()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Rock;
    }
    public void SetMode_BuildFloor()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Floor;
    }

    public void SetMode_Bulldoze()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Empty;
    }

    public void SetMode_BuildFurniture(string objectType)
    {
        buildModeIsObjects = true;
        buildModeObjectType = objectType;
    }

    public void DoBuild(Tile t)
    {
        if (buildModeIsObjects == true)
        {

            string furnitureType = buildModeObjectType;

            if (WorldController.Instance.world.IsFurniturePlacementValid(furnitureType, t) && t.pendingFurnitureJob == null)
            {
                // This tile position is valid for this furniture
                // Create a job for it to be build

                Job j = new Job(t, furnitureType, (theJob) => {
                    WorldController.Instance.world.PlaceFurniture(furnitureType, theJob.tile);

                    // FIXME: I don't like having to manually and explicitly set
                    // flags that preven conflicts. It's too easy to forget to set/clear them!
                    t.pendingFurnitureJob = null;
                }
                );

                // FIXME: I don't like having to manually and explicitly set
                // flags that preven conflicts. It's too easy to forget to set/clear them!
                t.pendingFurnitureJob = j;
                j.RegisterJobCancelCallback((theJob) => { theJob.tile.pendingFurnitureJob = null; });

                // Add the job to the queue
                WorldController.Instance.world.jobQueue.Enqueue(j);

            }



        }
        else
        {
            // We are in tile-changing mode.
            t.Type = buildModeTile;
        }

    }
}
