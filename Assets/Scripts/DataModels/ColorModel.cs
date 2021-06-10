using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        //this is temprary untill the link with the online api
        ColorModel[] colors = new ColorModel[] {
            new ColorModel(1, "Black", "000000"),
            new ColorModel(2, "White", "FFFFFF"),
            new ColorModel(3, "Red", "FF0000"),
            new ColorModel(4, "Green", "00FF00"),
            new ColorModel(5, "Blue", "0000FF")
        };
        foreach(ColorModel color in colors)
        {
            if (color.colorID == colorID)
            {
                return color;
            }
        }

        string query = "SELECT * FROM Colors WHERE color_id = " + colorID + ";";

        colors = Enumerable.ToArray < ColorModel > (DBMan.Instance.ExecuteQueryAndReturnRows<ColorModel>(query, reader => {
            return new ColorModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2)
                );
        }));
        return colors[0];
    }

    public static ColorModel[] GetColors()
    {
        //This is the main logic
        string query = "SELECT * FROM Colors;";
        //IDataReader reader = DBMan.Instance.ExecuteQueryAndReturnDataReader(query);
        //List<ColorModel> dbColors = new List<ColorModel>();
        //while (reader.Read())
        //{
        //    DBMan.Instance.PrintDataReader(reader);
        //    dbColors.Add(new ColorModel(
        //        reader.GetInt32(0),
        //        reader.GetString(1),
        //        reader.GetString(2)
        //        ));
        //}
        //reader?.Close();
        //reader = null;
        ColorModel[] colors = Enumerable.ToArray < ColorModel > (DBMan.Instance.ExecuteQueryAndReturnRows<ColorModel>(query, reader => {
            return new ColorModel(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2)
                );
        }));
        //TODO:This is temprary until conntecting the app with the online api
        colors = new ColorModel[] {
            new ColorModel(1, "Black", "000000"),
            new ColorModel(2, "White", "FFFFFF"),
            new ColorModel(3, "Red", "FF0000"),
            new ColorModel(4, "Green", "00FF00"),
            new ColorModel(5, "Blue", "0000FF")
        };
        return colors;
    }
}
