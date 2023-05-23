using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgDealer : MonoBehaviour
{
    public float dmgImpactAmount = 10.0f;
    public float dmgStayAmount = 10.0f;
    public int dmgStayAmountWait = 2;

    //checks if player is colliding and callin DmgUnit function
    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Player"){
            PlayerHealth health = trigger.gameObject.GetComponent<PlayerHealth>();
            health.DmgUnit(dmgImpactAmount);
        }
        /*if(trigger.gameObject.TryGetComponent(out PlayerHealth _currentHealth)){
            _currentHealth.DmgUnit(dmgAmount); 
            Debug.Log("asdkjgh");
        }*/
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
            health.DmgUnitOverTime(dmgStayAmount, dmgStayAmountWait); 
        }
    }
}