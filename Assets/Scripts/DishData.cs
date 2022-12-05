using System.Collections.Generic;

[System.Serializable]
public class DishData
{
    public List<Dish> Dish_Data = new List<Dish>();
    public List<Clue> Clues = new List<Clue>();
    public List<HidingSpot> HidingSpots = new List<HidingSpot>();

    [System.Serializable]
    public class Dish
    {
        public string Id;
        public string Dish_Name;
        public List<Ingredient> Ingredients = new List<Ingredient>();
    }

    [System.Serializable]
    public class Ingredient
    {
        public string name;
    }

    [System.Serializable]
    public class Clue
    {
        public string object_name;
        public string clue;
        public string location;
    }

    [System.Serializable]
    public class HidingSpot
    {
        public string object_name;
        public string hidingspot;
    }
}
