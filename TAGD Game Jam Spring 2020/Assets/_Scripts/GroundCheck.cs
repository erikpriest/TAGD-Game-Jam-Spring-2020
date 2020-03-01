using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_collisionLayer;
    /*
    private bool m_touchingGround;
    public bool TouchingGround
    {
        get
        {
            return m_touchingGround;
        }
        protected set
        {
            m_touchingGround = value;
        }
    }
    */
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision != null && (((1 << collision.gameObject.layer) & m_collisionLayer) != 0)))
        {
            //TouchingGround = true;
            gameObject.SendMessageUpwards("Grounded");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //TochingGround = (collision != null && (((1 << collision.gameObject.layer) & m_collisionLayer) != 0)) ? true : false;
        
        if((collision != null && (((1 << collision.gameObject.layer) & m_collisionLayer) != 0)))
        {
            gameObject.SendMessageUpwards("Grounded");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision != null && (((1 << collision.gameObject.layer) & m_collisionLayer) != 0)))
        {
            //TouchingGround = false;
            gameObject.SendMessageUpwards("NotGrounded");
        }
    }

}
