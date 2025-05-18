using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonCollider : MonoBehaviour
{
    Player player;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            player.isJumping = false;      //��������ذ����Ծ�������ָ�������Ծ��״̬
            player.m_Animator.SetBool("Jump", false);      //��������ذ����Ծ��������
        }
        if(collision.tag == "MoveGround")
        {
            player.isJumping = false;      //��������ذ����Ծ�������ָ�������Ծ��״̬
            player.m_Animator.SetBool("Jump", false);      //��������ذ����Ծ��������
            player.transform.parent = collision.transform;  //����ҳ�Ϊ�ذ�������
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "MoveGround")
        {
            player.transform.parent = null;
        }
    }
}
