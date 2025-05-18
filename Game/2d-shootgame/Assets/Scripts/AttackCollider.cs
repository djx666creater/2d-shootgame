using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackCollider : MonoBehaviour
{
    public GeneralEnemy generalEnemy;
    public BossEnemy bossEnemy;

    private void Awake()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)     //Íæ¼ÒµÄAttackCollider
    {
        if(collision.tag == "GeneralEnemy")
        {
            
        }
        if(collision.tag == "BossEnemy")
        {

        }
    }
}
