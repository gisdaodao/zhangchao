using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OKr.Common.Context;
using OKr.Common.Storage;
using OKr.Win8Book.Client.Core.Data;

namespace OKr.Win8Book.Client.Core.Context
{
    public class ProgressContext : OKrStateContextBase<Progress>
    {
        public ProgressContext()
            : base(new OKrStorage<Progress>(OKrBookConstant.PROGRESS), true)
        {
 
        }

        protected async override Task<Progress> DoLoad()
        {
            return new Progress();
        }

        protected override void DoSave(Progress data)
        {
        }
    }
}
