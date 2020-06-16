using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//In this class, the map has been created.
//You have to edit GetRelativeBlock section to calculate current relative block to cast player rope to hold on
//Update Block Position section to make infinite map.
public class BlockCreator : MonoBehaviour {

    private static BlockCreator instance = null;
    [SerializeField]
    private GameObject[] blockPrefabs;
    [SerializeField]
    private GameObject pointPrefab;
    private GameObject pointObject;
    public int blockCount;

    private List<GameObject> blockPool = new List<GameObject>();
    private float lastHeightUpperBlock = 10;
    

    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    float poolingWaitTime = .5f;
    public static BlockCreator GetSingleton()
    {
        if(instance == null)
        {
            instance = new GameObject("_BlockCreator").AddComponent<BlockCreator>();
        }
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        ActionSystem.OnLevelLoaded += InstantiateBlocks;
        ActionSystem.OnGameStarted += StartPooling;
        ActionSystem.OnGameStarted += StopPooling;
        ActionSystem.OnPointPassed += RePositionPoint;
    }

    private void OnDisable()
    {
        ActionSystem.OnLevelLoaded -= InstantiateBlocks;
        ActionSystem.OnGameStarted -= StartPooling;
        ActionSystem.OnGameStarted -= StopPooling;
        ActionSystem.OnPointPassed -= RePositionPoint;

    }

    public void InstantiateBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            lastHeightUpperBlock = GetNextUpperBlockHeight();
            float randomHeightLowerBlock = GetNextLowerBlockHeight();
            GameObject newUpperBlock = Instantiate(blockPrefabs[i % blockPrefabs.Length], new Vector3(0, lastHeightUpperBlock, i + 1), Quaternion.identity);
            GameObject newLowerBlock = Instantiate(blockPrefabs[i % blockPrefabs.Length], new Vector3(0, randomHeightLowerBlock, i + 1), Quaternion.identity);
            newUpperBlock.layer = 8;
            blockPool.Add(newUpperBlock);
            blockPool.Add(newLowerBlock);
            if (i == 15)
            {
                pointObject = Instantiate(pointPrefab, new Vector3(0, (lastHeightUpperBlock + randomHeightLowerBlock) / 2f, i + 1), Quaternion.Euler(90, 0, 0));
            }
        }
    }
    private float GetNextUpperBlockHeight()
    {
        return UnityEngine.Random.Range(lastHeightUpperBlock - GameManager.Instance.Difficulty, lastHeightUpperBlock + GameManager.Instance.Difficulty);
    }

    private float GetNextLowerBlockHeight()
    {
        return UnityEngine.Random.Range(lastHeightUpperBlock - 20, lastHeightUpperBlock - 20 + GameManager.Instance.Difficulty * 3);
    }




    public Transform GetRelativeBlock(Vector3 playerPos)
    {
        Vector3 dir = (Vector3.forward  + Vector3.up*2).normalized;
        Debug.DrawLine(playerPos, dir*100+ playerPos, Color.red,5);
        if (Physics.Raycast(playerPos, dir, out RaycastHit hit, 100,wallLayer))
        {
            return hit.transform;
        }
        else
            return null;
    }

    void StartPooling()
    {
        StartCoroutine(IEStartPooling());
    }

    void StopPooling()
    {
        StopCoroutine(IEStartPooling());
    }

    void RePositionPoint()
    {
        var count = blockPool.Count;
        pointObject.transform.position = (blockPool[count - 1].transform.position + blockPool[count - 2].transform.position)*.5f;
    }

    IEnumerator IEStartPooling()
    {
        while(true)
        {
            yield return new WaitForSeconds(poolingWaitTime);
            UpdateBlockPosition(GetCurrentBlockIndex());
        }
    }

    int GetCurrentBlockIndex()
    {
        if (Physics.Raycast(GameManager.Instance.Player.position, Vector3.up, out RaycastHit hit, 100, wallLayer))
        {
            return blockPool.IndexOf(hit.transform.gameObject);
        }
        else
            return -1;
    }

    public void UpdateBlockPosition(int blockIndex)
    {
        var tempPool = new List<GameObject>();
        var lastBlockZ = blockPool[blockPool.Count-1].transform.position.z;

        for (int i = 0; i < blockIndex-10 ; i++)
        {
            tempPool.Add(blockPool[i]);
        }
        for (int i = 0; i < blockIndex-10; i++)
        {
            blockPool.RemoveAt(0);
        }
        var count = tempPool.Count*.5f;
        for (int i = 0; i < count; i++)
        {
            lastHeightUpperBlock = GetNextUpperBlockHeight();
            float randomHeightLowerBlock = GetNextLowerBlockHeight();
            tempPool[0].transform.position = new Vector3(0, lastHeightUpperBlock, i + lastBlockZ);
            blockPool.Add(tempPool[0]);
            tempPool.RemoveAt(0);
            tempPool[0].transform.position = new Vector3(0, randomHeightLowerBlock, i  + lastBlockZ);
            blockPool.Add(tempPool[0]);
            tempPool.RemoveAt(0);
        }
    }
}
