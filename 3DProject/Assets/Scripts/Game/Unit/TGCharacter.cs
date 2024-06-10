using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

// �� ������Ʈ�� ������ �����ϱ� ���� Ŭ�����Դϴ�.
public class TGCharacter : TGObject
{
    //Inspector
    public GameObject HandPosition;   //�������� �ֿ��� �� ��ġ�� ������ GameObject

    //public
    public MCharacterStats characterStat { get; protected set; } // �÷��̾� ĳ���� ����

    public Dictionary<EItemType, TGItem> equipItems = new Dictionary<EItemType, TGItem>();

    public List<TGItem> inventory = new List<TGItem>(); // �ֿ� ������ ����Ʈ

    //protected
    protected EItemType handInItem = EItemType.Default;   //�÷��̾� ĳ���Ͱ� ��� �ִ� ������ ref

    //private
    
    
    //Unity lifecycle
    private void Awake()
    {
        equipItems.Add(EItemType.PrimaryWeapon, null);
        equipItems.Add(EItemType.SecondaryWeapon, null);

        ChildAwake();
    }

    // item ��ȣ�ۿ� ���� �޼ҵ�
    public void TakeItem(TGItem item)    // item ���� �õ� �޼ҵ�
    {
        if(item == null) return;                        // ���õ� item object�� null�� ��� �޼ҵ� ����

        equipItems[item.itemType] = item;
        equipItems[item.itemType].OnPickedUpThisItem(gameObject);        // Item�� ���� ���� �� item instance�� ����Ǿ� �� ��� ����
        Debug.Log(gameObject.name + " picked up " + item.name);
    }

    public void DropItem(EItemType itemType)  // item ��� �õ� �޼ҵ�
    {
        equipItems[itemType].OnDropThisItem();
        equipItems[itemType] = null;
    }

    // �̵� ���� �޼ҵ�
    public void CommandMove(Vector3 direction, float moveSpeed)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // getter/setter
    public EItemType GetHandInItem()
    {
        return handInItem;
    }

    // child
    protected virtual void ChildAwake() {}

}
