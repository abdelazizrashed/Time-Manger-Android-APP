using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorModel
{
    public int colorID { get; set;}
    public string colorName { get; set; }
    public string colorValue { get; set; }

    public ColorModel(int id, string name, string value)
    {
        colorID = id;
        colorName = name;
        colorValue = value;
    }

    public static ColorModel GetColorByColorID(int colorID)
    {
        //Todo: implement this method
    }

    public static ColorModel[] GetColors()
    {
        //Todo: implement this method
        ColorModel[] colors = new ColorModel[] {
            new ColorModel(1, "Black", "000000"),
            new ColorModel(2, "White", "FFFFFF"),
            new ColorModel(3, "Red", "FF0000"),
            new ColorModel(4, "Green", "00FF00"),
            new ColorModel(5, "Blue", "0000FF")
        };
        return colors;
    }
}
