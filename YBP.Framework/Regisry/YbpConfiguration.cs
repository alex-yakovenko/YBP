using System;
using System.Collections.Generic;
using System.Linq;

namespace YBP.Framework.Regisry
{
    public class YbpConfiguration
    {
        public static Dictionary<Type, List<IYbpActionBase>> Actions { get; private set; } 
            = new Dictionary<Type, List<IYbpActionBase>>();

        public static void LoadActionsFromAssembly<TTypeFromAssembly>()
        {
            var assembly = typeof(TTypeFromAssembly).Assembly;

            var i = typeof(IYbpActionBase);

            var actions = assembly
                .GetTypes()
                .Where(x => i.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && x.IsPublic)
                .GroupBy(x => GetTopParent(x).GenericTypeArguments[0])
                .Select(x => new {
                    x.Key,
                    Vals = x.Select(y => CreateInstance(y)).ToList()
                })
                .ToDictionary(
                    x => x.Key,
                    x => x.Vals
                );

            foreach (var item in actions)
            {
                if (!Actions.ContainsKey(item.Key))
                    Actions.Add(item.Key, new List<IYbpActionBase>());

                Actions[item.Key].AddRange(item.Value);
            }
        }

        private static Type GetTopParent(Type p)
        {
            while (p.Name != "YbpActionBase`3")
            {
                p = p.BaseType;
            }
            return p;
        }

        private static IYbpActionBase CreateInstance(Type type)
        {
            var constructor = type
                .GetConstructors()
                .OrderBy(x => x.GetParameters().Count())
                .FirstOrDefault();

            if (constructor == null || constructor.GetParameters().Count() == 0 )
                return Activator.CreateInstance(type) as IYbpActionBase;

            var prms = constructor
                .GetParameters()
                .Select(x => x.ParameterType.IsValueType ? Activator.CreateInstance(type) : null)
                .ToArray();

            return Activator.CreateInstance(type, prms) as IYbpActionBase;
        }


    }

}
