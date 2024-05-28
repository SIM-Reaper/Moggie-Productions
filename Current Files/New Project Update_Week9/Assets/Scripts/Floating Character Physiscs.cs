using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCharacterPhysiscs : MonoBehaviour
{
    public float dampFactor = 1;
    public float dampFrequency = 15;
    public float hoverHeight = 1.5f;
    public float maxDistance = 2;
    public float castRadius = .5f;      
    public Rigidbody rb;
    RaycastHit[] hits = new RaycastHit[10];

    private void Awake()
    {        
        if (!rb)
        {
            Debug.LogError($"[{nameof(Hover)}] missing field RigidBody.");
            enabled = false;
        }
    }

    private object Hover()
    {
        throw new NotImplementedException();
    }

    private void FixedUpdate()
    {
        ApplyHoverForce();
    }

    void ApplyHoverForce()
    {
        if (GroundCast(out RaycastHit hit))
        {
            Vector3 rayDirection = Vector3.down;
            float springDelta = GetSpringDelta(hit);
            float springStrength = SpringStrength(rb.mass, dampFrequency);
            float dampStrength = DampStrength(dampFactor, rb.mass, dampFrequency);
            float springSpeed = GetRelativeSpeedAlongDirection(rb, hit.rigidbody, rayDirection);
            Vector3 springForce = GetSpringForce(
                springDelta, 
                springSpeed, 
                springStrength, 
                dampStrength, 
                rayDirection);
            springForce -= Physics.gravity;
            rb.AddForce(springForce);
            if (hit.rigidbody) hit.rigidbody.AddForceAtPosition(-springForce, hit.point);
        }
    }

    bool GroundCast(out RaycastHit hit)
    {
        int hitCount = Physics.SphereCastNonAlloc(
            transform.position,
            castRadius,
            -transform.up,
            hits,
            maxDistance);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit current = hits[i];
                if (current.rigidbody == rb) continue;
                hit = current;
                return true;
            }
        }
        hit = default;
        return false;
    }

    float GetSpringDelta(RaycastHit hit)
    {
        return hit.distance - (hoverHeight - castRadius);
    }

    static float GetRelativeSpeedAlongDirection(
        Rigidbody targetBody, 
        Rigidbody frameBody, 
        Vector3 direction)
    {
        Vector3 velocity = targetBody.velocity;
        Vector3 hitBodyVelocity = frameBody ? frameBody.velocity : default;
        float rayDirectionSpeed = Vector3.Dot(direction, velocity);
        float hitBodyRayDirectionSpeed = Vector3.Dot(direction, hitBodyVelocity);
        return rayDirectionSpeed - hitBodyRayDirectionSpeed;
    }

    static float SpringStrength(float mass, float frequency)
    {
        return frequency * frequency * mass;
    }

    static float DampStrength(float dampFactor, float mass, float frequency)
    {
        float criticalDampStrength = 2 * mass * frequency;
        return dampFactor * criticalDampStrength;
    }

    static Vector3 GetSpringForce(
        float springDelta,
        float springSpeed,
        float springStrength,
        float dampStrength,
        Vector3 direction)
    {
        float tension = springDelta * springStrength;
        float damp = springSpeed * dampStrength;
        float forceMagnitude = tension - damp;
        Vector3 force = direction * forceMagnitude;
        return force;
    }
}
