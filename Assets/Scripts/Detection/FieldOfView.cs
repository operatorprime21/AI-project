using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Field of View variables")]
    [Range(0, 360)]
    [Tooltip("Angle in which the AI can detect the player")] public float angle;
    [Tooltip("Range in which the AI can detect the player")] public float range;
    [Tooltip("Detects wall layer masks to avoid seeing AI through walls")] public LayerMask wallsMask;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
