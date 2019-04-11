/*
 * Copyright (c) 2018 Demerzel Solutions Limited
 * This file is part of the Nethermind library.
 *
 * The Nethermind library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The Nethermind library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Linq;
using Nethermind.Core.Logging;

namespace Nethermind.Blockchain.Synchronization
{
    internal class SyncPeersReport
    {
        private TimeSpan _fullPeerListInterval = TimeSpan.FromSeconds(120);
        private DateTime _timeOfTheLastFullPeerListLogEntry = DateTime.MinValue;
        private int _lastInitializedPeerCount;

        private readonly IEthSyncPeerPool _peerPool;
        private readonly ILogger _logger;

        public SyncPeersReport(IEthSyncPeerPool peerPool, ILogManager logManager)
        {
            _peerPool = peerPool ?? throw new ArgumentNullException(nameof(peerPool));
            _logger = logManager?.GetClassLogger() ?? throw new ArgumentNullException(nameof(logManager));
        }

        private object _writeLock = new object();

        public void Write()
        {
            TimeSpan timeSinceLastEntry;
            int initializedPeerCount;
            lock (_writeLock)
            {
                timeSinceLastEntry = DateTime.UtcNow - _timeOfTheLastFullPeerListLogEntry;
                
                _timeOfTheLastFullPeerListLogEntry = DateTime.UtcNow;
                 initializedPeerCount = _peerPool.AllPeers.Count(p => p.IsInitialized);
                _lastInitializedPeerCount = initializedPeerCount;
            }

            if (timeSinceLastEntry > _fullPeerListInterval)
            {
                if (_logger.IsInfo) _logger.Info($"Sync peers {initializedPeerCount}({_peerPool.PeerCount})/{_peerPool.PeerMaxCount}");
                foreach (PeerInfo peerInfo in _peerPool.AllPeers)
                {
                    string prefix = _peerPool.Allocations.Any(a => a.Current == peerInfo)
                        ? " * "
                        : "   ";

                    if (_logger.IsInfo) _logger.Info($"{prefix}{peerInfo}");
                }
            }
            else if (initializedPeerCount != _lastInitializedPeerCount)
            {
                if (_logger.IsInfo) _logger.Info($"Sync peers {initializedPeerCount}({_peerPool.PeerCount})/{_peerPool.PeerMaxCount}");
                foreach (SyncPeerAllocation syncPeerAllocation in _peerPool.Allocations)
                {
                    if (syncPeerAllocation.Current != null)
                    {
                        if (_logger.IsInfo) _logger.Info($"  sync in progress with {syncPeerAllocation.Current}");
                    }
                }
            }
            else if (initializedPeerCount == 0)
            {
                if (_logger.IsInfo) _logger.Info($"Sync peers 0({_peerPool.PeerCount})/{_peerPool.PeerMaxCount}, searching for peers to sync with...");
            }
        }
    }
}