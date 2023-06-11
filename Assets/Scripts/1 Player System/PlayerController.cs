using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    public Rigidbody2D player_rigid;    // rigidbody2d component
    public Actions player_actions;      // input action 
    
    // Varibles
    public float player_move;           // player move speed

    void Awake()
    {
        // Assign Component
        player_rigid = GetComponent<Rigidbody2D>();
        player_actions = new Actions();

        // action register


        // Assign Variable
        player_move = 5f;

    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        player_actions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void OnDisable()
    {
        player_actions.Player.Disable();
    }

    public void Initial()
    {   
        // Assign Component
        player = GameObject.Find("Player");
        if(player == null)
            player = ResourceController.GetController().Load<GameObject>("General/Player");  


        player_invent = player.GetComponent<Inventory>();
        if(player_invent)
            player_invent = player.AddComponent<Inventory>();
        player_invent.Initial(20);

        player_rigid = player.GetComponent<Rigidbody2D>();
        player_actions = new Actions();
    }

    /// <summary>
    /// Player facing direction
    /// </summary>
    private void Face()
    {
        player.GetComponent<SpriteRenderer>().flipX = Mouse.current.position.ReadValue().x > Screen.width/2;
    }

    /// <summary>
    /// Player Movement
    /// </summary>
    private void Movement()
    {
        player_rigid.velocity = player_move * player_actions.Player.Movement.ReadValue<Vector2>();
    }
}
