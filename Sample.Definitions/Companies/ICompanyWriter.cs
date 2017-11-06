using System;
using System.Collections.Generic;
using Sample.Definitions.Common;

namespace Sample.Definitions.Companies
{
    public interface ICompanyWriter: IEntitySaver, IEntityRemover
    {
    }
}
