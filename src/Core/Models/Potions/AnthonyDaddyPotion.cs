#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2mod.Potions;

// 【药水类名 - 你想要的名称】
public sealed class AnthonyDaddyPotion : PotionModel
{
    public override PotionRarity Rarity => PotionRarity.Common;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyPlayer;

    // 动态变量：格挡 15，Unpowered 表示不受力量修正
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(15m, ValueProp.Unpowered)
    ];

    // 悬浮提示：显示“格挡”说明
    public override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PotionModel.AssertValidForTargetedPotion(target);

        await CreatureCmd.GainBlock(
            target,
            base.DynamicVars.Block,
            null
        );
    }
}