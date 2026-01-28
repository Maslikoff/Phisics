using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float BaseSpeed = 5f;

    protected virtual void Update(){}
}