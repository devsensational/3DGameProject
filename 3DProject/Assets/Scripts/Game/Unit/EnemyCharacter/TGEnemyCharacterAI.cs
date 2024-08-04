using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum CharacterStatus
{
    Moving = 0x00,
    Reloading = 0x01,
    Dead = 0x02,
    Stun = 0x04,
}


// �� ĳ������ AI�� �����մϴ�
// �� �̻� ������� �ʽ��ϴ�.
public class TGEnemyCharacterAI : MonoBehaviour
{
    //Inspector
    [Header("Checking parameters")]
    [SerializeField] List<GameObject> coverableObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> playerCharacterList = new List<GameObject>();
    [SerializeField] float speed = 0;

    [Header("AI Options")]
    public float stoppingDistance       = 1.0f; // ���� �Ÿ��� ������ ��� �̵� �ߴ��� ���� �Ķ����
    public float nextFireIntervalTime   = 2f;   // ���� �߻� ���͹� Ÿ�� �Ķ���� (���� AI �ൿ ���� Ÿ�̸�)
    public float burstCount             = 5f;   // �� ���߷� �߻��� ������ �����ϴ� �Ķ����

    //private
    TGEnemyCharacter    character;
    NavMeshAgent        nav;
    Animator            animator;
    CapsuleCollider     capsuleCollider;

    WaitForSeconds autoWeaponFireWaitForSeconds;
    
    RaycastHit hit;

    float timer = 5f;

    int AIStatusFlags = 0x00;
    int layerMask;

    bool canShoot = false;

    //Unity lifecylce
    void Start()
    {
        InitReference();
    }

    void Update()
    {
        AITimer();      // AI �ൿ Ÿ�̸�
        MoveToPlayer(); // 
        AIStatusControl();

        speed = nav.speed;

        if (RaycastObstacleCheck())
        {
            DetermineFire();
            transform.LookAt(playerCharacterList[0].transform);
            nav.stoppingDistance = stoppingDistance;
        }
        else
        {
            
            //nav.stoppingDistance = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerCharacterList.Add(other.gameObject);
        }
        if(other.gameObject.tag == "CoverableObject")
        {
            coverableObjectList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCharacterList.Remove(other.gameObject);
        }
        if (other.gameObject.tag == "CoverableObject")
        {
            coverableObjectList.Remove(other.gameObject); 
        }
    }

    //Init
    void InitReference()
    {
        character       = GetComponent<TGEnemyCharacter>();
        nav             = GetComponent<NavMeshAgent>();
        animator        = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

    }


    // ���� AI ����
    void AITimer()
    {
        if (timer > 0f) timer -= Time.deltaTime;
    }

    void AIStatusControl()
    {
        if(!nav.pathPending && nav.remainingDistance > nav.stoppingDistance)
        {
            AIStatusFlags |= (int)CharacterStatus.Moving; //Moving ���� �Ҵ�
        }
        else
        {
            AIStatusFlags &= ~(int)CharacterStatus.Moving; //Moving ���� ����
        }
    }

    // AI ���� ����
    void DetermineFire()
    {
        if (character.HandInItem == EEquipmentType.None || character.HandInItem == EEquipmentType.Default) return;
        if (playerCharacterList.Count == 0) return;
        if (timer > 0f) return;

        TGItemWeapon itemPtr = (TGItemWeapon)character.equipItems[character.HandInItem];
        autoWeaponFireWaitForSeconds = new WaitForSeconds(60 / itemPtr.weaponStats.fireRate);

        if (itemPtr.currentAmmo <= 0) // ź�� ���� ��� ������
        {
            character.CommandReloadInHandItem();
            HideToCoverableObject();
            timer = itemPtr.weaponStats.reloadTime + 1;
            AIStatusFlags |= (int)CharacterStatus.Reloading; 

            return;
        }

        if (itemPtr.currentAmmo >= 0)
        {
            StartCoroutine(Fireintermittently());
            AIStatusFlags &= ~(int)CharacterStatus.Reloading;
        }

        timer = nextFireIntervalTime;
    }

    IEnumerator Fireintermittently()
    {
        for (int i = 0; i < burstCount; i++)
        {
            character.equipItems[character.HandInItem].UseItem();
            yield return autoWeaponFireWaitForSeconds;
        }
    }

    // AI �̵� ����
    void MoveToPlayer()
    {
        if (playerCharacterList.Count < 1) return;
        if (nav == null) return;

        if(AIStatusFlags <= 0)
        {
            animator.SetFloat("Speed", nav.speed);
            nav.SetDestination(playerCharacterList[0].transform.position);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void HideToCoverableObject() // ���󰡴��� ��ֹ��� �̵���Ű�� �޼ҵ� (���⼭ ���� ���� �ʿ�)
    {
        if (playerCharacterList.Count < 1) return;
        if (coverableObjectList.Count < 1) return;
        if (nav == null) return;

        // ���� ����� ��ֹ��� �˻�
        Vector3 pos = coverableObjectList[0].transform.position;
        float distance = Vector3.Distance(transform.position, coverableObjectList[0].transform.position);

        for(int i = 1; i < coverableObjectList.Count; i++)
        {
            Vector3 comparePos = coverableObjectList[i].transform.position;
            float compareDist = Vector3.Distance(transform.position, comparePos);
            if (distance > compareDist)
            {
                pos = comparePos;
                distance = compareDist;
            }
        }

        Vector3 obstaclePosition = (pos - playerCharacterList[0].transform.position) * 1.1f;
        Debug.Log($"(TGEnemyCharacterAI:HideToCoverableObject) move at position: {obstaclePosition}");

        nav.stoppingDistance = 0f;
        nav.SetDestination(pos);
    }

    bool RaycastObstacleCheck() // Player�� AI�� ��ֹ� ���� üũ �޼ҵ�, �������� ������ true ��ȯ
    {
        if (playerCharacterList.Count < 1) return false;

        Vector3 direction = playerCharacterList[0].GetComponent<Collider>().bounds.center - transform.position;
        Debug.DrawRay(transform.position, direction);

        if (Physics.Raycast(transform.position, direction, out hit, direction.magnitude, layerMask))
        {
            if (hit.transform.gameObject.layer == 6)
            {
                return false;
            }
        }

        return true;
    }

}
