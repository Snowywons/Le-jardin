using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class SaveSystemComponent : MonoBehaviour
{
    public Dictionary<string, TileInfo> tiles = new Dictionary<string, TileInfo>();
    public Dictionary<int, SavedItem> playerInventory = new Dictionary<int, SavedItem>();
    public Dictionary<int, SavedItem> warehouseInventory = new Dictionary<int, SavedItem>();
    public int playerInventoryCapacity;
    public int warehouseInventoryCapacity;
    string path;

    //Valeurs initiales
    public void Initialize()
    {
        path = Path.Combine(Application.persistentDataPath, "savefile.txt");
        try
        {
            Load();
        }
        catch (IOException)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        playerInventory.Clear();
        warehouseInventory.Clear();
        playerInventory.Add(playerInventory.Count, new SavedItem { item = FindObjectOfType<WateringComponent>(), quantity = -1 });
        playerInventory.Add(playerInventory.Count, new SavedItem { item = FindObjectOfType<HarvestingComponent>(), quantity = -1 });
        playerInventoryCapacity = 4;
        warehouseInventoryCapacity = 20;

        foreach (PlantType plante in GameSystem.Instance.Plants)
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
                                    new JProperty("playerCapacity", playerInventoryCapacity),
                                    new JProperty("warehouseCapacity", warehouseInventoryCapacity));
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
        playerInventoryCapacity = savefile.Value<int>("playerCapacity");
        warehouseInventoryCapacity = savefile.Value<int>("warehouseCapacity");
        var jTiles = (JObject)savefile["tiles"];
        tiles.Clear();
        foreach(var tile in jTiles.Properties())
        {
            var tileValue = tile.Value;
            var plantID = tileValue.Value<string>("plant");
            var tileInfo = new TileInfo(tileValue.Value<bool>("isWet"),
                                        (PlantType)GameSystem.Instance.itemDB[plantID],
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
            var item = GameSystem.Instance.itemDB[itemID];
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
