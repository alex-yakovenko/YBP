using System;

namespace YBP.Framework
{
    public interface IYbpActionDefinition
    {
        bool CanBeExecutedAutomatically { get; }

        bool RunOnlyOnce { get; }

        Func<YbpFlagsDictionary, bool> NeedsToBeExecuted { get; }

        Func<YbpFlagsDictionary, bool> MayBeExecuted { get; }

        Func<YbpFlagsDictionary, bool> MayNotBeExecuted { get; }

        Type ActionType { get; }
    }
}