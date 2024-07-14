using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ������Ʈ�� Item�� �����ϴ� �ֻ��� Ŭ�����Դϴ�.
public class TGItem : TGObject
{
    //Inspector
    public GameObject itemModel;
    public string groundTag = "Terrain"; // �ٴڿ� ����� �±�

    public int itemCount = 0; //�������� ����

    //public
    public TGUILootableItemInterective  itemButton;
    public EEquipmentType               equipmentType = EEquipmentType.None;
    public EItemType                    itemType;

    public GameObject   itemHolder { get; set; }    //�������� �ֿ� ������Ʈ

    public bool         isDropped = false;          //�������� �ֿ� �� �ִ� �������� Ȯ��

    public MCharacterStats characterStats { get; set; }

    //protected
    protected bool isHandIn = false;


    //private
    float maxRayDistance = 100f; // Raycast�� �ִ� �Ÿ�

    Rigidbody rb;
    Collider cl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();

        ChildAwake();
    }

    void Start()
    {
        ChildStart();
    }

    void Update()
    {
        ChildUpdate();
    }

    private void OnDestroy()
    {
        Destroy(itemButton.gameObject);
    }

    // ������ ��ȣ�ۿ� ���� �޼ҵ�
    public void OnDropThisItem()    // �������� �������� �� ȣ��
    {
        itemHolder = null;          //�ش� ������ ������ �ʱ�ȭ
        isDropped = true;           //������ �ִ� ���·� ����
        isHandIn = false;   
        transform.SetParent(null);  //�θ� ����
        itemModel.SetActive(true);

        DropToGround();

        //cl.isTrigger = false;
        //rb.isKinematic = false;
    }


    public void OnPickedUpThisItem(GameObject pickedUpCharacterObject)    // �������� �ֿ����� �� ȣ��
    {
        itemHolder = pickedUpCharacterObject;
        isDropped = false;

        TGCharacter chracterComponent = itemHolder.GetComponent<TGCharacter>();

        // �θ� ����
        transform.SetParent(chracterComponent.HandPosition.transform);
        characterStats = chracterComponent.characterStat; //�ֿ� ĳ������ ���� ������ ��������

        // ��ġ �� ȸ�� �ʱ�ȭ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if(chracterComponent.GetInHandItem() != equipmentType)
        {
            itemModel.SetActive(false);
        }

    }

    public void OnHandInThisItem() // ĳ���Ͱ� �ش� �������� �տ� ����� �� ȣ��
    {
        if(isHandIn)
        {
            isHandIn = false;
            itemModel.SetActive(false);
            Debug.Log($"(TGItem:OnHandInThisItem) {objectName} hand off from {itemHolder.name}");
        }
        else
        {
            isHandIn = true;
            itemModel.SetActive(true);
            Debug.Log($"(TGItem:OnHandInThisItem) {objectName} on hand to {itemHolder.name}");
        }
    }

    public void UpdateButtonUI()     // �ش� �����ۿ� �ش��ϴ� UI ��ư ����
    {
        itemButton.SetItemName();
        Debug.Log($"(TGItem:OnHandInThisItem) Updated item button UI");
    }

    void DropToGround()     // �������� ������ ������ �����ų �� ����ϴ� �޼ҵ�
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, maxRayDistance);

        if (hits.Length > 0)
        {
            bool groundFound = false;
            foreach (RaycastHit hit in hits)
            {
                // Raycast�� �ٴڿ� ��Ҵ��� Ȯ��
                if (hit.collider.CompareTag(groundTag))
                {
                    // �ٴڿ� ����� ��� ������Ʈ�� ��ġ�� �ٴ��� ��ġ�� ����
                    Vector3 newPosition = hit.point;
                    transform.position = newPosition;

                    // ������Ʈ�� X������ 90�� ȸ��
                    transform.rotation = Quaternion.Euler(0, 0, 90);

                    Debug.Log("(TGItem:DropToGround) Object dropped to ground at position: " + newPosition);
                    groundFound = true;
                    break; // groundTag�� ���� ��ü�� ã�����Ƿ� ������ �����մϴ�
                }
            }

            if (!groundFound)
            {
                Debug.Log("(TGItem:DropToGround) Raycast did not hit an object with the tag: " + groundTag);
            }
        }
        else
        {
            Debug.Log("(TGItem) Raycast did not hit anything.");
        }
    }

    public void ReduceItemCount(int value) // ������ ī��Ʈ�� �ܺη� ���� ���ҽ�ų �� ���
    {
        if (itemCount >= 1)
        {
            itemCount -= value;
            if (itemCount <= 0) 
            { 
                Destroy(gameObject); 
            }
        }
    }

    public virtual void UseItem() 
    {
        Debug.Log("(TGItem:UseItem) Use this item: " + objectName);
    }

    protected virtual IEnumerator Reload()
    {
        yield return null;
    }

    //�θ��� �������� �޼ҵ带 �������� �ʰ� �޼ҵ� �ۼ��� �����ϰ� �ϴ� �޼ҵ��
    protected virtual void ChildAwake() { }     
    protected virtual void ChildUpdate() { }
    protected virtual void ChildStart() { }     
}
