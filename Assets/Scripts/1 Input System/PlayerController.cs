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

    public static PlayerController GetController()
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
            player = ResourceController.GetController().Load<GameObject>("General/Player");

        ItemController.GetController().AddInvent("Player", 20);

        player_rigid = player.GetComponent<Rigidbody2D>();
        player_actions = new Actions();

        // Action Initial
        player_actions.Building.Act.performed += Act;
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
                player_actions.Player.Enable();
                player_actions.Building.Disable();
                break;
            case PlayerActionMode.Building:
                player_actions.Player.Disable();
                player_actions.Building.Enable();
                break;
            case PlayerActionMode.End:
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
        player_rigid.velocity = player_move * player_actions.Player.Movement.ReadValue<Vector2>();
    }

    /// <summary>
    /// Invoke the building system function when mouse left pressed
    /// </summary>
    private void Act(InputAction.CallbackContext context)
    {
        if(BuildController.GetController().build_mode != "")
            EventController.GetController().EventTrigger<Vector2>(BuildController.GetController().build_mode, Mouse.current.position.ReadValue());
    }


    
}

public enum PlayerActionMode
{
    Normal,
    GUI,
    Building,
    End
}