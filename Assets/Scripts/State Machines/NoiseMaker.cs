using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    [SerializeField] private Movement movementData;

    // Start is called before the first frame update
    void Start()
    {
        movementData = this.gameObject.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
