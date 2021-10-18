using UnityEngine;

public enum GameState
{
    Resume,
    Pause
}

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }
    public InventoryComponent Inventory { get; private set; }
    public ClockComponent Clock { get; private set; }
    public GameState State { get; private set; }

    [SerializeField] InventoryComponent inventory;
    [SerializeField] ClockComponent clock;

    [SerializeField] GameObject pausePanel;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<GameSystem>();
            Inventory = inventory;
            Clock = clock;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (State.Equals(GameState.Resume))
        {
            Clock.TogglePause();
            pausePanel.SetActive(true);
            State = GameState.Pause;
        }
        else
        {
            Clock.TogglePause();
            pausePanel.SetActive(false);
            State = GameState.Resume;
        }
    }
}
