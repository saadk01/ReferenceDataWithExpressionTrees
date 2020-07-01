using System;

namespace DomainLayer
{
    public class ReferenceData : IReferenceData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveTillDate { get; set; }
    }

    public class Province : ReferenceData { }

    public class Season : ReferenceData { }
}
