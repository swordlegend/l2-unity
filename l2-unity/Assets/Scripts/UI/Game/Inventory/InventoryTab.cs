using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class InventoryTab : L2Tab {
    private InventorySlot[] _inventorySlots;
    private int _selectedSlot = -1;

    private VisualElement _contentContainer;

    public override void Initialize(VisualElement chatWindowEle, VisualElement tabContainer, VisualElement tabHeader) {
        base.Initialize(chatWindowEle, tabContainer, tabHeader);

        _selectedSlot = -1;
        _contentContainer = tabContainer.Q<VisualElement>("Content");
    }

    public void UpdateItemList(List<ItemInstance> items) {
        // Clear slots
        _contentContainer.Clear();

        // Create empty slots
        int slotCount = 80;
        _inventorySlots = new InventorySlot[slotCount];
        for (int i = 0; i < slotCount; i++) {
            VisualElement slotElement = InventoryWindow.Instance.InventorySlotTemplate.Instantiate()[0];
            _contentContainer.Add(slotElement);

            InventorySlot slot = new InventorySlot(i, slotElement, this);
            _inventorySlots[i] = slot;
        }

        // Add disabled slot to fill up the window
        int padSlot = 0;
        if (slotCount < 72) {
            padSlot = 72 - slotCount;
        } else if (slotCount % 9 != 0) {
            padSlot = 9 - slotCount % 9;
        }

        for (int i = 0; i < padSlot; i++) {
            VisualElement slotElement = InventoryWindow.Instance.InventorySlotTemplate.Instantiate()[0];
            slotElement.AddToClassList("inventory-slot");
            slotElement.AddToClassList("disabled");
            _contentContainer.Add(slotElement);
        }

        // Assign items to slots
        items.ForEach(item => {
            _inventorySlots[item.Slot].AssignItem(item);
        });
    }

    public void SelectSlot(int slotPosition) {
        if(_selectedSlot != slotPosition) {
            if(_selectedSlot != -1) {
                _inventorySlots[_selectedSlot].UnSelect();
            }
            _inventorySlots[slotPosition].SetSelected();
            _selectedSlot = slotPosition;
        }
    }

    protected override void OnGeometryChanged() {
    }

    protected override void OnSwitchTab() {
    }

    protected override void RegisterAutoScrollEvent() {
    }
}
