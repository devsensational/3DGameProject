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


// 적 캐릭터의 AI를 구성합니다
// 더 이상 사용하지 않습니다.
public class TGEnemyCharacterAI : MonoBehaviour
{
    //Inspector
    [Header("Checking parameters")]
    [SerializeField] List<GameObject> coverableObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> playerCharacterList = new List<GameObject>();
    [SerializeField] float speed = 0;

    [Header("AI Options")]
    public float stoppingDistance       = 1.0f; // 일정 거리에 도달할 경우 이동 중단을 위한 파라미터
    public float nextFireIntervalTime   = 2f;   // 다음 발사 인터벌 타임 파라미터 (다음 AI 행동 결정 타이머)
    public float burstCount             = 5f;   // 몇 연발로 발사할 것인지 결정하는 파라미터

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
        AITimer();      // AI 행동 타이머
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


    // 유닛 AI 관련
    void AITimer()
    {
        if (timer > 0f) timer -= Time.deltaTime;
    }

    void AIStatusControl()
    {
        if(!nav.pathPending && nav.remainingDistance > nav.stoppingDistance)
        {
            AIStatusFlags |= (int)CharacterStatus.Moving; //Moving 상태 할당
        }
        else
        {
            AIStatusFlags &= ~(int)CharacterStatus.Moving; //Moving 상태 해제
        }
    }

    // AI 공격 관련
    void DetermineFire()
    {
        if (character.HandInItem == EEquipmentType.None || character.HandInItem == EEquipmentType.Default) return;
        if (playerCharacterList.Count == 0) return;
        if (timer > 0f) return;

        TGItemWeapon itemPtr = (TGItemWeapon)character.equipItems[character.HandInItem];
        autoWeaponFireWaitForSeconds = new WaitForSeconds(60 / itemPtr.weaponStats.fireRate);

        if (itemPtr.currentAmmo <= 0) // 탄이 없을 경우 재장전
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

    // AI 이동 관련
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

    void HideToCoverableObject() // 엄폐가능한 장애물로 이동시키는 메소드 (여기서 부터 수정 필요)
    {
        if (playerCharacterList.Count < 1) return;
        if (coverableObjectList.Count < 1) return;
        if (nav == null) return;

        // 가장 가까운 장애물을 검색
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

    bool RaycastObstacleCheck() // Player와 AI간 장애물 유무 체크 메소드, 존재하지 않으면 true 반환
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
