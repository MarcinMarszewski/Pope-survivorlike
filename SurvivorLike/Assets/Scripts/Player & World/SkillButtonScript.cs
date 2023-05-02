using UnityEngine;
using UnityEngine.EventSystems;

public sealed class SkillButtonScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Skills skill;

    [SerializeField]
    Skills skillNeeded;

    [SerializeField]
    int cost;

    [SerializeField]
    GameObject enableOnRightClick;

    [SerializeField]
    GameObject enableOnSkillUpgrade;

    PlayerController player;
    GameObject[] arr;

    bool isEnabled = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        arr = GameObject.FindGameObjectsWithTag("SkillButton");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button ==PointerEventData.InputButton.Left)
        {
            if (player.skills.IsSkillUnlocked(skillNeeded) && player.skillPoints >= cost&&isEnabled)
            {
                isEnabled = false;
                player.skills.UnlockSkill(skill);
                player.skillPoints -= cost;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>().UpdateSkillPointIndicator();
                enableOnSkillUpgrade.SetActive(true);

                foreach (var item in arr)
                {
                    item.GetComponent<SkillButtonScript>().CheckAndUpdateSkillAvailability();
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(enableOnRightClick != null) enableOnRightClick.SetActive(!enableOnRightClick.activeSelf);
        }
    }

    public void CheckAndUpdateSkillAvailability()
    {
        if(player.skills.IsSkillUnlocked(skillNeeded)&&!player.skills.IsSkillUnlocked(skill))
        {
            enableOnSkillUpgrade.SetActive(false);
            return;
        }
        enableOnSkillUpgrade.SetActive(true);
    }
}
