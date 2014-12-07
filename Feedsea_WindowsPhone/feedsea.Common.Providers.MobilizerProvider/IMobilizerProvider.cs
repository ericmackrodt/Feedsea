﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers.MobilizerProvider
{
    public interface IMobilizerProvider
    {
        Task<string> GetMobilized(Mobilizer mobilizer, string articleUrl);
    }
}
