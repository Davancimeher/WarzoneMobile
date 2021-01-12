using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statics
{
    public static Dictionary<CloudRegionCode, string> regionsDict = new Dictionary<CloudRegionCode, string>()
    {
        { CloudRegionCode.asia,"Asia" },
        { CloudRegionCode.au,"Australia" },
        { CloudRegionCode.cae,"Canada" },
        { CloudRegionCode.eu,"Europe" },
        { CloudRegionCode.@in,"India" },
        { CloudRegionCode.jp,"Japan" },
        { CloudRegionCode.ru,"Russia" },
        { CloudRegionCode.sa,"South America"},
        { CloudRegionCode.kr,"South Korea" },
        { CloudRegionCode.us,"USA" }
    };
}
