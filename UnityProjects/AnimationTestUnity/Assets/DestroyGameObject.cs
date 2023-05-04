using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;

    void Update() => Destroy(gameObject, lifeTime);
}

