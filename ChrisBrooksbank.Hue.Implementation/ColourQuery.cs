using ChrisBrooksbank.Hue.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;

namespace ChrisBrooksbank.Hue.Implementation
{
    public class ColourQuery: IColourQuery
    {
        public INamedColourDetail GetNamedColourDetail(NamedColour namedColour)
        {
            INamedColourDetail namedColourDetail = null;

            IEnumerable<INamedColourDetail> namedColourEnumerator = this.GetNamedColourDetails();

            foreach (INamedColourDetail namedColourDetailCandidate in namedColourEnumerator)
            {
                NamedColour namedColourInDetail = this.GetNamedColourFromDetail(namedColourDetailCandidate);
                if (namedColourInDetail == namedColour)
                {
                    namedColourDetail = namedColourDetailCandidate;
                }
            }

            return namedColourDetail;
        }

        public IEnumerable<INamedColourDetail> GetNamedColourDetails()
        {
            ObjectCache cache = MemoryCache.Default;
            List<NamedColourDetail> namedColours =  cache["NamedColourDetailList"] as List<NamedColourDetail>;
            if ( namedColours == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddMinutes(20)); // TODO read cache timeout from config

                string namedColourFileName = "NamedColours.json";

                if (!File.Exists(namedColourFileName))
                {
                    throw new ApplicationException("missing named colours file " + namedColourFileName);
                }

                string namedColoursJSON = File.ReadAllText(namedColourFileName);

                namedColours = JsonConvert.DeserializeObject<List<NamedColourDetail>>(namedColoursJSON);

                cache.Set("NamedColourDetailList", namedColours, policy);
            }

            return namedColours;
        }

        public NamedColour GetNamedColourFromDetail(INamedColourDetail namedColourDetail )
        {
            NamedColour namedColour;

            Enum.TryParse<NamedColour>(namedColourDetail.Colour, out namedColour);
            return namedColour;
        }
    }
}
