﻿#region

using System.Collections.Generic;
using System.IO;
using ESAPIX.Interfaces;
using Newtonsoft.Json;
using ESAPIX.Common;
using ESAPIX.Extensions;
using Newtonsoft.Json.Serialization;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using ESAPIX.Facade.API;

#endregion

namespace ESAPIX.Facade.Serialization
{
    public static class FacadeSerializer
    {
        public static JsonSerializerSettings DeserializeSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None,
                    Converters = new List<JsonConverter>
                    {
                        new IEnumerableJsonConverter(),
                        new MeshGeometryConverter(),
                        new ProfilePointConverter(),
                        new DoseProfileConverter(),
                        new DVHPointConverter(),
#if !VMS110
                        new StructureCodeInfoConverter(),
#endif
                        new DoseValueConverter(),
                        new ControlPointCollectionConverter(),
#if VMS155
                     
#endif
                    },
                    ContractResolver = new ESAPIContractResolver()
                };
            }
        }

        public static JsonSerializerSettings SerializeSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None,
                    ContractResolver = new ESAPIContractResolver(),
                    Converters = new List<JsonConverter> { new DoseProfileConverter() }
                };
            }
        }

        /// <summary>
        /// Serialize to JSON string
        /// </summary>
        /// <param name="o">object to be serialized</param>
        /// <returns>json string of object</returns>
        public static string Serialize(IXmlSerializable apiObject)
        {
            var xml = SerializeToXML(apiObject);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            //Convert to JSON
            var json = JsonConvert.SerializeXmlNode(doc.FirstChild, Newtonsoft.Json.Formatting.None, true);
            json = json.Replace("@", "");
            return json;
        }

        /// <summary>
        /// Serialize to XML string
        /// </summary>
        /// <param name="o">object to be serialized</param>
        /// <returns>json string of object</returns>
        public static string SerializeToXML(IXmlSerializable apiObject)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.CloseOutput = false;
            var ms = new StringBuilder();
            {
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {

                    //Write to XML first (built into ESAPI)
                    var @class = apiObject.GetType().FullName;
                    writer.WriteStartElement(@class);
                    apiObject.WriteXml(writer);
                    writer.WriteFullEndElement();
                    writer.Close();
                    var xml = ms.ToString();
                    return xml;
                }
            }
        }
        /// <summary>
        /// Serialize to JSON string
        /// </summary>
        /// <param name="o">object to be serialized</param>
        /// <returns>json string of object</returns>
        public static string SerializeStructureWithGeometry(Structure st)
        {
            var mesh = st.MeshGeometry;
            var meshJson = JsonConvert.SerializeObject(mesh, SerializeSettings);
            var jsonNonGeometry = Serialize(st);
            var recovered = JsonConvert.DeserializeObject<Structure>(jsonNonGeometry, DeserializeSettings);
            recovered.MeshGeometry = mesh;
            return JsonConvert.SerializeObject(recovered, SerializeSettings);
        }

        /// <summary>
        /// Serialize to JSON string
        /// </summary>
        /// <param name="o">object to be serialized</param>
        /// <returns>json string of object</returns>
        public static string Serialize(object o)
        {
            var json = JsonConvert.SerializeObject(o, SerializeSettings);
            return json;
        }

        /// <summary>
        /// Serialize to text file (json)
        /// </summary>
        /// <param name="o">object to be serialized</param>
        /// <param name="jsonPath">file path to save object file</param>
        public static void SerializeToFile(object o, string jsonPath)
        {
            var json = Serialize(o);
            File.WriteAllText(jsonPath, json);
        }

        /// <summary>
        /// Deserialize from JSON string
        /// </summary>
        /// <typeparam name="T">the type of object to be returned</typeparam>
        /// <param name="json">json string of object</param>
        /// <returns>object</returns>
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, DeserializeSettings);
        }

        /// <summary>
        /// Deserialize from JSON string
        /// </summary>
        /// <typeparam name="T">the type of object to be returned</typeparam>
        /// <param name="jsonPath">file path to save object file</param>
        /// <returns>object</returns>
        public static T DeserializeFromFile<T>(string jsonPath)
        {
            var json = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<T>(json, DeserializeSettings);
        }

        public static void SerializeContext(IScriptContext ctx, string jsonPath)
        {
            SerializeToFile(ctx, jsonPath);
        }

        public static OfflineContext DeserializeContext(string jsonPath)
        {
            var ctx = DeserializeFromFile<OfflineContext>(jsonPath);
#if !VMS110
            //BRACHY
            if (ctx.BrachyPlanSetup != null) ctx.BrachyPlanSetup.Course = ctx.Course;
            if (ctx.BrachyPlansInScope != null)
                foreach (var ps in ctx.BrachyPlansInScope)
                    ps.Course = ctx.Course;
            //EXTERNAL PLAN SETUPS
            if (ctx.ExternalPlanSetup != null) ctx.ExternalPlanSetup.Course = ctx.Course;
            if (ctx.ExternalPlansInScope != null)
                foreach (var ps in ctx.PlansInScope)
                    ps.Course = ctx.Course;
#endif
            //PLAN SETUPS
            if (ctx.PlanSetup != null) ctx.PlanSetup.Course = ctx.Course;
            if (ctx.PlansInScope != null)
                foreach (var ps in ctx.PlansInScope)
                    ps.Course = ctx.Course;


            //PLAN SUMS
#if !VMS110
            if (ctx.PlanSumsInScope != null)
                foreach (var ps in ctx.PlanSumsInScope)
                    ps.Course = ctx.Course;
#endif
            return ctx;
        }

    }
}