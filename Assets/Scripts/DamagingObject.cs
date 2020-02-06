using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    [SerializeField] private float _damageValue = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerCharacteristics>() != null)
        {
            collision.gameObject.GetComponent<PlayerCharacteristics>().TakeDamage(_damageValue);
        }
    }
}
