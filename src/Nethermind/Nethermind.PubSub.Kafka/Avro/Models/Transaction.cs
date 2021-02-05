// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Nethermind.PubSub.Kafka.Avro.Models
{
	using global::Avro;
	using global::Avro.Specific;
	
	public partial class Transaction : ISpecificRecord
	{
		public static Schema _SCHEMA = Schema.Parse(@"{""type"":""record"",""name"":""Transaction"",""namespace"":""Nethermind.PubSub.Kafka.Avro.Models"",""fields"":[{""name"":""blockHash"",""type"":""string""},{""name"":""blockNumber"",""type"":""long""},{""name"":""fromAddr"",""type"":""string""},{""name"":""gas"",""type"":""long""},{""name"":""gasPrice"",""type"":""long""},{""name"":""hash"",""type"":""string""},{""name"":""input"",""type"":""bytes""},{""name"":""nonce"",""type"":""int""},{""name"":""toAddr"",""type"":[""null"",""string""]},{""name"":""transactionIndex"",""type"":""int""},{""name"":""weiValue"",""type"":""string""},{""name"":""v"",""type"":""int""},{""name"":""r"",""type"":""string""},{""name"":""s"",""type"":""string""}]}");
		private string _blockHash;
		private long _blockNumber;
		private string _fromAddr;
		private long _gas;
		private long _gasPrice;
		private string _hash;
		private byte[] _input;
		private int _nonce;
		private string _toAddr;
		private int _transactionIndex;
		private string _weiValue;
		private long _v;
		private string _r;
		private string _s;
		public virtual Schema Schema
		{
			get
			{
				return Transaction._SCHEMA;
			}
		}
		public string blockHash
		{
			get
			{
				return this._blockHash;
			}
			set
			{
				this._blockHash = value;
			}
		}
		public long blockNumber
		{
			get
			{
				return this._blockNumber;
			}
			set
			{
				this._blockNumber = value;
			}
		}
		public string fromAddr
		{
			get
			{
				return this._fromAddr;
			}
			set
			{
				this._fromAddr = value;
			}
		}
		public long gas
		{
			get
			{
				return this._gas;
			}
			set
			{
				this._gas = value;
			}
		}
		public long gasPrice
		{
			get
			{
				return this._gasPrice;
			}
			set
			{
				this._gasPrice = value;
			}
		}
		public string hash
		{
			get
			{
				return this._hash;
			}
			set
			{
				this._hash = value;
			}
		}
		public byte[] input
		{
			get
			{
				return this._input;
			}
			set
			{
				this._input = value;
			}
		}
		public int nonce
		{
			get
			{
				return this._nonce;
			}
			set
			{
				this._nonce = value;
			}
		}
		public string toAddr
		{
			get
			{
				return this._toAddr;
			}
			set
			{
				this._toAddr = value;
			}
		}
		public int transactionIndex
		{
			get
			{
				return this._transactionIndex;
			}
			set
			{
				this._transactionIndex = value;
			}
		}
		public string weiValue
		{
			get
			{
				return this._weiValue;
			}
			set
			{
				this._weiValue = value;
			}
		}
		public long v
		{
			get
			{
				return this._v;
			}
			set
			{
				this._v = value;
			}
		}
		public string r
		{
			get
			{
				return this._r;
			}
			set
			{
				this._r = value;
			}
		}
		public string s
		{
			get
			{
				return this._s;
			}
			set
			{
				this._s = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.blockHash;
			case 1: return this.blockNumber;
			case 2: return this.fromAddr;
			case 3: return this.gas;
			case 4: return this.gasPrice;
			case 5: return this.hash;
			case 6: return this.input;
			case 7: return this.nonce;
			case 8: return this.toAddr;
			case 9: return this.transactionIndex;
			case 10: return this.weiValue;
			case 11: return this.v;
			case 12: return this.r;
			case 13: return this.s;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.blockHash = (System.String)fieldValue; break;
			case 1: this.blockNumber = (System.Int64)fieldValue; break;
			case 2: this.fromAddr = (System.String)fieldValue; break;
			case 3: this.gas = (System.Int64)fieldValue; break;
			case 4: this.gasPrice = (System.Int64)fieldValue; break;
			case 5: this.hash = (System.String)fieldValue; break;
			case 6: this.input = (System.Byte[])fieldValue; break;
			case 7: this.nonce = (System.Int32)fieldValue; break;
			case 8: this.toAddr = (System.String)fieldValue; break;
			case 9: this.transactionIndex = (System.Int32)fieldValue; break;
			case 10: this.weiValue = (System.String)fieldValue; break;
			case 11: this.v = (System.Int32)fieldValue; break;
			case 12: this.r = (System.String)fieldValue; break;
			case 13: this.s = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
