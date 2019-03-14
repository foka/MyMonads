using System;
using NUnit.Framework;

namespace MyMonads
{
	public class Maybe<TValue>
	{
		private readonly TValue value;
		private readonly bool hasValue;

		public Maybe(TValue value)
		{
			this.value = value;
			this.hasValue = true;
		}

		private Maybe()
		{
			this.hasValue = false;
		}

		public Maybe<TValue> Bind(Func<TValue, Maybe<TValue>> bindFn)
		{
			return hasValue ? bindFn(this.value) : None();
		}

		public T Match<T>(Func<TValue, T> valueFn, Func<T> nothingFn)
		{
			return this.hasValue ? valueFn(this.value) : nothingFn();
		}

		public static Maybe<TValue> None()
		{
			return new Maybe<TValue>();
		}
	}

	[TestFixture]
	public class MaybeTests
	{
		class EmployeeOld
		{
			public string Name;
			public EmployeeOld Manager;
		}

		class EmployeeNew
		{
			public string Name;
			public Maybe<EmployeeNew> Manager;
		}

		static string GetManagerManagerNameOld(EmployeeOld employee)
		{
			if (employee.Manager == null)
			{
				return null;
			}
			if (employee.Manager.Manager == null)
			{
				return null;
			}

			return employee.Manager.Manager.Name;
		}

		static string GetManagerManagerNameNew(EmployeeNew employee)
		{
			 return employee.Manager
				.Bind(e => e.Manager)
				.Match(e => e.Name, () => null);
		}

		[Test]
		public void WriteEmployeeManagerManagerWithoutMonads()
		{
			var m0 = new EmployeeOld { Name = "RB" };
			var m1 = new EmployeeOld { Name = "MJ", Manager = m0 };
			var m2 = new EmployeeOld { Name = "MT", Manager = m1 };

			var directorName = GetManagerManagerNameOld(m2);

			Assert.AreEqual("RB", directorName);
		}

		[Test]
		public void WriteEmployeeManagerManagerWithMonadsExample()
		{
			var m0 = new EmployeeNew { Name = "RB", Manager = Maybe<EmployeeNew>.None() };
			var m1 = new EmployeeNew { Name = "MJ", Manager = new Maybe<EmployeeNew>(m0) };
			var m2 = new EmployeeNew { Name = "MT", Manager = new Maybe<EmployeeNew>(m1) };

			var directorName = GetManagerManagerNameNew(m2);

			Assert.AreEqual("RB", directorName);
		}
	}
}