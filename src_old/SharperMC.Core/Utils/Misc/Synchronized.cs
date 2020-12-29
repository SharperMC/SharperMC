namespace SharperMC.Core.Utils.Misc
{
	public class Synchronized<T>
	{
		// *** Locking ***
		private readonly object _mValueLock;

		// *** Value buffer ***
		private T _mValue;

		// *** Access to value ***
		private T Value
		{
			get
			{
				lock (_mValueLock)
				{
					return _mValue;
				}
			}
			set
			{
				lock (_mValueLock)
				{
					_mValue = value;
				}
			}
		}

		// *******************
		// *** Constructor ***
		// *******************
		internal Synchronized()
		{
			_mValueLock = new object();
		}

		internal Synchronized(T value)
		{
			_mValueLock = new object();
			Value = value;
		}

		internal Synchronized(T value, object @lock)
		{
			_mValueLock = @lock;
			Value = value;
		}

		// ********************************
		// *** Type casting overloading ***
		// ********************************
		public static implicit operator T(Synchronized<T> value)
		{
			return value.Value;
		}
	}
}
