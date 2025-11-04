using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SkillManager.Instance.RequestOpenSkillPanel("Angel's Blessing");
            var status = collision.gameObject.GetComponent<PlayerStatus>();
            status.IncreaseDungeonHp(100);

            Destroy(gameObject);
        }
    }
}
