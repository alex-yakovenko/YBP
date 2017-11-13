using System;
using System.Collections.Generic;
using System.Linq;

namespace YBP.Framework.Regisry
{
    public class YbpConfiguration
    {
        public static Dictionary<Type, List<Type>> Actions => new Dictionary<Type, List<Type>>();

        public static void LoadActionsFromAssembly<TTypeFromAssembly>()
        {
            var assembly = typeof(TTypeFromAssembly).Assembly;

            var i = typeof(YbpActionBase<,,>);

            var actions = assembly
                .GetTypes()
                .Where(x => i.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && x.IsPublic)
                .GroupBy(x => x.BaseType.GenericTypeArguments[0])
                .Select(x => new { x.Key, Vals = x.ToList() })
                .ToDictionary(
                    x => x.Key,
                    x => x.Vals
                );

            foreach (var item in actions)
            {
                Actions[item.Key] = Actions[item.Key] ?? new List<Type>();
                Actions[item.Key].AddRange(item.Value);
            }
        }

    }

}
