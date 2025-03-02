﻿using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;

using Mono.Debugging.Soft;
using Mono.Debugging.Client;
using Mono.Debugging.VisualStudio;
using Meadow.CLI.Core.Devices;

namespace Meadow
{
    using DebuggerSession = Mono.Debugging.VisualStudio.DebuggerSession;

    [Export(typeof(IDebugProfileLaunchTargetsProvider))]
    [AppliesTo(Globals.MeadowCapability)]
    [Order(999)]
    public class MeadowDebuggerLaunchProvider : IDebugProfileLaunchTargetsProvider, IDebugLauncher
    {
        static readonly DebuggingOptions debuggingOptions = new DebuggingOptions
        {
            EvaluationTimeout = 10000,
            MemberEvaluationTimeout = 15000,
            ModificationTimeout = 10000,
            SocketTimeout = 0
        };

        VsOutputPaneLogger outputPane;

        // FIXME: Find a nicer way than storing this
        DebuggerSession vsSession;

        // https://github.com/microsoft/VSProjectSystem/blob/master/doc/overview/mef.md
        [ImportMany(ExportContractNames.VsTypes.IVsHierarchy)]
        OrderPrecedenceImportCollection<IVsHierarchy> vsHierarchies;

        IVsHierarchy VsHierarchy => vsHierarchies.Single().Value;

        public bool SupportsProfile(ILaunchProfile profile) => true; // FIXME: Would we ever not?

        [ImportingConstructor]
        public MeadowDebuggerLaunchProvider(ConfiguredProject configuredProj)
        {
            vsHierarchies = new OrderPrecedenceImportCollection<IVsHierarchy>(
                projectCapabilityCheckProvider: configuredProj);
            outputPane = new VsOutputPaneLogger(configuredProj.UnconfiguredProject.ProjectService.Services.ThreadingPolicy);
        }

        public async Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions launchOptions, ILaunchProfile profile)
        {
            if (launchOptions.HasFlag(DebugLaunchOptions.NoDebug))
            {
                vsSession = null;
                return Array.Empty<IDebugLaunchSettings>();
            }

            DeployProvider.Meadow?.Dispose();

            var device = await MeadowProvider.GetMeadowSerialDeviceAsync(logger: outputPane);

            if (device != null)
            {
                DeployProvider.Meadow = new MeadowDeviceHelper(device, outputPane);

                var meadowSession = new MeadowSoftDebuggerSession(DeployProvider.Meadow);

                var startArgs = new SoftDebuggerConnectArgs(profile.Name, IPAddress.Loopback, 55898);
                var startInfo = new StartInfo(startArgs, debuggingOptions, VsHierarchy.GetProject());

                var sessionInfo = new SessionMarshalling(meadowSession, startInfo);
                vsSession = new DebuggerSession(startInfo, outputPane, meadowSession, this);

                var settings = new MonoDebugLaunchSettings(launchOptions, sessionInfo);

                return new[] { settings };
            }
            else
			{
                vsSession = null;
                return Array.Empty<IDebugLaunchSettings>();
            }
        }

        public Task OnBeforeLaunchAsync(DebugLaunchOptions launchOptions, ILaunchProfile profile) => Task.CompletedTask;

        public Task OnAfterLaunchAsync(DebugLaunchOptions launchOptions, ILaunchProfile profile)
        {
            vsSession?.Start();
            return Task.CompletedTask;
        }

        bool IDebugLauncher.StartDebugger(SoftDebuggerSession session, StartInfo startInfo)
        {
            // nop here because VS is responseable for starting us and then calling On*LaunchAsync above
            return true;
        }
    }
}
