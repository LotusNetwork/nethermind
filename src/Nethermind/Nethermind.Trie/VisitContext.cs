//  Copyright (c) 2021 Demerzel Solutions Limited
//  This file is part of the Nethermind library.
// 
//  The Nethermind library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  The Nethermind library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading;

namespace Nethermind.Trie
{
    public class TrieVisitContext : IDisposable
    {
        private SemaphoreSlim? _semaphore;
        private readonly int _maxDegreeOfParallelism = 1;

        public int Level { get; internal set; }
        public bool IsStorage { get; internal set; }
        public int? BranchChildIndex { get; internal set; }
        public bool ExpectAccounts { get; init; }

        public int MaxDegreeOfParallelism
        {
            get => _maxDegreeOfParallelism;
            internal init => _maxDegreeOfParallelism = value == 0 ? Environment.ProcessorCount : value;
        }

        public SemaphoreSlim Semaphore
        {
            get
            {
                if (_semaphore is null)
                {
                    if (MaxDegreeOfParallelism == 1) throw new InvalidOperationException("Can not create semaphore for single threaded trie visitor.");
                    _semaphore = new SemaphoreSlim(MaxDegreeOfParallelism, MaxDegreeOfParallelism);
                }

                return _semaphore;
            }
        }

        public TrieVisitContext Clone() => (TrieVisitContext) MemberwiseClone();
        
        public void Dispose()
        {
            _semaphore?.Dispose();
        }
    }
}
