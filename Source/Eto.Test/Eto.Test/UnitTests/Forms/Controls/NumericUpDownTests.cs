﻿using Eto.Forms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eto.Test.UnitTests.Forms.Controls
{
	[TestFixture]
	public class NumericUpDownTests
	{
		[Test]
		public void DefaultValuesShouldBeCorrect()
		{
			TestUtils.Invoke(() =>
			{
				var numeric = new NumericUpDown();
				int valueChanged = 0;
				numeric.ValueChanged += (sender, e) => valueChanged++;

				Assert.AreEqual(double.NegativeInfinity, numeric.MinValue, "MinValue should be double.NegativeInfinity");
				Assert.AreEqual(double.PositiveInfinity, numeric.MaxValue, "MaxValue should be double.PositiveInfinity");
				Assert.AreEqual(0, numeric.Value, "initial value should be 0");

				Assert.AreEqual(0, valueChanged, "ValueChanged event should not fire when setting to default values");
			});
		}

		[Test]
		public void MinMaxShouldRetainValue()
		{
			TestUtils.Invoke(() =>
			{
				var numeric = new NumericUpDown();
				int valueChanged = 0;
				int currentValueChanged = 0;
				numeric.ValueChanged += (sender, e) => valueChanged++;

				numeric.MinValue = 100;
				numeric.MaxValue = 1000;
				Assert.AreEqual(100, numeric.MinValue, "MinValue should return the same value as set");
				Assert.AreEqual(1000, numeric.MaxValue, "MaxValue should return the same value as set");
				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event should fire when changing the MinValue");

				numeric.MinValue = double.NegativeInfinity;
				numeric.MaxValue = double.PositiveInfinity;
				numeric.Value = 0;

				Assert.AreEqual(double.NegativeInfinity, numeric.MinValue, "MinValue should be double.NegativeInfinity");
				Assert.AreEqual(double.PositiveInfinity, numeric.MaxValue, "MaxValue should be double.PositiveInfinity");
				Assert.AreEqual(0, numeric.Value, "Value should be back to 0");

				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event should fire when changing the MinValue");
			});
		}

		[Test]
		public void ValueShouldBeLimitedToMinMax()
		{
			TestUtils.Invoke(() =>
			{
				var numeric = new NumericUpDown();
				int valueChanged = 0;
				int currentValueChanged = 0;
				numeric.ValueChanged += (sender, e) => valueChanged++;

				numeric.MinValue = 0;
				numeric.MaxValue = 2000;
				Assert.AreEqual(0, numeric.MinValue, "Could not correctly set MinValue");
				Assert.AreEqual(2000, numeric.MaxValue, "Could not correctly set MaxValue");

				numeric.MinValue = 100;
				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event was not fired the correct number of times");
				Assert.AreEqual(100, numeric.Value, "Value should be set after MinValue is set");

				numeric.Value = 1000;
				Assert.AreEqual(1000, numeric.Value, "Could not correctly set Value after Min/Max is set");
				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event was not fired the correct number of times");

				numeric.Value = 2001;
				Assert.AreEqual(2000, numeric.Value, "Value should be limited to Min/Max value");
				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event was not fired the correct number of times");

				numeric.Value = -1000;
				Assert.AreEqual(100, numeric.Value, "Value should be limited to Min/Max value");
				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event was not fired the correct number of times");

				numeric.MinValue = 1000;
				Assert.AreEqual(1000, numeric.Value, "Value should be changed to match new MinValue");
				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event was not fired the correct number of times");

				numeric.MinValue = 0;
				numeric.MaxValue = 500;
				Assert.AreEqual(500, numeric.Value, "Value should be changed to match new MaxValue");
				Assert.AreEqual(++currentValueChanged, valueChanged, "ValueChanged event was not fired the correct number of times");
			});
		}

		[TestCase(100, 32.767, 33, 0)]
		[TestCase(100, 32.167, 32, 0)]
		[TestCase(100, 32.767, 32.767, 3)]
		[TestCase(100, 32.767, 32.77, 2)]
		public void FractionalMaxValueShouldSetValueCorrectly(double value, double maxValue, double newValue, int decimalPlaces)
		{
			TestUtils.Invoke(() =>
			{
				var numeric = new NumericUpDown();
				int valueChanged = 0;
				numeric.ValueChanged += (sender, e) => valueChanged++;

				numeric.DecimalPlaces = decimalPlaces;

				numeric.Value = value;
				Assert.AreEqual(1, valueChanged, "ValueChanged event was not fired the correct number of times");
				numeric.MaxValue = maxValue;
				Assert.AreEqual(newValue, numeric.Value, "Value should be changed to match new MaxValue");
				Assert.AreEqual(2, valueChanged, "ValueChanged event was not fired the correct number of times");
			});
		}

		[TestCase(10, 32.767, 32.767, 3)]
		[TestCase(10, 32.767, 32.77, 2)]
		[TestCase(10, 32.767, 33, 0)]
		[TestCase(10, 32.167, 32, 0)]
		public void FractionalMinValueShouldSetValueCorrectly(double value, double minValue, double newValue, int decimalPlaces)
		{
			TestUtils.Invoke(() =>
			{
				var numeric = new NumericUpDown();
				int valueChanged = 0;
				numeric.ValueChanged += (sender, e) => valueChanged++;

				numeric.DecimalPlaces = decimalPlaces;

				numeric.Value = value;
				numeric.MinValue = minValue;
				Assert.AreEqual(newValue, numeric.Value, "Value should be changed to match new MaxValue");
				Assert.AreEqual(2, valueChanged, "ValueChanged event was not fired the correct number of times");
			});
		}

		[TestCase(10.126, 10, 0)]
		[TestCase(10.126, 10.1, 1)]
		[TestCase(10.126, 10.13, 2)]
		public void ValueShouldBeRoundedToDecimalPlaces(double value, double newValue, int decimalPlaces)
		{
			TestUtils.Invoke(() =>
			{
				var numeric = new NumericUpDown();
				int valueChanged = 0;
				numeric.ValueChanged += (sender, e) => valueChanged++;

				numeric.DecimalPlaces = decimalPlaces;

				numeric.Value = value;
				Assert.AreEqual(newValue, numeric.Value, "Value should be be rounded to the number of decimal places");
				Assert.AreEqual(1, valueChanged, "ValueChanged event was not fired the correct number of times");
			});
		}

		[TestCase(10.126, 10, 0)]
		[TestCase(10.126, 10.1, 1)]
		[TestCase(10.126, 10.13, 2)]
		public void ValueShouldBeRoundedToDecimalPlacesWhenSetAfter(double value, double newValue, int decimalPlaces)
		{
			TestUtils.Invoke(() =>
			{
				var numeric = new NumericUpDown();
				int valueChanged = 0;
				numeric.ValueChanged += (sender, e) => valueChanged++;

				numeric.Value = value;
				Assert.AreEqual(Math.Round(value, 0), numeric.Value, "Value should be set to the initial value");

				numeric.DecimalPlaces = decimalPlaces;

				Assert.AreEqual(newValue, numeric.Value, "Value should be be rounded to the number of decimal places");
				Assert.AreEqual(1, valueChanged, "ValueChanged event was not fired the correct number of times");
			});
		}
	}
}
