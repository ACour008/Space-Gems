using System;
using System.Collections.Generic;
using UnityEngine;
using MiskoWiiyaas.Cells;
using MiskoWiiyaas.Enums;

namespace MiskoWiiyaas.Tiles
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject spawnerPrefab;
        [SerializeField] private Vector3 startLocation;
        private SpawnerCell[] spawnPoints;
        private GameObject prefab;


        public void Init(int numSpawnPoints, GameObject tilePrefab)
        {
            spawnPoints = new SpawnerCell[numSpawnPoints];
            prefab = tilePrefab;

            Vector3 spawnLocation = startLocation;
            for (int i =0; i < spawnPoints.Length; i++)
            {
                GameObject spawner = GameObject.Instantiate<GameObject>(spawnerPrefab, transform);
                spawner.transform.localPosition = spawnLocation;
                spawnLocation.x += 1f;

                SpawnerCell cell = spawner.GetComponent<SpawnerCell>();
                spawnPoints[i] = cell;
            }
        }

        public Tile SpawnTileAtCol(int x, List<TileType> possibleTypes)
        {
            SpawnerCell spawner = spawnPoints[x];
            // List<TileType> possibleTypes = new List<TileType>((TileType[])Enum.GetValues(typeof(TileType)));
            return spawner.CreateTile(prefab, possibleTypes, spawner.transform);
        }
    }

}