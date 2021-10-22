using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Text index;
    public Text quantity;
    public Image icon;
    public Outline outline;
    public Image border;
    
    // Référence sur le slot qui le contient
    public Slot slot;

    // Référence sur son ancien canvasContent
    [HideInInspector]
    public Transform oldCanvasContent;
}
