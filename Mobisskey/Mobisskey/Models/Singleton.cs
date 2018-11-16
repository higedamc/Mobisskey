namespace Mobisskey.Models
{
	public abstract class Singleton<T> where T : Singleton<T>, new()
	{
		protected Singleton() { }

		public static T I { get; } = new T();
	}
}
