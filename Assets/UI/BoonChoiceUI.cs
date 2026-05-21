using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BoonChoiceUI : BaseStatUIDisplay
{
    //Current Boon To Display
    private BaseBoon displayedBoon;

    //Fields for Visual Elements
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI DescriptionText;
    [SerializeField] private TextMeshProUGUI LevelNumText;
    
    //[SerializeField] private Image iconImage;

    public void AssignBoonVisuals(BaseBoon boon)
    {
        ClearStatList();
        displayedBoon = boon;
        TitleText.text = boon.BonusName;
        DescriptionText.text = boon.BonusDescription;
        //iconImage.sprite = boon.Icon;


        if (boon is Virtue virtue)
        {
            bool collected = GameManager.Instance.runData.IsVirtueCollected(virtue);
            if (!collected) 
            { 
                LevelNumText.text = "1"; AssignStatsFromItem(boon.GetListOfStatsForDisplay());
            }
            else 
            { 
                LevelNumText.text = "2";
                AssignStatsFromItem(boon.GetLeveledStatsPreview());
            }
        }
        else
        {
            AssignStatsFromItem(boon.GetListOfStatsForDisplay());
        }
    }

    public void Select()
    {
        GameManager.Instance.boonOptions.ChoiceMade(displayedBoon);
    }
}
