using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Party;

namespace MoreBandits
{
	public class MCMSettings : AttributeGlobalSettings<MCMSettings>
	{
		public override string Id => "MoreBandits";

		public override string DisplayName => "More Bandits";

		public override string FolderName => "MoreBandits";

		public override string FormatType => "json";


		[SettingPropertyFloatingInteger(
			"Bandit Party Size Multiplier", 
			0.01f, 
			10.0f, 
			"#0%", 
			RequireRestart = false,
			HintText = "Adjusts the size of all bandit parties. Only newly spawned parties are affected. High values decrease performance and may have negative influence on game experience. [Native: 100%]", 
			Order = 1)]
		[SettingPropertyGroup(
			"Bandit Party Size Multipliers", 
			GroupOrder = 0)]
		public float BanditMultiplier { get; set; } = 1f;


		public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
		{
			IDictionary<string, Func<BaseSettings>> availablePresets = base.GetAvailablePresets();
			availablePresets.Add("Native", () => new MCMSettings
			{
				BanditMultiplier = 1f,
			});
			return availablePresets;
		}
	}
}
