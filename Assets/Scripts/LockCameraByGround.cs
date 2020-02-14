using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class LockCameraByGround : MonoBehaviour
{
    
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _verticalScreenPositionOnGround;
    
    private float _defaultScreenPosition;
    private CinemachineFramingTransposer _vCamTransposer;
    private Transform _player;
    public Transform player
    {
        get { return _player; }
        set { _player = value; }
    }
    private Collider2D _playerCollider;

    private void Awake()
    {
        _vCamTransposer = gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        _defaultScreenPosition = _vCamTransposer.m_ScreenY;
    }

    private void Update()
    {
        if ((_playerCollider != null) && _playerCollider.IsTouchingLayers(_groundLayers))
        {
            ChangeVCamScreenPos(_verticalScreenPositionOnGround);
        }
        else
        {
            ChangeVCamScreenPos(_defaultScreenPosition);
        }
    }

    private void ChangeVCamScreenPos(float screenPos)
    {
        _vCamTransposer.m_ScreenY = Mathf.Lerp(_vCamTransposer.m_ScreenY, screenPos, 0.08f);
    }

    public void GetPlayerCollider()
    {
        _playerCollider = _player.GetComponent<Collider2D>();
    }
}
