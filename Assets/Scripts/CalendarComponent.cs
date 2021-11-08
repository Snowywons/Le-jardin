using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class CalendarComponent : MonoBehaviour
{
    [SerializeField] Transform calendarContent;
    [SerializeField] Transform calendarDaysPanel;
    [SerializeField] DaySlot daySlotPrefab;

    public void UpdateCalendar()
    {
        calendarContent.transform.Clear();

        int currentDay = GameSystem.Instance.Clock.GetDay();
        var discountsList = FindObjectOfType<DiscountComponent>().discountsList;

        // Initialiser les 31 jours
        for (int i = 0; i < 31; i++)
        {
            // Ajouter une case (jour) dans le calendrier
            DaySlot daySlot = Instantiate(daySlotPrefab.gameObject, calendarContent.transform).GetComponent<DaySlot>();
            daySlot.index.text = $"{i + 1}";
            daySlot.border.gameObject.SetActive(false);

            // Activer le X de complétion si la journée appartient au passé
            if (i < currentDay - 1)
                daySlot.completedIcon.gameObject.SetActive(true);

            // S'il s'agit d'un discount
            var discount = discountsList.Where(d => d.day == i).FirstOrDefault();
            if (discount != null)
            {
                daySlot.icon.sprite = discount.sprite;
                daySlot.border.gameObject.SetActive(true);
            }
        }
    }
}
