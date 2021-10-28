using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public int zoneId;

    public bool isFarmable;
    public bool isWet;
    public PlantType plante;
    public int age;
    public Material wetMaterial;
    private Material dryMaterial;
    private SaveSystemComponent savesystem;
    [SerializeField] GameObject outline;

    [HideInInspector] public Transform modele;

    public bool instantGrow; // For Debug Only

    public static HashSet<TileComponent> tiles = new HashSet<TileComponent>();

    private void Start()
    {
        isFarmable = zoneId <= GameSystem.Instance.farmableZoneCount;

        savesystem = FindObjectOfType<SaveSystemComponent>();

        outline.gameObject.SetActive(false);

        dryMaterial = GetComponent<MeshRenderer>().material;

        if (savesystem.tiles.TryGetValue(gameObject.name, out var tileInfo))
        {
            if (tileInfo.isWet)
                SetWet();

            if (tileInfo.plante)
            {
                Plant(tileInfo.plante);
            }

            age = tileInfo.age;
        }

        if (instantGrow)
        {
            for (int i = 0; i < plante.maturingTime; i++, isWet = true)
                OnDayAdvance();
        }
    }

    public bool Plant(PlantType type)
    {
        if (plante == null)
        {
            Debug.Log("Planter");
            SetModel(type.seed);
            plante = type;
            age = 0;
            return true;
        }
        return false;
    }

    public bool Harvest()
    {
        // Security check
        if (plante == null)
        {
            return false;
        }

        InventoryItem item = null;
        if (age == plante.maturingTime)
        {
            item = plante;
        }
        else if(age > plante.maturingTime)
        {
            item = new Seed(plante);
        }

        if (GameSystem.Instance.PlayerInventory.Update(item, 1))
        {
            TileReset();
            return true;
        }

        else
        {
            // TO DO : Informer le joueur que son inventaire est plein.
            Debug.Log("Inventaire plein.");
            return false;
        }
    }

    public void SetWet()
    {
        isWet = true;
        GetComponent<MeshRenderer>().material = wetMaterial;
    }

    public void OnInteract()
    {
        if (!isFarmable) return;

        var selection = GameSystem.Instance.PlayerInventory.GetSelected();
        if (selection is IUsable usable)
        {
            if (usable.Use(this)) 
            {
                savesystem.tiles[gameObject.name] = new TileInfo(isWet, plante, age);
                if (usable.Consumable)
                    GameSystem.Instance.PlayerInventory.Remove(selection);
            }
        }
    }

    private void TileReset()
    {
        age = 0;
        plante = null;
        Destroy(modele.gameObject);
    }

    private void SetModel(Transform nouveau)
    {
        if (modele != null)
        {
            Destroy(modele.gameObject);
        }

        modele = Instantiate(nouveau);
        modele.position = new Vector3(0, 0.5f, 0);
        modele.SetParent(transform, false);
    }

    public void OnDayAdvance()
    {
        Debug.Log("advance");
        
        if (isWet)
        {
            GetComponent<MeshRenderer>().material = dryMaterial;
            isWet = false;
            if (plante != null)
            {
                age++;
        
                if (age == 1)
                {
                    SetModel(plante.young);
                }
                else if (age == plante.maturingTime)
                {
                    SetModel(plante.mature);
                }
            }
        }

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnDayAdvance();
        }
    }

    void OnMouseEnter()
    {
        if (!isFarmable) return;

        ShowTilesOutline(true);
    }

    void OnMouseExit()
    {
        if (!isFarmable) return;

        ShowTilesOutline(false);
    }

    private void CopyFromTile(TileComponent tile)
    {
        zoneId = tile.zoneId;
        isFarmable = tile.isFarmable;
        isWet = tile.isWet;
        plante = tile.plante;
        age = tile.age;
        modele = tile.modele;
        transform.position = tile.transform.position;
    }

    private HashSet<TileComponent> FindAllAdjacents(bool inRowOnly = false)
    {
        string[] name = gameObject.name.Split(' ');
        int id = int.Parse(name[1]);

        int zoneId = (id / 10) * 10; // 0, 10, 20, 30...


        tiles.Clear();

        // Détermine les limites de la recherche de tiles
        int fromId = inRowOnly && id - zoneId >= 5 ? zoneId += 5 : zoneId;
        int toId = inRowOnly ? zoneId + 5 : zoneId + 10;

        for (int i = fromId, j = 0; i < toId; i++, j++)
        {
            GameObject tile = GameObject.Find($"Tile {i}");
            if (tile)
                tiles.Add(tile.GetComponent<TileComponent>());
        }

        return tiles;
    }

    private void ShowTilesOutline(bool shown)
    {
        // Si l'objet en main est une watering can
        if (GameSystem.Instance.PlayerInventory.GetSelected().Name == "Watering Can")
        {
            int level = GameSystem.Instance.wateringCanLevel;
            
            // Niveau 0
            if (level == 0)
            {
                // On vide le hashset de tiles par défaut
                tiles.Clear();
                outline.gameObject.SetActive(shown);
            }
            else

            // Niveau 1
            if (level == 1)
            {
                FindAllAdjacents(true);
                foreach (var t in tiles)
                    t.outline.gameObject.SetActive(shown);
            }
            else

            // Niveau 2
            if (level == 2)
            {
                FindAllAdjacents();
                foreach (var t in tiles)
                    t.outline.gameObject.SetActive(shown);
            }
        }
        else
        {
            // L'objet en main n'est pas la watering can..
            // Tous les outlines des tiles contenues dans le hashset doivent être désactivés
            foreach (var t in tiles)
                t.outline.gameObject.SetActive(false);

            tiles.Clear();
            outline.gameObject.SetActive(shown);
        }
    }
}
