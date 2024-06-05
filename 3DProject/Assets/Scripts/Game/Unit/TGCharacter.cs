using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ������Ʈ�� ������ �����ϱ� ���� Ŭ�����Դϴ�.
public class TGCharacter : TGObject
{
    //public
    public GameObject HandObject;   //�������� �ֿ��� �� ��ġ�� ������ GameObject

    //protected
    protected Dictionary<ItemType, TGItem> equipItems = new Dictionary<ItemType, TGItem>();
    protected TGItem handInItem = null;   //�÷��̾� ĳ���Ͱ� ��� �ִ� ������ ref

    //private
    
    
    //Unity lifecycle
    private void Awake()
    {
        equipItems.Add(ItemType.PrimaryWeapon, null);
        equipItems.Add(ItemType.SecondaryWeapon, null);

        ChildAwake();
    }

    // item ��ȣ�ۿ� ���� �޼ҵ�
    protected void TakeItem(TGItem item)    // item ���� �õ� �޼ҵ�
    {
        if(item == null) return;                        // ���õ� item object�� null�� ��� �޼ҵ� ����

        equipItems[item.itemType] = item;
        equipItems[item.itemType].OnPickedUpThisItem(gameObject);        // Item�� ���� ���� �� item instance�� ����Ǿ� �� ��� ����
        Debug.Log(gameObject.name + " picked up " + item.name);
    }

    protected void DropItem(ItemType itemType)  // item ��� �õ� �޼ҵ�
    {
        equipItems[itemType] = null;
    }

    // �̵� ���� �޼ҵ�
    public void CommandMove(Vector3 direction, float moveSpeed)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // getter/setter
    public TGItem GetHandInItem()
    {
        return handInItem;
    }

    // child
    protected virtual void ChildAwake() {}

}
