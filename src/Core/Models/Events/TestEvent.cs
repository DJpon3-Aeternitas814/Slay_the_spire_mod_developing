using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2mod.Events;

public sealed class TestEvent : EventModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new MaxHpVar(10m),                                            // 最大生命值 +10
        new DamageVar(5m, ValueProp.Unblockable | ValueProp.Unpowered) // 直接扣血 5，不可格挡、不受力量影响
    ];

    // 初始页面的两个选项
    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return [
            new EventOption(this, TouchAltar, "TEST_EVENT.pages.INITIAL.options.TOUCH_ALTAR")
                .ThatDoesDamage(base.DynamicVars.Damage.BaseValue),   // 选项上显示"会造成5点伤害"
            new EventOption(this, Leave, "TEST_EVENT.pages.INITIAL.options.LEAVE")
        ];
    }

    // 选项 A：触摸祭坛
    private async Task TouchAltar()
    {
        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),  // 事件里没有真实玩家上下文，用这个包一层
            base.Owner.Creature,
            base.DynamicVars.Damage,            // 伤害值 5
            null, null, null                    // 来源、卡牌等，事件里都是 null
        );
        await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);

        // 切换到结束页
        SetEventFinished(L10NLookup("TEST_EVENT.pages.TOUCH_ALTAR.description"));
    }

    // 选项 B：离开
    private Task Leave()
    {
        SetEventFinished(L10NLookup("TEST_EVENT.pages.LEAVE.description"));
        return Task.CompletedTask;
    }
}