using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace MoreBandits
{
	internal static class HarmonyPatches
	{
		private static bool _initialized = false;
		public static void Initialize()
		{
			if (_initialized)
				return;
			_initialized = true;

			try
			{
				var method = typeof(MobileParty).GetMethod("FillPartyStacks", BindingFlags.NonPublic | BindingFlags.Instance);
				var patches = Harmony.GetPatchInfo(method);
				if (patches?.Transpilers?.Count > 0)
				{
					string output = "";
					foreach (var patch in patches.Transpilers)
						output += patch.ToString();
					FileLog.Log("MoreBandits: Warning: transpiler patches detected: " + output);
				}

				var harmony = new Harmony("com.sy.morebandits");

				// Patch MobileParty.FillPartyStacks
				harmony.Patch(method, transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.Transpiler_MobileParty_FillPartyStacks)));

				// Disable NoBanditsNoCry.FillPartyStacks patch
				method = Type.GetType("NoBanditsNoCry.Patches.FillPartyStacks, NoBanditsNoCry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")?.GetMethod("Prefix", BindingFlags.Public | BindingFlags.Static);
				if (method != null)
					harmony.Patch(method, transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.Transpiler_NoBanditsNoCry_FillPartyStacks)));
			}
			catch (Exception e)
			{
				FileLog.Log(e.ToString());
			}
		}

		private static IEnumerable<CodeInstruction> Transpiler_MobileParty_FillPartyStacks(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			//FileLog.Log("");

			bool applied = false;
			LocalBuilder lb_f = null, lb_f2 = null, lb_f3 = null;
			var list = new List<CodeInstruction>(instructions);
			for (int i = 0; i < list.Count - 2; i++)
			{
				if (list[i].opcode == OpCodes.Stloc_S
					&& list[i].operand is LocalBuilder lb
					&& lb.LocalType == typeof(float))
				{
					if (lb.LocalIndex == 7)
						lb_f = lb;
					else if (lb.LocalIndex == 8)
						lb_f2 = lb;
					else if (lb.LocalIndex == 9)
					{
						lb_f3 = lb;

						if (lb_f != null && lb_f2 != null && lb_f3 != null)
						{
							// insert after "Stloc_S 9"
							list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
							list.Insert(++i, new CodeInstruction(OpCodes.Ldloca_S, lb_f));  // 7
							list.Insert(++i, new CodeInstruction(OpCodes.Ldloca_S, lb_f2)); // 8
							list.Insert(++i, new CodeInstruction(OpCodes.Ldloca_S, lb_f3)); // 9
							list.Insert(++i, new CodeInstruction(OpCodes.Call, typeof(Main).GetMethod(nameof(Main.ModifyVariables), BindingFlags.Static | BindingFlags.Public)));
							applied = true;
							break;
						}
					}
				}
			}

			//foreach (var instruction in list)
			//	FileLog.Log(instruction.ToString());
			if (!applied)
				throw new Exception("MoreBandits: failed to apply Harmony-patch 'Transpiler_MobileParty_FillPartyStacks'");
			return list;
		}

		private static IEnumerable<CodeInstruction> Transpiler_NoBanditsNoCry_FillPartyStacks(IEnumerable<CodeInstruction> instructions)
		{
			yield return new CodeInstruction(OpCodes.Ldc_I4_1);
			yield return new CodeInstruction(OpCodes.Ret);
		}
	}
}
