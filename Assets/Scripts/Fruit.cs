using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.sharedInstance.MakeInvincible(1.0f);
            UIManager.sharedInstance.IncreaseScore(200);
            Destroy(this.gameObject);
        }
    }

}
