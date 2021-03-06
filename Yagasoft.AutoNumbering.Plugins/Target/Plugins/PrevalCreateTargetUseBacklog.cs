﻿// this file was generated by the xRM Test Framework VS Extension

#region Imports

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Yagasoft.Libraries.Common;
using Microsoft.Xrm.Sdk;

#endregion

namespace Yagasoft.AutoNumbering.Plugins.Target.Plugins
{
	/// <summary>
	///     This plugin ... .<br />
	///     Version: 0.1.1
	/// </summary>
	public class PrevalCreateTargetUseBacklog : IPlugin
	{
		private readonly string config;

		public PrevalCreateTargetUseBacklog(string unsecureConfig, string secureConfig)
		{
			if (string.IsNullOrEmpty(unsecureConfig))
			{
				throw new InvalidPluginExecutionException(
					"Plugin config is empty. Please enter the config record ID or inline config first.");
			}

			config = unsecureConfig;
		}

		public void Execute(IServiceProvider serviceProvider)
		{
			////var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
			new PrevalCreateTargetUseBacklogLogic(config).Execute(this, serviceProvider, PluginUser.System);
		}
	}

	internal class PrevalCreateTargetUseBacklogLogic : PluginLogic<PrevalCreateTargetUseBacklog>
	{
		private readonly string config;

		public PrevalCreateTargetUseBacklogLogic(string unsecureConfig) : base("Create", PluginStage.PreValidation)
		{
			config = unsecureConfig;
		}

		protected override void ExecuteLogic()
		{
			// get the triggering record
			var target = (Entity)context.InputParameters["Target"];

			if (log.MaxLogLevel >= LogLevel.Debug)
			{
				Libraries.Common.CrmHelpers.LogAttributeValues(target.Attributes, target, log, "Target Attributes");
			}

			var autoNumberConfig =
				(from autoNumberQ in new XrmServiceContext(service).AutoNumberingSet
				 where autoNumberQ.UniqueID == config
					 && autoNumberQ.Status == AutoNumbering.StatusEnum.Active
				 select new AutoNumbering
						{
							Id = autoNumberQ.Id,
							Name = autoNumberQ.Name,
							FormatString = autoNumberQ.FormatString,
							UseBacklog = autoNumberQ.UseBacklog
						}).FirstOrDefault();

			if (autoNumberConfig == null)
			{
				throw new InvalidPluginExecutionException($"Couldn't find an active auto-numbering configuration"
					+ $" with ID '{config}'.");
			}

			if (autoNumberConfig.FormatString.Contains("{!index!}") && autoNumberConfig.UseBacklog == true)
			{
				var triggerId = Guid.NewGuid().ToString();

				log.Log($"Updating config with trigger ID '{triggerId}' ...");
				service.Update(
					new AutoNumbering
					{
						Id = autoNumberConfig.Id,
						TriggerID = triggerId
					});
				log.Log($"Finished updating config with trigger ID.");

				context.SharedVariables["AutoNumberingTriggerId"] = triggerId;
				log.Log($"Added trigger ID to shared variables.");
			}
		}
	}
}
