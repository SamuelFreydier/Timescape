using System.Collections;
using UnityEngine;

public class SpectrumFollow : MonoBehaviour //Classe g�rant le "pathfinding" d'un ennemi qui doit suivre le joueur
{
    private Rigidbody2D rb;
    private GameObject target; //Cible des ennemis (ce sera le joueur)

    public float speed; //Vitesse de l'ennemi
    private bool facingRight = true; //O� regarde l'ennemi ?

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player"); //La cible est le joueur
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        //Find direction
        Vector3 dir = (target.transform.position - rb.transform.position).normalized;
        rb.MovePosition(rb.transform.position + dir * speed * Time.fixedDeltaTime);
        if (rb.position.x < target.transform.position.x && !facingRight) //S'il va � droite et regarde � gauche, on le flip
        {
            Flip();
        }
        else if (rb.position.x > target.transform.position.x && facingRight) //S'il va � gauche et regarde � droite, on le flip
        {
            Flip();
        }
    }

    private void MoveEnemy() //Fonction de d�placement de l'ennemi
    {
        Vector2 deltaPos = target.transform.position - transform.position;
        rb.velocity = 1f / Time.fixedDeltaTime * deltaPos * Mathf.Pow(speed, 90f * Time.fixedDeltaTime);
    }

    private void Flip() //Fonction permettant de faire regarder � gauche ou � droite l'ennemi selon son d�placement pour que cela soit coh�rent
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        facingRight = !facingRight;
    }
}