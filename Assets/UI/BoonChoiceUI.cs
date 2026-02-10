using UnityEngine;
using UnityEngine.UI;

public class BoonChoiceUI : MonoBehaviour
{
    //Current Boon To Display
    private BaseBoon displayedBoon;

    //Fields for Visual Elements
    [SerializeField] private Text TitleText;
    [SerializeField] private Text DescriptionText;
    [SerializeField] private Text LevelNumText;
    //[SerializeField] private Image iconImage;

    public void AssignBoonVisuals(BaseBoon boon)
    {
        displayedBoon = boon;
        TitleText.text = boon.boonName;
        DescriptionText.text = boon.boonDescription;
        //iconImage.sprite = boon.Icon;


        if (boon is Virtue virtue)
        {
            bool collected = GameManager.Instance.runData.IsVirtueCollected(virtue);
            if (!collected) { LevelNumText.text = "1"; }
            else { LevelNumText.text = "2"; }
        }
    }

    public void Select()
    {
        GameManager.Instance.boonOptions.ChoiceMade(displayedBoon);
    }
}
