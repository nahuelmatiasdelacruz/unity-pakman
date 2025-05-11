using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        UIManager.sharedInstance.IncreaseScore(100);
        Destroy(this.gameObject);
    }
}
