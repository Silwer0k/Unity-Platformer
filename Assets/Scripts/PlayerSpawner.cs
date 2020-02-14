using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;

    public PlayerSpawnedEvent OnPlayerSpawned;

    private void Start()
    {
        if(_playerPrefab != null)
        {
            var player = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
            OnPlayerSpawned.Invoke(player.transform);
        }
        else
        {
            throw new NullReferenceException("Player prefab isnt set");
        }
    }
}

[Serializable]
public class PlayerSpawnedEvent: UnityEvent<Transform> { }
