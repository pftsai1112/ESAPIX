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
    public class ScriptEnvironment
    {
        internal dynamic _client;
        public bool IsLive
        {
            get
            {
                return !DefaultHelper.IsDefault(_client) && !(_client is ExpandoObject);
            }
        }

        public System.String ApplicationName
        {
            get
            {
                if ((_client) is System.Dynamic.ExpandoObject)
                {
                    if (((ExpandoObject)(_client)).HasProperty("ApplicationName"))
                    {
                        return _client.ApplicationName;
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
                        return _client.ApplicationName;
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
                    _client.ApplicationName = (value);
                }
                else
                {
                }
            }
        }

        public System.String VersionInfo
        {
            get
            {
                if ((_client) is System.Dynamic.ExpandoObject)
                {
                    if (((ExpandoObject)(_client)).HasProperty("VersionInfo"))
                    {
                        return _client.VersionInfo;
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
                        return _client.VersionInfo;
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
                    _client.VersionInfo = (value);
                }
                else
                {
                }
            }
        }

        public System.String ApiVersionInfo
        {
            get
            {
                if ((_client) is System.Dynamic.ExpandoObject)
                {
                    if (((ExpandoObject)(_client)).HasProperty("ApiVersionInfo"))
                    {
                        return _client.ApiVersionInfo;
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
                        return _client.ApiVersionInfo;
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
                    _client.ApiVersionInfo = (value);
                }
                else
                {
                }
            }
        }

        public IEnumerable<ESAPIX.Facade.API.ApplicationScript> Scripts
        {
            get
            {
                if (_client is ExpandoObject)
                {
                    if ((_client as ExpandoObject).HasProperty("Scripts"))
                    {
                        foreach (var item in _client.Scripts)
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
                        var asEnum = (IEnumerable)_client.Scripts;
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
                        var facade = new ESAPIX.Facade.API.ApplicationScript();
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
                    _client.Scripts = value;
            }
        }

        public void ExecuteScript(System.Reflection.Assembly scriptAssembly, ESAPIX.Facade.API.ScriptContext scriptContext, System.Windows.Window window)
        {
            if ((XC.Instance.CurrentContext) != (null))
            {
                XC.Instance.CurrentContext.Thread.Invoke(() =>
                {
                    _client.ExecuteScript(scriptAssembly, scriptContext._client, window);
                }

                );
            }
            else
            {
                _client.ExecuteScript(scriptAssembly, scriptContext, window);
            }
        }

        public ScriptEnvironment()
        {
            _client = (new ExpandoObject());
        }

        public ScriptEnvironment(dynamic client)
        {
            _client = (client);
        }

        //public ScriptEnvironment(System.String appName, System.Collections.Generic.IEnumerable<VMS.TPS.Common.Model.IApplicationScript> scripts, System.Action<System.Reflection.Assembly,System.Object,System.Windows.Window,System.Object> scriptExecutionEngine)
        //{
        //    if ((XC.Instance.CurrentContext) != (null))
        //    {
        //        _client = (VMSConstructor.ConstructScriptEnvironmentFunc0(appName, scripts, scriptExecutionEngine));
        //    }
        //    else
        //    {
        //        throw new Exception("There is no VMS Context to create the class");
        //    }
        //}
    }
}