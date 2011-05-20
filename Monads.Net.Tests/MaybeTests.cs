using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monads.NET;

namespace Monads.Net.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class MaybeTests
	{
		public MaybeTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void With_on_An_Class_returns_value()
		{
			var testValue = "Erin Was here";
			var td = new TestableDummy
			{
				DummyString = testValue,
				DummyValueType = 5
			};

			Assert.AreEqual(td.With(c => c.DummyString), testValue);
		}

		[TestMethod]
		public void With_will_return_Null_when_class_is_null()
		{
			TestableDummy td = null;

			Assert.IsNull(td.With(c => c.DummyString));
		}

		[TestMethod]
		public void With_on_Dictionary_returns_Value()
		{
			var testValue = "Erin Was here";
			var testKey = "tiger";
			var td = new TestableDummy
			{
				DummyString = testValue,
				DummyValueType = 5,
				DummyDictionary = new Dictionary<string, string>()
			};
			td.DummyDictionary[testKey] = testValue;

			Assert.AreEqual(td.DummyDictionary.With(testKey), testValue);
		}

		[TestMethod]
		public void With_on_Dictionary_Without_Key_Returns_Null()
		{
			var td = new TestableDummy
			{
				DummyDictionary = new Dictionary<string, string>()
			};

			Assert.IsNull(td.DummyDictionary.With("TestValueLookup"));
		}

		[TestMethod]
		public void With_on_null_Dictionary_will_Return_Null()
		{
			var td = new TestableDummy
			{
				DummyValueType = 5,
			};

			Assert.IsNull(td.DummyDictionary.With("TestValueLookup"));
		}

		[TestMethod]
		public void Return_on_a_class_returns_value()
		{
			var testValue = "Erin Was here";
			var td = new TestableDummy
			{
				DummyString = testValue,
				DummyValueType = 5
			};

			Assert.AreEqual(td.Return(c => c.DummyString, "monkey"), testValue);
		}

		[TestMethod]
		public void Return_on_a_class_returns_ValueType_value()
		{

			var td = new TestableDummy
			{
				DummyValueType = 5
			};
			Assert.AreEqual(td.Return(c => c.DummyValueType, 8), 5);
		}

		[TestMethod]
		public void Return_on_null_class_returns_failValue()
		{
			var testValue = "Erin Was here";
			var td = new TestableDummy
			{
				DummyValueType = 5
			};
			Assert.AreNotEqual(td.Return(c => c.DummyString, "monkey"), testValue);
		}

		[TestMethod]
		public void Return_on_a_class_returns_ValueType_failvalue()
		{

			var td = new TestableDummy
			{
				DummyString = "hi ho hi ho"
			};
			Assert.AreNotEqual(td.Return(c => c.DummyValueType, 8), 5);
		}

		[TestMethod]
		public void Return_on_Dictionary_will_return_Value()
		{
			var testValue = "Erin Was here";
			var testKey = "tiger";
			var td = new TestableDummy
			{
				DummyString = testValue,
				DummyValueType = 5,
				DummyDictionary = new Dictionary<string, string>()
			};
			td.DummyDictionary[testKey] = testValue;
			Assert.AreEqual(td.DummyDictionary.Return(testKey, "notTestValue"), testValue);
		}

		[TestMethod]
		public void Return_on_Dictionary_no_key_return_fail_value()
		{
			var testValue = "Erin Was here";
			var testKey = "tiger";
			var td = new TestableDummy
			{
				DummyString = testValue,
				DummyValueType = 5,
				DummyDictionary = new Dictionary<string, string>()
			};
			td.DummyDictionary[testKey] = testValue;
			Assert.AreNotEqual(td.DummyDictionary.Return("Fire", "notTestValue"), testValue);
		}

		[TestMethod]
		public void Return_on_null_dictionary_returns_null()
		{
			var testValue = "Erin Was here";
			var testKey = "tiger";
			var td = new TestableDummy
			{
				DummyString = testValue,
				DummyValueType = 5
			};
			Assert.AreNotEqual(td.DummyDictionary.Return(testKey, "notTestValue"), testValue);
		}

		[TestMethod]
		public void Do_on_IEnumerable()
		{
			var source = new[] { "a", "b", "c" };

			var items = new List<string>();
			source.Do(i => { items.Add(i); });

			Assert.AreEqual("a,b,c", String.Join(",", items));
		}


		[TestMethod]
		public void With_on_IEnumerable_returns_value()
		{
			var source = new[] { "a", "b", "c" };

			var result = source.With(c => c + c);

			Assert.AreEqual("aa,bb,cc", String.Join(",", result));
		}

		[TestMethod]
		public void With_on_IEnumerable_is_null()
		{
			string[] source = null;

			Assert.IsNull(source.With(c => c + c));
		}
	}

	public class TestableDummy
	{
		public string DummyString { get; set; }
		public int DummyValueType { get; set; }
		public Dictionary<string, string> DummyDictionary { get; set; }
	}

}
