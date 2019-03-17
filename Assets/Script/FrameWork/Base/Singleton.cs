using System.Diagnostics;

namespace Framework
{
	public class Singleton<T> where T: new()
	{
		private static readonly object _lock = new object();
		private static T _Instance;
		
		protected Singleton()
		{
			Debug.Assert(_Instance == null);
		}
		
		public static bool Exists
		{
			get
			{
				return _Instance != null;
			}
		}
		
		public static T Instance
		{
			get {
				if (_Instance == null)
				{
					lock (_lock)
					{
						if (_Instance == null)
						{
							_Instance = new T();
						}
					}
				}
				return _Instance;
			}
		}
	}
}