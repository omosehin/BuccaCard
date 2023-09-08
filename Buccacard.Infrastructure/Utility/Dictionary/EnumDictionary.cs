using Buccacard.Infrastructure.Utility;
using HermesApp.Infrastructure.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Buccacard.Infrastructure.Dictionary
{
    public static class EnumDictionary
    {
        public static List<EnumList> GetList<T>() where T : struct
        {
            var returnList = (from int e in Enum.GetValues(typeof(T))
                              select new EnumList
                              {
                                  ItemId = e,
                                  ItemName = ((Enum)Enum.Parse(typeof(T), e.ToString())).DisplayName()
                              }).OrderBy(x => x.ItemName).ToList();

            return returnList;
        }
    }

    public class EnumList
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
