using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 物品品级
/// </summary>
public enum Quality
{
    Common, Uncommon, Rere, Epic
}

public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>() {
        { Quality.Common,"#ffffffff"},
        { Quality.Uncommon ,"#00ff00ff"},
        { Quality.Rere,"#0E6BECFF" },
        { Quality.Epic,"#A712dbff"}
    };

    public static Dictionary<Quality, string> MyColors
    {
        get => colors;
    }
}