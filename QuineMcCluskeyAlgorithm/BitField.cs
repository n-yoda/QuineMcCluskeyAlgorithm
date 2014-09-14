using System;
using System.Collections.Generic;
using System.Collections;

namespace QuineMcCluskeyAlgorithm
{
	/// <summary>
	/// 長さを指定出来る高速なビットフィールド。
	/// </summary>
	public class BitField
	{
		ulong[] fields;
		readonly ulong highMask;
		readonly int length;

		public BitField (int length)
		{
			if (length <= 0) {
				throw new NotSupportedException ();
			}
			this.length = length;
			fields = new ulong[length / 64 + 1];
			highMask = (1ul << (length % 64)) - 1ul;
		}

		public int Length 
		{
			get {
				return length;
			}
		}

		public bool this [int i]
		{
			get {
				if (i >= length || i < 0) {
					throw new IndexOutOfRangeException ();
				} else {
					int div = i / 64;
					ulong mask = 1ul << (i % 64);
					return (fields [div] & mask) != 0ul;
				}
			}
			set {
				if (i >= length || i < 0) {
					throw new IndexOutOfRangeException ();
				} else {
					int div = i / 64;
					ulong mask = 1ul << (i % 64);
					if (value) {
						fields [div] |= mask;
					} else {
						fields [div] &= ~mask;
					}
				}
			}
		}

		public bool IsZero
		{
			get {
				var last = fields.Length - 1;
				for (int i = 0; i < last; i ++) {
					if (fields [i] != 0)
						return false;
				}
				return (fields [last] & highMask) == 0ul;
			}
		}

		public bool IsMax
		{
			get {
				var last = fields.Length - 1;
				for (int i = 0; i < last; i ++) {
					if (fields [i] != ulong.MaxValue)
						return false;
				}
				return (fields [last] & highMask) == highMask;
			}
		}

		// 包含関係
		public bool Contains (BitField a)
		{
			var last = fields.Length - 1;
			for (int i = 0; i < last; i ++) {
				if ((a.fields [i] | fields [i]) != fields[i])
					return false;
			}
			return ((a.fields [last] & a.highMask) | fields [last]) == fields [last];
		}

		// 長さが違うとエラー（特にエラー処理はしない）
		public static BitField operator | (BitField a, BitField b){
			var c = new BitField (a.length);
			for (int i = 0; i < a.fields.Length; i ++) {
				c.fields [i] = a.fields [i] | b.fields [i];
			}
			return c;
		}
		public static BitField operator & (BitField a, BitField b){
			var c = new BitField (a.length);
			for (int i = 0; i < a.fields.Length; i ++) {
				c.fields [i] = a.fields [i] & b.fields [i];
			}
			return c;
		}
		public static BitField operator ^ (BitField a, BitField b){
			var c = new BitField (a.length);
			for (int i = 0; i < a.fields.Length; i ++) {
				c.fields [i] = a.fields [i] ^ b.fields [i];
			}
			return c;
		}
		public static BitField operator ~ (BitField a){
			var c = new BitField (a.length);
			for (int i = 0; i < a.fields.Length; i ++) {
				c.fields [i] = ~a.fields [i];
			}
			return c;
		}
		public static bool operator == (BitField a, BitField b){
			if ((object)a == null || (object)b == null) {
				if ((object)a == null && (object)b == null) {
					return true;
				} else {
					return false;
				}
			}
			var last = a.fields.Length - 1;
			for (int i = 0; i < last; i ++) {
				if ((a.fields [i] ^ b.fields [i]) != 0ul)
					return false;
			}
			return ((a.fields [last] ^ b.fields [last]) & a.highMask) == 0ul;
		}
		public static bool operator != (BitField a, BitField b){
			return !(a == b);
		}
		public override bool Equals (object obj)
		{
			return base.Equals (obj);
		}
		public override string ToString ()
		{
			char[] result = new char[length];
			for (int i = 0; i < Length; i ++) {
				result [i] = this[i] ? '1' : '0';
			}
			return new string (result);
		}
	}
}

