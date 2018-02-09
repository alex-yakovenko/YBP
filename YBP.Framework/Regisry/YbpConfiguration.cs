using System;
using System.Collections.Generic;
using System.Linq;

namespace YBP.Framework.Regisry
{
    public class YbpConfiguration
    {
        public static Dictionary<Type, YbpProcessBase> Processes { get; private set; } 
            = new Dictionary<Type, YbpProcessBase>();

        public static void LoadActionsFromAssembly<TTypeFromAssembly>()
        {
            var assembly = typeof(TTypeFromAssembly).Assembly;

            var i = typeof(YbpProcessBase);

            Processes = assembly
                .GetTypes()
                .Where(x => i.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && x.IsPublic)
                .ToDictionary(
                    x => x,
                    x => Activator.CreateInstance(x) as YbpProcessBase
                );

        }

        private static Type GetTopParent(Type p)
        {
            while (p.Name != "YbpActionBase`3")
            {
                p = p.BaseType;
            }
            return p;
        }

    }

}
