using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class loop : MonoBehaviour
{
    public GameObject[] prefabs;
    private Transform objectTransform;
    private float spawnZ = 500.0f;
    private float tileLength = 500.0f;
    private float safeZone = 250.0f;
    private int amnTilesOnScreen = 2;
    private int lastPrefabIndex = 0;

    private List<GameObject> activeTiles;

	
	private void Start ()
    {
        activeTiles = new List<GameObject>();
        objectTransform = GameObject.FindGameObjectWithTag ("Player").transform;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile();
            
        }
    }
	
	
	private void Update ()
    {
	    if (objectTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLength))
        {
            SpawnTile();
            SpawnTile();
            DeleteTile();
            
        }
	}

    private void SpawnTile (int prefabIndex = -1)
    {
        GameObject go;
        go = Instantiate(prefabs[0]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }
    
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);

    } 
}   
