using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float moveSpeed = 0.2f;
    //public GameObject player;

    private InputAction moveAction;


    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 moveVec = moveAction.ReadValue<Vector2>();
        Debug.Log($"moveVec = {moveVec}");

        transform.Translate(new Vector3(moveVec.x * moveSpeed, 0.0f, moveVec.y * moveSpeed));
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        enabled = false;
        GameManager.restartGame();
        //Destroy(player);
    }

    public void youWin()
    {
        Debug.Log("player has completed the trials");

    }
}
