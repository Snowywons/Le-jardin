using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystemComponent : MonoBehaviour
{
    public Dictionary<string, TileInfo> tiles = new Dictionary<string, TileInfo>();
    public Dictionary<int, SavedItem> playerInventory = new Dictionary<int, SavedItem>();
    public Dictionary<int, SavedItem> warehouseInventory = new Dictionary<int, SavedItem>();
    public List<PlantType> plants;
    public Dictionary<string, InventoryItem> itemDB;
    public int randomSeed;

    public int playerInventoryLevel;
    public int playerInventoryCapacity => playerInventoryLevel + 4;
    public int warehouseInventoryCapacity => 20;
    public int wateringCanLevel;
    public int farmingZoneLevel;
    public int farmingZonesUnlocked => farmingZoneLevel + 3;
    public int scorePoints;
    public int money;
    public float currentDayTime;
    public int currentDay;
    string path;

    //Valeurs initiales
    public void Awake()
    {
        itemDB = new Dictionary<string, InventoryItem>();
        foreach (var plant in plants)
        {
            itemDB.Add(plant.ID, plant);
            var seed = new Seed(plant);
            itemDB.Add(seed.ID, seed);
        }
        var wateringCan = FindObjectOfType<WateringComponent>();
        itemDB.Add(wateringCan.ID, wateringCan);
        var shovel = FindObjectOfType<HarvestingComponent>();
        itemDB.Add(shovel.ID, shovel); 
        
    }
    public string GetPath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"savefile{slot}.txt");
    }

    public void StartGame(int slot)
    {
        path = GetPath(slot);
        try
        {
            Load();
        }
        catch (IOException)
        {
            NewGame();
            Save();
        }
        FindObjectOfType<SceneNavigatorComponent>().Load(SceneNavigatorComponent.WORLD);
    }

    public bool GameExists(int slot)
    {
        return File.Exists(GetPath(slot));
    }

    public void DeleteGame(int slot)
    {
        File.Delete(GetPath(slot));
    }

    private void NewGame()
    {
        var rando = new System.Random();
        playerInventory.Clear();
        warehouseInventory.Clear();
        playerInventory.Add(playerInventory.Count, new SavedItem { item = FindObjectOfType<WateringComponent>(), quantity = -1 });
        playerInventory.Add(playerInventory.Count, new SavedItem { item = FindObjectOfType<HarvestingComponent>(), quantity = -1 });
        playerInventoryLevel = 0;
        farmingZoneLevel = 0;
        wateringCanLevel = 0;
        scorePoints = 0;
        money = 45000;
        currentDay = 1;
        currentDayTime = 0;
        randomSeed = rando.Next();

        foreach (PlantType plante in plants)
        {
            if (playerInventory.Count < playerInventoryCapacity)
                playerInventory.Add(playerInventory.Count, new SavedItem { item = new Seed(plante), quantity = 2 });
            if (warehouseInventory.Count < warehouseInventoryCapacity)
                warehouseInventory.Add(warehouseInventory.Count, new SavedItem { item = new Seed(plante), quantity = 2 });
        }
    }

    public void Save()
    {        
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream);

        JObject jTiles = new JObject();
        foreach(var tile in tiles)
        {
            JObject jTileInfo = new JObject(
                                    new JProperty("isWet", tile.Value.isWet),
                                    new JProperty("age", tile.Value.age),
                                    new JProperty("plant", tile.Value.plante?.ID));
            jTiles.Add(tile.Key, jTileInfo);
        }
        JObject jPlayer = SerializeInventory(playerInventory);
        JObject jWarehouse = SerializeInventory(warehouseInventory);
        JObject saveFile = new JObject(
                                    new JProperty("tiles", jTiles),
                                    new JProperty("player", jPlayer),
                                    new JProperty("warehouse", jWarehouse),
                                    new JProperty("playerInventoryLevel", playerInventoryLevel),
                                    new JProperty("wateringCanLevel", wateringCanLevel),
                                    new JProperty("farmingZoneLevel", farmingZoneLevel),
                                    new JProperty("scorePoints", scorePoints),
                                    new JProperty("money", money),
                                    new JProperty("currentDay", currentDay),
                                    new JProperty("currentDayTime", currentDayTime),
                                    new JProperty("randomSeed", randomSeed));

        
        using var jsonWriter = new JsonTextWriter(writer);
        jsonWriter.Formatting = Formatting.Indented;
        saveFile.WriteTo(jsonWriter);
    }

    public void Load()
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(reader);

        JObject savefile = JObject.Load(jsonReader);
        playerInventoryLevel = savefile.Value<int>("playerInventoryLevel");
        wateringCanLevel = savefile.Value<int>("wateringCanLevel");
        farmingZoneLevel = savefile.Value<int>("farmingZoneLevel");
        scorePoints = savefile.Value<int>("scorePoints");
        money = savefile.Value<int>("money");
        currentDay = savefile.Value<int>("currentDay");
        currentDayTime = savefile.Value<float>("currentDayTime");
        randomSeed = savefile.Value<int>("randomSeed");
        var jTiles = (JObject)savefile["tiles"];
        tiles.Clear();
        foreach(var tile in jTiles.Properties())
        {
            var tileValue = tile.Value;
            var plantID = tileValue.Value<string>("plant");
            var tileInfo = new TileInfo(tileValue.Value<bool>("isWet"),
                                        plantID != null?(PlantType)itemDB[plantID]: null,
                                        tileValue.Value<int>("age"));
            tiles[tile.Name] = tileInfo;
        }
        DeserializeInventory((JObject)savefile["player"], playerInventory);
        DeserializeInventory((JObject)savefile["warehouse"], warehouseInventory);
    }

    private JObject SerializeInventory(Dictionary<int, SavedItem> inventory)
    {
        JObject jItems = new JObject();
        foreach (var item in inventory)
        {
            JObject jItem = new JObject(
                                    new JProperty("item", item.Value.item.ID),
                                    new JProperty("quantity", item.Value.quantity));
            jItems.Add(item.Key.ToString(), jItem);
        }
        return jItems;
    }
    private void DeserializeInventory(JObject toDeserialize, Dictionary<int, SavedItem> dictionary)
    {
        dictionary.Clear();
        foreach(var objet in toDeserialize.Properties())
        {
            var itemID = objet.Value.Value<string>("item");
            var item = itemDB[itemID];
            var quantity = objet.Value.Value<int>("quantity");
            dictionary[int.Parse(objet.Name)] = new SavedItem(item, quantity);
        }
    }
}

public struct SavedItem
{
    public InventoryItem item;
    public int quantity;

    public SavedItem(InventoryItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
