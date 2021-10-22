using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public enum GameState
{
    Resume,
    Pause
}

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }
    public InventoryComponent PlayerInventory { get; private set; }
    public InventoryComponent WarehouseInventory { get; private set; }
    public ClockComponent Clock { get; private set; }
    public List<PlantType> Plants { get; private set; }
    public GameState State { get; private set; }

    [SerializeField] ClockComponent clock;

    [SerializeField] GameObject pausePanel;
    [SerializeField] List<PlantType> plants;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<GameSystem>();
            PlayerInventory = FindObjectsOfType<InventoryComponent>().Where(i => i.mode.Equals(InventoryMode.Player)).FirstOrDefault();
            WarehouseInventory = FindObjectsOfType<InventoryComponent>().Where(i => i.mode.Equals(InventoryMode.Warehouse)).FirstOrDefault();
            Clock = clock;
            Plants = plants;
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
            Clock.SetPause(true);
            pausePanel.SetActive(true);
            State = GameState.Pause;
        }
        else
        {
            Clock.SetPause(false);
            pausePanel.SetActive(false);
            State = GameState.Resume;
        }
    }
}
