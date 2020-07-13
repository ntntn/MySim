using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSpriteController : MonoBehaviour
{
    FurnitureSpriteController fsc;
    Dictionary<Job, GameObject> jobGameObjectMap;


    void Start()
    {
        jobGameObjectMap = new Dictionary<Job, GameObject>();
        fsc = GameObject.FindObjectOfType<FurnitureSpriteController>();

        // FIXME: No such thing as a job queue yet!
        WorldController.Instance.world.jobQueue.RegisterJobCreationCallback(OnJobCreated);
    }

    void OnJobCreated(Job job)
    {
        //Sprite sprite = fsc.GetSpriteForFurniture(job);
        GameObject job_go = new GameObject();

        job_go.name = "JOB_" + job.jobObjectType + "_" + job.tile.X + "_" + job.tile.Y;
        job_go.transform.position = new Vector3(job.tile.X, job.tile.Y, 0);
        job_go.transform.SetParent(this.transform, true);

        SpriteRenderer sr = job_go.AddComponent<SpriteRenderer>();
        sr.sprite = fsc.GetSpriteForFurniture(job.jobObjectType);
        sr.color = new Color(0.5f, 1f, 0.5f, 0.25f);

        job.RegisterJobCompleteCallback(OnJobEnded);
        job.RegisterJobCancelCallback(OnJobEnded);
    }

    void OnJobEnded(Job job)
    {
        GameObject job_go = jobGameObjectMap[job];

        job.UnregisterJobCompleteCallback(OnJobEnded);
        job.UnregisterJobCancelCallback(OnJobEnded);

        Destroy(job_go);
    }
    void Update()
    {
        
    }
}
