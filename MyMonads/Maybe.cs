using NUnit.Framework;

namespace MyMonads
{
	public class Maybe<TValue>
	{
		public TValue Value { get; private set; }
		public bool HasValue { get; private set; }

		public Maybe(TValue value)
		{
			Value = value;
			HasValue = true;
		}

		public Maybe()
		{
			HasValue = false;
		} 
	}

	[TestFixture]
	public class MaybeTests
	{
		[Test]
		public void Maybe_HasValue()
		{
			var m = new Maybe<int>(2);

			Assert.AreEqual(2, m.Value);
			Assert.True(m.HasValue);
		}

		[Test]
		public void Maybe_DoesntHaveValue()
		{
			var m = new Maybe<int>();

			Assert.False(m.HasValue);
		}

	}
}