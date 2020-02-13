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

    private CinemachineFramingTransposer GetVirtualCameraTransposer()
    {
        if(gameObject.GetComponent<CinemachineVirtualCamera>() != null)
        {
            return gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else
        {
            throw new NullReferenceException("Cant find VCam");
        }
    }

    private void ChangeVCamScreenPos(float screenPos)
    {
        var virtualCameraTransposer = GetVirtualCameraTransposer();
        virtualCameraTransposer.m_ScreenY = Mathf.Lerp(virtualCameraTransposer.m_ScreenY, screenPos, 0.08f);
    }
}
