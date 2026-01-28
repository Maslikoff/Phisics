using UnityEngine;

public interface IMovement
{
    float CurrentSpeed { get; }
    bool IsMoving { get; }
    Vector3 Direction { get; }

    void Move(Vector3 direction);
    void Stop();
    void RotateTowards(Vector3 direction);
}