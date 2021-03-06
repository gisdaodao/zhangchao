﻿// 2012 OKr Works, http://okr.me

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKr.Common.Context
{
    public interface IOKrStateContext<TData>
    {
        Task<TData> Load();
        Task Save(TData data);
        Task Clear();
    }
}
