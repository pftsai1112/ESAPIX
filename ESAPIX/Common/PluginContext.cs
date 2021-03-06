﻿#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ESAPIX.Facade.API;
using ESAPIX.Interfaces;
using ESAPIX.Logging;

#endregion

namespace ESAPIX.Common
{
    /// <summary>
    ///     A simple wrapper around the script context to make it compliant with a common interface IScriptContext
    /// </summary>
    public class PluginContext : IScriptContext
    {
        private readonly ScriptContext _ctx;
        private readonly WeakReference _scriptReference;

        public PluginContext(ScriptContext ctx, Window w)
        {
            _ctx = ctx;
            _scriptReference = new WeakReference(ctx._client);
            Thread = new ScriptComThread(w.Dispatcher);
            Logger = new Logger();
        }

#if !VMS110
        public string ApplicationName => _ctx?.ApplicationName;

        public BrachyPlanSetup BrachyPlanSetup => _ctx?.BrachyPlanSetup;

        public IEnumerable<BrachyPlanSetup> BrachyPlansInScope => _ctx?.BrachyPlansInScope;

        public ExternalPlanSetup ExternalPlanSetup => _ctx?.ExternalPlanSetup;

        public IEnumerable<ExternalPlanSetup> ExternalPlansInScope => _ctx?.ExternalPlansInScope;
#endif

        public Course Course => _ctx?.Course;

        public User CurrentUser => _ctx?.CurrentUser;

        public Image Image => _ctx?.Image;

        public Patient Patient => _ctx?.Patient;

        public PlanSetup PlanSetup => _ctx?.PlanSetup;

        public IEnumerable<PlanSetup> PlansInScope => _ctx?.PlansInScope;

        public IEnumerable<PlanSum> PlanSumsInScope => _ctx?.PlanSumsInScope;

        public StructureSet StructureSet => _ctx?.StructureSet;

#if (VMS150 || VMS151 || VMS155)
        public IonPlanSetup IonPlanSetup => _ctx?.IonPlanSetup;
        public IEnumerable<IonPlanSetup> IonPlansInScope => _ctx?.IonPlansInScope;
        //NOT USED BUT REQUIRED FOR INTERFACE
        public event StandAloneContext.IonPlanSetupChangedHandler IonPlanSetupChanged;
#endif

        public bool IsDisposed
        {
            get { return _scriptReference.IsAlive; }
        }

        public IVMSThread Thread { get; }

        public async Task<T> GetValueAsync<T>(Func<IScriptContext, T> toExecute)
        {
            var result = default(T);
            await Thread.InvokeAsync(() => { result = toExecute(this); });
            return result;
        }

        public T GetValue<T>(Func<IScriptContext, T> toExecute)
        {
            var result = default(T);
            Thread.Invoke(() => { result = toExecute(this); });
            return result;
        }

        public Dispatcher UIDispatcher { get; set; }

        public Logger Logger { get; private set; }

#region NOT USED

        //These will never get called
        public event StandAloneContext.PatientChangedHandler PatientChanged;
        public event StandAloneContext.PlanSetupChangedHandler PlanSetupChanged;
#if !VMS110
        public event StandAloneContext.ExternalPlanSetupChangedHandler ExternalPlanSetupChanged;
        public event StandAloneContext.BrachyPlanSetupChangedHandler BrachyPlanSetupChanged;
#endif
        public event StandAloneContext.CourseChangedHandler CourseChanged;

#endregion
    }
}