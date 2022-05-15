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

        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING) //S'il peut se d�placer
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;  //Il se d�place en fonction de l'input horizontal et de sa vitesse
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); //Mise � jour de la valeur speed pour l'animation de Run

            animator.SetFloat("VerticalVelocity", controller.m_Rigidbody2D.velocity.y); //Mise � jour de la verticalvelocity pour animer le Jump Blend Tree

            if (Input.GetButtonDown("Jump")) //Si on appuie sur le bouton pour sauter, le bool�en passe � vrai
            {
                jump = true;
            }

            animator.SetBool("isGrounded", controller.m_Grounded); //Le bool�en de l'animator = le bool�en du controller qui permet de savoir si le joueur est au sol
        }
        else
        {
            controller.m_Rigidbody2D.velocity = new Vector2(0, controller.m_Rigidbody2D.velocity.y); //S'il ne peut pas bouger, on met sa v�locit� en x � 0
            animator.SetFloat("Speed", 0); //On met � jour sur l'animator pour passer en Idle
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
