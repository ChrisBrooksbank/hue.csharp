using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrisBrooksbank.Hue.Interfaces
{
    public enum NamedColour
    {
        AliceBlue, AntiqueWhite, Aqua, Aquamarine, Azure, Beige, Bisque, Black, BlanchedAlmond, Blue, BlueViolet, Brown, Burlywood,
        CadetBlue, Chartreuse, Chocolate, Coral, Cornflower, Cornsilk, Crimson, Cyan, DarkBlue, DarkCyan, DarkGoldenrod, DarkGray, DarkGreen, DarkKhaki, DarkMagenta,
        DarkOliveGreen, DarkOrange, DarkOrchid, DarkRed, DarkSalmon, DarkSeaGreen, DarkSlateBlue, DarkSlateGray, DarkTurquoise, DarkViolet, DeepPink, DeepSkyBlue,
        DimGray, DodgerBlue, Firebrick, FloralWhite, ForestGreen, Fuchsia, Gainsboro, GhostWhite, Gold, Goldenrod, Gray, WebGray, Green, WebGreen, GreenYellow, Honeydew,
        HotPink, IndianRed, Indigo, Ivory, Khaki, Lavender, LavenderBlush, LawnGreen, LemonChiffon, LightBlue, LightCoral, LightCyan, LightGoldenrod, LightGray, LightGreen,
        LightPink, LightSalmon, LightSeaGreen, LightSkyBlue, LightSlateGray, LightSteelBlue, LightYellow, Lime, LimeGreen, Linen, Magenta, Maroon, WebMaroon,
        MediumAquamarine, MediumBlue, MediumOrchid, MediumPurple, MediumSeaGreen, MediumSlateBlue, MediumSpringGreen, MediumTurquoise, MediumVioletRed, MidnightBlue,
        MintCream, MistyRose, Moccasin, NavajoWhite, NavyBlue, OldLace, Olive, OliveDrab, Orange, OrangeRed, Orchid, PaleGoldenrod, PaleGreen, PaleTurquoise,
        PaleVioletRed, PapayaWhip, PeachPuff, Peru, Pink, Plum, PowderBlue, Purple, WebPurple, RebeccaPurple, Red, RosyBrown, RoyalBlue, SaddleBrown, Salmon,
        SandyBrown, SeaGreen, Seashell, Sienna, Silver, SkyBlue, SlateBlue, SlateGray, Snow, SpringGreen, SteelBlue, Tan, Teal, Thistle, Tomato,
        Turquoise, Violet, Wheat, White, WhiteSmoke, Yellow, YellowGreen
    };

    public interface INamedColourDetail
    {
        string Colour { get; set; }
        string RGB { get; set; }
        string GamutA { get; set; }
        string GamutB { get; set; }
        string GamutC { get; set; }
    }

    public interface IColourQuery
    {
        IEnumerable<INamedColourDetail> GetNamedColourDetails();
        INamedColourDetail GetNamedColourDetail(NamedColour namedColour);
        NamedColour GetNamedColourFromDetail(INamedColourDetail namedColourDetail);
    }
}
