using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LevelAbility : ScriptableObject
{
    public CardInfo abilityInfo;
    
    [SerializeField]
    protected int maxAbilityLevel;
    [SerializeField]
    protected int nowAbilityLevel;

    public bool isActived = false;
    public bool update = false;
    public float updateDeltatime = 1f;

    public bool maxLevel => maxAbilityLevel == nowAbilityLevel;

    public virtual LevelAbility Init()
    {
        isActived = false;
        nowAbilityLevel = 0;
        var ability = Instantiate(this);
        return ability;
    }

    public void AbilityLevelUp()
    {
        isActived = false;
        nowAbilityLevel = Mathf.Clamp(nowAbilityLevel + 1, 0, maxAbilityLevel);
        UseAbility();
    }

    public void UseAbility()
    {
        StartAbility();
        if (update)
            PlayerManager.Instance.Player.StartCoroutine(UpdateCoroutine());
    }

    //�ѹ��� �����Ѵ�    
    protected virtual void StartAbility()
    {
        isActived = true;
    }

    //�ڷ�ƾ�� Ȱ���� ��� �����ش�
    private IEnumerator UpdateCoroutine()
    {
        while (isActived)
        {
            yield return new WaitForSeconds(updateDeltatime);
            UpdateAbility();
        }
    }

    protected virtual void UpdateAbility()
    {

    }

    public virtual void CollisionAbility()
    {
        
    }

}
