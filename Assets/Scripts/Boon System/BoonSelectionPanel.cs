using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoonSelectionPanel : MonoBehaviour
{
    [SerializeField] List<Image> godSelectionPanels = new List<Image>();
    [SerializeField] List<GodData> selectedGods;
    [SerializeField] List<GodData> allSelectedGods;

    void Start()
    {
        selectedGods = new List<GodData>(RunManager.Instance.Gods);
        allSelectedGods = new List<GodData>(selectedGods);
        foreach(Image panel in godSelectionPanels)
        {
            if(selectedGods.Count > 0)
            {
                int selectedGodIndex = Random.Range(0, selectedGods.Count);
                panel.sprite = selectedGods[selectedGodIndex].AssociatedImage;
                panel.transform.Find("Title Text").GetComponent<TMP_Text>().text = selectedGods[selectedGodIndex].Name;
                panel.transform.Find("Description Text").GetComponent<TMP_Text>().text = selectedGods[selectedGodIndex].Description;
                panel.transform.Find("Select Button").GetComponent<Button>().onClick.AddListener(() => OnGodSelectionButtonPressed(allSelectedGods[selectedGodIndex]));
                selectedGods.RemoveAt(selectedGodIndex);
            }
        }
    }
    void OnGodSelectionButtonPressed(GodData selectedGod)
    {
        RunManager.Instance.SetupBoonSelection(selectedGod);
        RunManager.Instance.Player?.GetComponentInChildren<PlayerInteractionHandler>()?.ForceEndInteraction();
    }
}
