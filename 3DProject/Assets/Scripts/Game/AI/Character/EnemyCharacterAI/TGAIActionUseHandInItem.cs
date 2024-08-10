using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class TGAIActionUseHandInItem : TGAIActionNodeBase
{
    // Inspector
    [Header("Attack options")]
    public int      burstCount;
    public float    ItemCooldown;

    // private
    List<GameObject>    playerLists;
    TGCharacter         character;
    NavMeshAgent        agent;


    // Unity Lifecycle
    public override void Start()
    {
        base.Start();
        character   = controller.character;
        agent       = GetComponent<NavMeshAgent>();
        playerLists = controller.playerCharacterList;
    }

    private void Update()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }


    public override IEnumerator Run()
    {
        if(character == null)                           { yield break; }
        if(character.HandInItem == EEquipmentType.None) { yield break; }
        if(cooldown > 0)                                { yield break; }

        agent.isStopped = true;

        TGItemWeapon weapon = character.GetInHandWeapon();

        WaitForSeconds autoWeaponFireWaitForSeconds = new WaitForSeconds(60 / weapon.weaponStats.fireRate);

        transform.LookAt(playerLists[0].transform.position);

        for (int i = 0; i < burstCount; i++)
        {
            weapon.UseItem();
            yield return autoWeaponFireWaitForSeconds;
        }

        cooldown = ItemCooldown;
    }
}
