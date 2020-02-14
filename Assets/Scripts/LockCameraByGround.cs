using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class LockCameraByGround : MonoBehaviour
{
    [SerializeField] private Collider2D _player;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _verticalScreenPositionOnGround;
    [SerializeField] private float _defaultScreenPosition;

    private CinemachineFramingTransposer _vCamTransposer;

    private void Awake()
    {
        _vCamTransposer = gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        if (_player.IsTouchingLayers(_groundLayers))
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
}
