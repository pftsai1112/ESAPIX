﻿#region

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VMS.TPS.Common.Model.Types;

#endregion

namespace ESAPIX.Facade.Serialization
{
    public class DoseValueConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DoseValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // Load the JSON for the Result into a JObject
            var jo = JObject.Load(reader);

            // Read the properties which will be used as constructor parameters
            var value = (double) jo["Dose"];
            var unitVal = jo["Unit"].Value<string>();
            DoseValue.DoseUnit unit = DoseValue.DoseUnit.Unknown;
            switch (unitVal)
            {
                case "%": unit = DoseValue.DoseUnit.Percent;break;
                case "cGy": unit = DoseValue.DoseUnit.cGy;break;
                case "Gy": unit = DoseValue.DoseUnit.Gy;break;
            }
        
            // Construct the Result object using the non-default constructor
            var dv = new DoseValue(value, unit);

            // Return the result
            return dv;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}