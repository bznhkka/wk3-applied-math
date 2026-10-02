using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static private bool youWon = false;
    [SerializeField] public GameObject player;

    static public void setGameWin()
    {
        //youWon = true;
    }

    static public void restartGame()
    {
        SceneManager.LoadScene(0);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
