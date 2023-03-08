using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSurfManager : MonoBehaviour
{
    public List<GameObject> arenaEnemies;
    private GameObject pitWall;
    private GameObject crowdSurfPoint;
    private bool hasTransformed = false;
    void Start()
    {
        pitWall = transform.Find("Pit Wall").gameObject;
        pitWall.gameObject.SetActive(true);
        crowdSurfPoint = transform.Find("Crowdsurf").gameObject;
        crowdSurfPoint.SetActive(false);
        hasTransformed = false;
    }

    void Update()
    {
        if (arenaEnemies.Count == 0 && !hasTransformed)
        {
            TransformCrowdsurfer();
        }
    }

    private void TransformCrowdsurfer()
    {
        transform.Find("Crowdsurf").gameObject.GetComponent<ArenaManager>().canTransport = true;
        hasTransformed = true;
        pitWall.SetActive(false);
        crowdSurfPoint.SetActive(true);
    }
}
