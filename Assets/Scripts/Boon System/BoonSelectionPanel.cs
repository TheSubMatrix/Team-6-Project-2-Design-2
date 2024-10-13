using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoonSelectionPanel : MonoBehaviour
{
    [SerializeField] List<Image> godSelectionPanels = new List<Image>();
    [SerializeField] List<GodData> selectedGods;

    void Start()
    {
        selectedGods = new List<GodData>(RunManager.Instance.Gods);
        foreach(Image panel in godSelectionPanels)
        {
            if(selectedGods.Count > 0)
            {
                int selectedGodIndex = Random.Range(0, selectedGods.Count);
                panel.sprite = selectedGods[selectedGodIndex].AssociatedImage;
                panel.transform.Find("Title Text").GetComponent<TMP_Text>().text = selectedGods[selectedGodIndex].Name;
                panel.transform.Find("Description Text").GetComponent<TMP_Text>().text = selectedGods[selectedGodIndex].Description;
                selectedGods.RemoveAt(selectedGodIndex);
            }
        }
    }
}
