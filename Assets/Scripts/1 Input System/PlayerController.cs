using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{
    // Components
    private static PlayerController controller; 
    public GameObject player;           // player game object
    public Rigidbody2D player_rigid;    // rigidbody2d component
    public Actions player_actions;      // input action 
    
    // Varibles
    public PlayerActionMode action_mode;// current player action mode
    public float player_move;           // player move speed

    // Player Interaction Varible
    public string current_storage;      // the storage which player currently interacting with
    

    void Awake()
    {
        Initial();
        // action register

        // Assign Variable
        player_move = 5f;

    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        SetActionMap(PlayerActionMode.Normal);
    }

    void Update()
    {
        Face();
    }
    
    void FixedUpdate()
    {   
        Movement();
    }

    void OnDisable()
    {
        SetActionMap(PlayerActionMode.End);
    }

    public static PlayerController Controller()
    {
        if(controller == null)
            controller = GameObject.Find("Player").GetComponent<PlayerController>();
        return controller;
    }

    public void Initial()
    {   
        // Assign Component
        player = GameObject.Find("Player");
        if(player == null)
            player = ResourceController.Controller().Load<GameObject>("General/Player");

        ItemController.Controller().AddInvent("Player", 35);

        player_rigid = player.GetComponent<Rigidbody2D>();
        player_actions = new Actions();

        // Action Initial
        player_actions.Player.ChangeQuickSlot.performed += ChangeQuickSlot;
        player_actions.Player.SetQuickSlot.performed += SetQuickSlot;

        player_actions.Building.Interact.performed += Interact;

    }

    /// <summary>
    /// Change Player Action map
    /// </summary>
    /// <param name="action">name of action mode</param>
    public void SetActionMap(PlayerActionMode action)
    {
        switch(action)
        {
            case PlayerActionMode.Normal:
                player_actions.General.Enable();
                player_actions.Player.Enable();
                player_actions.Building.Disable();
                break;
            case PlayerActionMode.Building:
                player_actions.General.Enable();
                player_actions.Player.Disable();
                player_actions.Building.Enable();
                break;
            case PlayerActionMode.End:
                player_actions.General.Disable();
                player_actions.Player.Disable();
                player_actions.Building.Disable();
                break;
            default:
                break;
        }
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
        player_rigid.velocity = player_move * player_actions.General.Movement.ReadValue<Vector2>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void Use(InputAction.CallbackContext context)
    {
        
    }

    /// <summary>
    /// Invoke the building system function when mouse left pressed
    /// </summary>
    private void Interact(InputAction.CallbackContext context)
    {
        
    }

    private void ChangeQuickSlot(InputAction.CallbackContext context)
    {
        if(context.control.ToString() == "Axis:/Mouse/scroll/up")
            GUIController.Controller().GetPanel<GeneralPanel>("GeneralPanel").ShortCutClick(-1, true);
        else
            GUIController.Controller().GetPanel<GeneralPanel>("GeneralPanel").ShortCutClick(1, true);
    }

    private void SetQuickSlot(InputAction.CallbackContext context)
    {
        int index = int.Parse(context.control.ToString().Substring(context.control.ToString().Length-1));
        GUIController.Controller().GetPanel<GeneralPanel>("GeneralPanel").ShortCutClick(index-1, false);
    }


    
}

public enum PlayerActionMode
{
    Normal,
    GUI,
    Building,
    End
}