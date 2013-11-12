using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class GlobalConst
    {
        public static int[] splitFilterString(string filterString)
        {
            int[] newsArticleTypeFilters = filterString.Split(new char[] { ',' }).Select(f => f.ToIntegerOrDefault(-1)).ToArray();
            return newsArticleTypeFilters;
        }
    }

    public enum ImageTypes { Custom = 0, General = 1, Blizzard = 2, Class_Barbarian = 3, Class_Sorcerer = 4, Class_WitchDoctor = 5, Class_Monk = 6, Class_DemonHunter = 7, Admin = 8, Lore = 9, Twitter = 10, PVP = 11, Facebook = 12, RSS_Community = 13, RSS_DiabloFans = 14, RSS_IncGamers = 15, Blizzcon = 16, Beta = 17 }
    public enum ArticleTypes { General = 0, Blizzard = 1, Class_Barbarian = 2, Class_Sorcerer = 3, Class_WitchDoctor = 4, Class_Monk = 5, Class_DemonHunter = 6, Admin = 7, Lore = 8, Twitter = 10, PVP = 11, Facebook = 12, RSS_Community = 13, RSS_DiabloFans = 14, RSS_IncGamers = 15, Blizzcon = 16, Beta = 17 }
}
