using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwaySpeed : MonoBehaviour
{
    [SerializeField] private PlayerController PlayerController;

    void OnTriggerEnter(Collider other)
    {
        PlayerController.speed = 30.0f;
    }

   void OnTriggerExit(Collider other)
    {
        PlayerController.speed = 10.0f; 
    }
}
