using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSurfManager : MonoBehaviour
{
    public List<GameObject> arenaEnemies;
    private GameObject pitWall;
    private GameObject crowdSurfPoint;
    private AudioSource crowdSurfAudio;
    [SerializeField] AudioClip[] crowdSurfClips;
    private bool hasTransformed = false;
    void Start()
    {
        pitWall = transform.Find("Pit Wall").gameObject;
        pitWall.gameObject.SetActive(true);
        crowdSurfPoint = transform.Find("Crowdsurf").gameObject;
        crowdSurfPoint.SetActive(false);
        hasTransformed = false;
        crowdSurfAudio = gameObject.AddComponent<AudioSource>();
        crowdSurfAudio.loop = false;
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
        crowdSurfAudio.PlayOneShot(ChooseSound());
    }

    private AudioClip ChooseSound()
    {
        AudioClip chosenSound;
        int index = Random.Range(0, crowdSurfClips.Length - 1);
        chosenSound = crowdSurfClips[index];
        return chosenSound;
    }
}
