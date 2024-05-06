﻿namespace PickList;
public class PickList
{
    public string? Name;
    public string? Shipment;
    public string? Address;
    public List<Item> Lines = new List<Item>();
    public void AddItem(Item item) { Lines.Add(item); }
}

public class Item
{
    public string? ProductID;
    public string? Title;
    public ItemType Type;
    public int Amount;
}

public enum ItemType
{
    Physical, Print
}


