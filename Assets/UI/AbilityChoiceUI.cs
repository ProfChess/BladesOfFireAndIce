using UnityEngine;
using UnityEngine.UI;

public class AbilityChoiceUI : MonoBehaviour
{
    private Ability fireAbilityRef;
    private Ability iceAbilityRef;
    [SerializeField] private Text fireTitle;
    [SerializeField] private Text fireDescription;
    [SerializeField] private Text iceTitle;
    [SerializeField] private Text iceDescription;

    public void AssignVisuals(Ability fireAbility, Ability iceAbility)
    {
        if (fireAbility != null && iceAbility != null)
        {
            //Fire
            fireAbilityRef = fireAbility;
            fireTitle.text = fireAbility.BonusName;
            fireDescription.text = fireAbility.BonusDescription;
            
            //Ice
            iceAbilityRef = iceAbility;
            iceTitle.text = iceAbility.BonusName;
            iceDescription.text = iceAbility.BonusDescription;
        }
    }

    public void Select()
    {
        GameManager.Instance.abilityOptions.AbilitySelected(fireAbilityRef, iceAbilityRef);
    }
}
