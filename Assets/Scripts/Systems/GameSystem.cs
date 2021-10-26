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
    [SerializeField] InventoryComponent playerInventory;
    [SerializeField] InventoryComponent warehouseInventory;

    [SerializeField] GameObject pausePanel;
    [SerializeField] List<PlantType> plants;

    public int farmableZoneCount = 1;
    //private List<TileComponent> tiles;
    //public const int MAX_TILES_COUNT = 80;
    //private int nextTileId;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<GameSystem>();
            PlayerInventory = playerInventory;
            WarehouseInventory = warehouseInventory;
            Clock = clock;
            Plants = plants;
            //tiles = new List<TileComponent>();
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

    //public void AddTilesToList(TileComponent tile)
    //{
    //    tiles.Add(tile);
    //}

    //public int GetTilesCount() => tiles.Count;

    //public TileComponent GetTile(int id) => tiles[id];
    //public TileComponent GetNextTile() => tiles[nextTileId++];

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //ResetInitValues();
    }

    //private void ResetInitValues()
    //{
    //    nextTileId = 0;
    //}
}
