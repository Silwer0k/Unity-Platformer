using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundFollowing : MonoBehaviour
{
    [SerializeField] private Vector2 _smoothFollowing;

    private Transform _camTransform;
    private Vector3 _lastCamPosition;
    private float _textureUnitSizeX;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _camTransform = Camera.main.transform;
        _lastCamPosition = _camTransform.position;
        Sprite _sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D _texture = _sprite.texture;
        _textureUnitSizeX = _texture.width / _sprite.pixelsPerUnit; 
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 deltaMovement = _camTransform.position - _lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * _smoothFollowing.x, deltaMovement.y * _smoothFollowing.y);
        _lastCamPosition = _camTransform.position;

        if (Mathf.Abs(_camTransform.position.x - transform.position.x) >= _textureUnitSizeX) 
        {
            float _offsetPositionX = (_camTransform.position.x - transform.position.x) % _textureUnitSizeX;
            transform.position = new Vector3 (_camTransform.position.x + _offsetPositionX, transform.position.y);
        }
    }
}
