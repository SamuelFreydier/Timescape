using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 25f;
    private Rigidbody2D rb;

    public PlayerController controller;
    public Animator animator;

    private float horizontalMove;
    private bool jump = false; //Est-ce que le joueur saute ?

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING) //S'il peut se déplacer
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;  //Il se déplace en fonction de l'input horizontal et de sa vitesse
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); //Mise à jour de la valeur speed pour l'animation de Run

            animator.SetFloat("VerticalVelocity", controller.m_Rigidbody2D.velocity.y); //Mise à jour de la verticalvelocity pour animer le Jump Blend Tree

            if (Input.GetButtonDown("Jump")) //Si on appuie sur le bouton pour sauter, le booléen passe à vrai
            {
                jump = true;
            }

            animator.SetBool("isGrounded", controller.m_Grounded); //Le booléen de l'animator = le booléen du controller qui permet de savoir si le joueur est au sol
        }
        else
        {
            controller.m_Rigidbody2D.velocity = new Vector2(0, controller.m_Rigidbody2D.velocity.y); //S'il ne peut pas bouger, on met sa vélocité en x à 0
            animator.SetFloat("Speed", 0); //On met à jour sur l'animator pour passer en Idle
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isGrounded", false);
    }
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump); //On appelle la fonction Move du controller avec sa vitesse horizontale et s'il saute ou non
        jump = false;
    }
}
