using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DesinationProperties { get; private set; }

        public bool Revert { get; set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DesinationProperties = destinationProperties;
            Revert = revert;
        }
    }
}
