using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Canvas _canvasUdied;

    [SerializeField] private GameObject _playerObject;

    public PlayerSpawnedEvent OnPlayerSpawned;

    private void Start()
    {
        if (_playerPrefab != null)
        {
            SpawnPlayer();
        }
        else
        {
            throw new System.NullReferenceException("Player prefab isnt set");
        }
    }

    public void SpawnPlayer()
    {
        if (_playerObject == null)
        {
            _playerObject = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
            OnPlayerSpawned.Invoke(_playerObject.transform);
            _playerObject.GetComponent<PlayerCharacteristics>().OnDeath.AddListener(DespawnPlayer);
            _playerObject.GetComponent<PlayerCharacteristics>().OnDeath.AddListener(() =>
            {
                _canvasUdied.enabled = true;
            });
        }
    }

    public void DespawnPlayer()
    {
        if(_playerObject != null)
        {
            Destroy(_playerObject);
            _playerObject = null;
        }
    }
}

[System.Serializable]
public class PlayerSpawnedEvent: UnityEvent<Transform> { }
