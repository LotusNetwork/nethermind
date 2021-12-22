using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nethermind.Core;
using Nethermind.Core.Crypto;

namespace Nethermind.State.SnapDb
{
    public interface ISortedAccounts
    {
        void AddAccount(Keccak addressHash);
    }
}
