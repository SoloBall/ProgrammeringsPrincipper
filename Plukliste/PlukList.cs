﻿namespace PlukListe;
public class Pluklist
{
    public string? Name;
    public string? Forsendelse;
    public string? Adresse;
    public List<Item> Lines = new List<Item>();
    public void AddItem(Item item) 
    { 
        Lines.Add(item); 
    }
}
// TODO: Change field names to be consistent english on website 😡
public class Item
{
    public string? ProductID;
    public string? Title;
    public ItemType Type;
    public int Amount;
}

public enum ItemType
{
    Fysisk, Print
}



