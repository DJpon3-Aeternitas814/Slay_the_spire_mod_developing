using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2mod.Relics;

// 【遗物类名 - 你想要的名称】
public sealed class TestShield : RelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;

    // 动态变量：格挡 5，Unpowered 表示不受力量修正
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(15m, ValueProp.Unpowered)
    ];

    // 悬浮提示：显示“格挡”说明
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    // 战斗开始时触发
    public override async Task BeforeCombatStart()
    {
        Flash();  // 遗物图标闪一下
        await CreatureCmd.GainBlock(
            base.Owner.Creature,
            base.DynamicVars.Block,
            null
        );
    }
}