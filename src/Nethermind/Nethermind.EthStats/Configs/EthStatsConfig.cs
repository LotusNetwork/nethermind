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

namespace Nethermind.EthStats.Configs
{
    public class EthStatsConfig : IEthStatsConfig
    {
        public bool Enabled { get; set; }
        public string? Server { get; set; } = "ws://localhost:3000/api";
        public string? Name { get; set; } = "Nethermind";
        public string? Secret { get; set; } = "secret";
        public string? Contact { get; set; } = "hello@nethermind.io";
    }
}
