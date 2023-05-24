using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth PlayerHealth;

    public TextMeshProUGUI currentHealth;
    public TextMeshProUGUI currentMaxHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentHealth.text = PlayerHealth._currentHealth.ToString() + " /";
        currentMaxHealth.text = PlayerHealth._currentMaxHealth.ToString();
    }
}
