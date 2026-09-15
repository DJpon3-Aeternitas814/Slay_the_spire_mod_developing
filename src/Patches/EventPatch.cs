using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;         // ModelDb
using MegaCrit.Sts2.Core.Models.Acts;    // Overgrowth
using MegaCrit.Sts2.Core.Models.Events;  // EventModel
using sts2mod.Events;                    // 你的 TestEvent

namespace sts2mod.Patches;

[HarmonyPatch(typeof(Overgrowth), nameof(Overgrowth.AllEvents), MethodType.Getter)]
public static class OvergrowthAllEventsPatch
{
    static void Postfix(ref IEnumerable<EventModel> __result)
    {
        __result = __result
            .Concat(new[] { ModelDb.Event<TestEvent>() })
            .Distinct();
    }
}