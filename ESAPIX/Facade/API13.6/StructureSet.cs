using System;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Dynamic;
using ESAPIX.Extensions;
using VMS.TPS.Common.Model.Types;
using XC = ESAPIX.Facade.XContext;
using Types = VMS.TPS.Common.Model.Types;

namespace ESAPIX.Facade.API
{
    public class StructureSet : ESAPIX.Facade.API.ApiDataObject, System.Xml.Serialization.IXmlSerializable
    {
        public IEnumerable<ESAPIX.Facade.API.Structure> Structures
        {
            get
            {
                if (_client is ExpandoObject)
                {
                    if ((_client as ExpandoObject).HasProperty("Structures"))
                    {
                        foreach (var item in _client.Structures)
                        {
                            yield return item;
                        }
                    }
                    else
                    {
                        yield break;
                    }
                }
                else
                {
                    IEnumerator enumerator = null;
                    XC.Instance.CurrentContext.Thread.Invoke(() =>
                    {
                        var asEnum = (IEnumerable)_client.Structures;
                        if ((asEnum) != null)
                        {
                            enumerator = asEnum.GetEnumerator();
                        }
                    }

                    );
                    if (enumerator == null)
                    {
                        yield break;
                    }

                    while (XC.Instance.CurrentContext.GetValue<bool>(sc => enumerator.MoveNext()))
                    {
                        var facade = new ESAPIX.Facade.API.Structure();
                        XC.Instance.CurrentContext.Thread.Invoke(() =>
                        {
                            var vms = enumerator.Current;
                            if (vms != null)
                            {
                                facade._client = vms;
                            }
                        }

                        );
                        if (facade._client != null)
                        {
                            yield return facade;
                        }
                    }
                }
            }

            set
            {
                if (_client is ExpandoObject)
                    _client.Structures = value;
            }
        }

        public ESAPIX.Facade.API.Image Image
        {
            get
            {
                if ((_client) is System.Dynamic.ExpandoObject)
                {
                    if (((ExpandoObject)(_client)).HasProperty("Image"))
                    {
                        return _client.Image;
                    }
                    else
                    {
                        return default (ESAPIX.Facade.API.Image);
                    }
                }
                else if ((XC.Instance.CurrentContext) != (null))
                {
                    return XC.Instance.CurrentContext.GetValue(sc =>
                    {
                        if ((_client.Image) != (null))
                        {
                            return new ESAPIX.Facade.API.Image(_client.Image);
                        }
                        else
                        {
                            return null;
                        }
                    }

                    );
                }
                else
                {
                    return default (ESAPIX.Facade.API.Image);
                }
            }

            set
            {
                if ((_client) is System.Dynamic.ExpandoObject)
                {
                    _client.Image = (value);
                }
                else
                {
                }
            }
        }

        public System.String UID
        {
            get
            {
                if ((_client) is System.Dynamic.ExpandoObject)
                {
                    if (((ExpandoObject)(_client)).HasProperty("UID"))
                    {
                        return _client.UID;
                    }
                    else
                    {
                        return default (System.String);
                    }
                }
                else if ((XC.Instance.CurrentContext) != (null))
                {
                    return XC.Instance.CurrentContext.GetValue(sc =>
                    {
                        return _client.UID;
                    }

                    );
                }
                else
                {
                    return default (System.String);
                }
            }

            set
            {
                if ((_client) is System.Dynamic.ExpandoObject)
                {
                    _client.UID = (value);
                }
                else
                {
                }
            }
        }

        public ESAPIX.Facade.API.Structure AddStructure(System.String dicomType, System.String id)
        {
            if ((XC.Instance.CurrentContext) != (null))
            {
                var vmsResult = (XC.Instance.CurrentContext.GetValue(sc =>
                {
                    var fromClient = (_client.AddStructure(dicomType, id));
                    if ((fromClient) == (null))
                    {
                        return null;
                    }

                    return new ESAPIX.Facade.API.Structure(fromClient);
                }

                ));
                return vmsResult;
            }
            else
            {
                return (ESAPIX.Facade.API.Structure)(_client.AddStructure(dicomType, id));
            }
        }

        public System.Boolean CanAddStructure(System.String dicomType, System.String id)
        {
            if ((XC.Instance.CurrentContext) != (null))
            {
                var vmsResult = (XC.Instance.CurrentContext.GetValue(sc =>
                {
                    var fromClient = (_client.CanAddStructure(dicomType, id));
                    if (fromClient.Equals(default (System.Boolean)))
                    {
                        return default (System.Boolean);
                    }

                    return (System.Boolean)(fromClient);
                }

                ));
                return vmsResult;
            }
            else
            {
                return (System.Boolean)(_client.CanAddStructure(dicomType, id));
            }
        }

        public System.Boolean CanRemoveStructure(ESAPIX.Facade.API.Structure structure)
        {
            if ((XC.Instance.CurrentContext) != (null))
            {
                var vmsResult = (XC.Instance.CurrentContext.GetValue(sc =>
                {
                    var fromClient = (_client.CanRemoveStructure(structure._client));
                    if (fromClient.Equals(default (System.Boolean)))
                    {
                        return default (System.Boolean);
                    }

                    return (System.Boolean)(fromClient);
                }

                ));
                return vmsResult;
            }
            else
            {
                return (System.Boolean)(_client.CanRemoveStructure(structure));
            }
        }

        public void RemoveStructure(ESAPIX.Facade.API.Structure structure)
        {
            if ((XC.Instance.CurrentContext) != (null))
            {
                XC.Instance.CurrentContext.Thread.Invoke(() =>
                {
                    _client.RemoveStructure(structure._client);
                }

                );
            }
            else
            {
                _client.RemoveStructure(structure);
            }
        }

        public StructureSet()
        {
            _client = (new ExpandoObject());
        }

        public StructureSet(dynamic client)
        {
            _client = (client);
        }
    }
}