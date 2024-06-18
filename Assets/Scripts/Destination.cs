using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : ProjectBehaviourScript
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCtrl player = collision.gameObject.GetComponent<PlayerCtrl>();
        if(player != null)
        {
            GameManager.Instance.WinGame();
            LevelSystemManager.Instance.LevelComplete(3);

        }
    }
}
