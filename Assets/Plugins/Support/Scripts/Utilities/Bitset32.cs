using System;

namespace Support
{
	public struct Bitset32
	{
		private int _bitset;

		public int Bitset => _bitset;
		
		public bool this[int i]
		{
			get
			{
				if (i > 31 || i < 0)
					throw new IndexOutOfRangeException($"{nameof(Bitset32)} can take only values form range [0, 31]");
				
				int targetBit = 1 << i;

				return (_bitset & targetBit) > 0;
			}
			set
			{
				if (i > 31 || i < 0)
					throw new IndexOutOfRangeException($"{nameof(Bitset32)} can take only values form range [0, 31]");
				
				int targetBit = 1 << i;
				
				if (value) 
					_bitset |= targetBit;
				else
					_bitset &= ~targetBit;
			}
		}

		public override string ToString()
		{
			return $"Bitset: {Convert.ToString(_bitset, 2)}";
		}

		public static Bitset32 operator |(Bitset32 b1, Bitset32 b2)
		{
			b1._bitset |= b2._bitset;

			return b1;
		}
		
		public static Bitset32 operator &(Bitset32 b1, Bitset32 b2)
		{
			b1._bitset &= b2._bitset;

			return b1;
		}
	}
}