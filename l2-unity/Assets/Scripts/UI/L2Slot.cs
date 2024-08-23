using System;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class L2Slot
{
    public enum SlotType
    {
        Other,
        Inventory,
        InventoryBis, //other tab slots
        Gear,
        Skill,
        SkillBar,
        Action,
        Trash
    }

    [SerializeField] protected int _id;
    [SerializeField] protected int _position;
    [SerializeField] protected SlotType _slotType;
    protected string _name;
    protected string _description;
    protected string _icon;
    protected VisualElement _slotElement;
    protected VisualElement _slotBg;
    protected TooltipManipulator _tooltipManipulator;
    protected SlotHoverDetectManipulator _hoverManipulator;

    public int Id { get { return _id; } set { _id = value; } }
    public int Position { get { return _position; } set { _position = value; } }
    public SlotType Type { get { return _slotType; } set { _slotType = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public string Description { get { return _description; } set { _description = value; } }
    public string Icon { get { return _icon; } set { _icon = value; } }
    public VisualElement SlotBg { get { return _slotBg; } }
    public VisualElement SlotElement { get { return _slotElement; } }

    public L2Slot(VisualElement slotElement, int position, int id, string name, string description, string icon, SlotType type)
    {
        _slotElement = slotElement;
        if (type != SlotType.Other)
        {
            _slotElement.AddToClassList("empty");
        }
        else
        {
            _slotElement.AddToClassList("dragged");
        }

        _position = position;
        _id = id;
        _name = name;
        _description = description;
        _icon = icon;
        _slotType = type;
        _slotBg = _slotElement.Q<VisualElement>(null, "slot-bg");
    }

    public L2Slot(VisualElement slotElement, int position, SlotType type)
    {
        _slotElement = slotElement;
        _slotElement.AddToClassList("empty");
        _position = position;
        _slotType = type;

        if (slotElement == null)
        {
            return;
        }

        _slotBg = _slotElement.Q<VisualElement>(null, "slot-bg");

        if (_tooltipManipulator == null)
        {
            _tooltipManipulator = new TooltipManipulator(_slotElement, "");
            _slotElement.AddManipulator(_tooltipManipulator);
        }

        if (_hoverManipulator == null)
        {
            _hoverManipulator = new SlotHoverDetectManipulator(_slotElement, this);
            _slotElement.AddManipulator(_hoverManipulator);
        }

        RegisterCallbacks();
    }

    protected void HandleSlotClick(MouseDownEvent evt)
    {
        if (evt.button == 0)
        {
            HandleLeftClick();
        }
        else if (evt.button == 1)
        {
            HandleRightClick();
        }
        else
        {
            HandleMiddleClick();
        }
    }

    protected void RegisterCallbacks()
    {
        if (_slotElement == null)
        {
            return;
        }

        _slotElement.RegisterCallback<MouseDownEvent>(HandleSlotClick, TrickleDown.TrickleDown);
    }

    public void UnregisterCallbacks()
    {
        if (_slotElement == null)
        {
            return;
        }

        _slotElement.UnregisterCallback<MouseDownEvent>(HandleSlotClick, TrickleDown.TrickleDown);
    }

    protected virtual void HandleLeftClick() { }
    protected virtual void HandleRightClick() { }
    protected virtual void HandleMiddleClick() { }

    public void SetSelected()
    {
        Debug.Log($"Slot {_position} selected.");
        _slotElement.AddToClassList("selected");
    }

    public void UnSelect()
    {
        Debug.Log($"Slot {_position} unselected.");
        _slotElement.RemoveFromClassList("selected");
    }

    public virtual void ClearManipulators()
    {
        if (_slotElement == null)
        {
            return;
        }

        if (_tooltipManipulator != null)
        {
            _tooltipManipulator.Clear();
            _slotElement.RemoveManipulator(_tooltipManipulator);
            _tooltipManipulator = null;
        }

        if (_hoverManipulator == null)
        {
            _slotElement.RemoveManipulator(_hoverManipulator);
        }
    }
}
