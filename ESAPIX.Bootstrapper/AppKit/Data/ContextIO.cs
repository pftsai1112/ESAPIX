﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESAPIX.Common;
using ESAPIX.Extensions;
using ESAPIX.Helpers.IO;
using ESAPIX.Interfaces;
using Microsoft.Win32;

namespace ESAPIX.Bootstrapper.AppKit.Data
{
    public class ContextIO
    {
        public static void SaveToFile(StandAloneContext app)
        {
            var contextString = ArgumentBuilder.Build(app);
            var dlg = new SaveFileDialog();
            dlg.FileName = "context.txt"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension

            // Show open file dialog box
            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                var filename = dlg.FileName;
                File.WriteAllText(filename, contextString);
            }
        }

        public static string[] ReadArgsFromFile(string filePath)
        {
            return File.ReadAllText(filePath).SplitOnWhiteSpace();
        }
    }

    public class ArgumentBuilder
    {
        public static string Build(IScriptContext ctx)
        {
            var sb = new StringBuilder();

            if (ctx.CurrentUser != null)
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.CurrentUserId, ctx.CurrentUser.Id));
            }
#if !VMS110
            if (!string.IsNullOrEmpty(ctx.ApplicationName))
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.ApplicationName, ctx.ApplicationName));
            }
            if (ctx.BrachyPlansInScope != null && ctx.BrachyPlansInScope.Any())
            {
                var uids = ctx.BrachyPlansInScope.Select(p => p.UID).ToArray();
                sb.Append(string.Format("{0} {1} ", ArgumentKey.BrachyPlansInScope, string.Join(" ", uids)));
            }
            if (ctx.ExternalPlansInScope != null && ctx.ExternalPlansInScope.Any())
            {
                var uids = ctx.ExternalPlansInScope.Select(p => p.UID).ToArray();
                sb.Append(string.Format("{0} {1} ", ArgumentKey.ExternalPlansInScope, string.Join(" ", uids)));
            }
            if (ctx.BrachyPlanSetup != null)
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.BrachyPlanSetup, ctx.BrachyPlanSetup.UID));
            }
            if (ctx.ExternalPlanSetup != null)
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.ExternalPlanSetup, ctx.ExternalPlanSetup.UID));
            }
#endif
            if (ctx.Patient != null && !string.IsNullOrEmpty(ctx.Patient.Id))
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.PatientId, ctx.Patient.Id));
            }
            if (ctx.Image != null)
            {
                sb.Append(string.Format("{0} {1} {2} ", ArgumentKey.Image, ctx.Image.Id, ctx.Image.Series.UID));
            }
            if (ctx.StructureSet != null)
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.StructureSet, ctx.StructureSet.UID));
            }
            if (ctx.PlanSetup != null)
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.PlanSetup, ctx.PlanSetup.UID));
            }

            if (ctx.Course != null)
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.Course, ctx.Course.Id));
            }
            if (ctx.PlansInScope != null && ctx.PlansInScope.Any())
            {
                var uids = ctx.PlansInScope.Select(p => p.UID).ToArray();
                sb.Append(string.Format("{0} {1} ", ArgumentKey.PlansInScope, string.Join(" ", uids)));
            }

            if (ctx.PlanSumsInScope != null && ctx.PlanSumsInScope.Any())
            {
                var ids = ctx.PlanSumsInScope.Select(p => p.Id).ToArray();
                sb.Append(string.Format("{0} {1} ", ArgumentKey.PlanSumsInScope, string.Join(" ", ids)));
            }
#if (VMS151 || VMS150 || VMS155)
            if (ctx.IonPlansInScope != null && ctx.IonPlansInScope.Any())
            {
                var ids = ctx.IonPlansInScope.Select(p => p.Id).ToArray();
                sb.Append(string.Format("{0} {1} ", ArgumentKey.IonPlansInScope, string.Join(" ", ids)));
            }
            if (ctx.IonPlanSetup != null)
            {
                sb.Append(string.Format("{0} {1} ", ArgumentKey.IonPlanSetup, ctx.IonPlanSetup.UID));
            }
#endif
            return sb.ToString();
        }
    }

    /// <summary>
    /// The prefix key to the cmd line arguments for ESAPIX to parse
    /// </summary>
    public class ArgumentKey
    {
        public const string CurrentUserId = "-u";
        public const string Course = "-c";
        public const string Image = "-i";
        public const string StructureSet = "-ss";
        public const string ApplicationName = "-a";
        public const string PatientId = "-id";
        public const string PlanSetup = "-p";
        public const string BrachyPlanSetup = "-bp";
        public const string BrachyPlansInScope = "-bpsc";
        public const string ExternalPlanSetup = "-ep";
        public const string ExternalPlansInScope = "-epsc";
        public const string PlansInScope = "-psc";
        public const string PlanSumsInScope = "-pssc";
        public const string IonPlansInScope = "-ipsc";
        public const string IonPlanSetup = "-ip";
    }

}
