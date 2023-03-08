using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    [SerializeField] List<GameObject> pitEnemies;

    private void Start()
    {
        if (pitEnemies.Count != 0)
        {
            foreach (GameObject pitEnemy in pitEnemies)
            {
                pitEnemy.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateAllEnemies()
    {
        if (pitEnemies.Count != 0)
        {
            foreach (GameObject pitEnemy in pitEnemies)
            {
                pitEnemy.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivateAllEnemies();
        }
    }
}
