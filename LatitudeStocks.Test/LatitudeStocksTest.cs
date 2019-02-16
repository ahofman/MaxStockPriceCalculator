using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LatitudeStocks.Test
{
    [TestClass]
    public class LatitudeStocksTest
    {
        [TestMethod]
        public void AllPricesIncreasing()
        {
            var testData = new decimal[] { 1, 2, 3, 4, 5, 6, 7 };

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void AllPricesDecreasing()
        {
            var testData = new decimal[] { 7, 6, 5, 4, 3, 2, 1 };

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);
            
            Assert.AreEqual(-1, result);
        }

        /// <summary>
        /// On a trading day where all prices only go down, we want
        /// the method to return the least amount of loss possible.
        /// </summary>
        [TestMethod]
        public void AllPricesDecreasingWithDifferentLosses()
        {
            var testData = new decimal[] { 100, 72, 70, 60, 40, 20, 10 };

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);

            Assert.AreEqual(-2, result);
        }

        [TestMethod]
        public void AllPricesTheSame()
        {
            var testData = new decimal[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TwoPrices()
        {
            var testData = new decimal[] { 10, 20 };

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void DownUpDown()
        {
            var testData = new decimal[] { 5, 3, 1, 2, 7, 11, 10 };

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void SmallSpreadThenBigSpread()
        {
            var testData = new decimal[] { 10, 9, 8, 9, 10, 11, 12, // small spread of 8 - 12
             11, 8, 5, 3, 5, 8, 10, 14, 10 };   // larger spread of 3 - 14

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);

            Assert.AreEqual(11, result);
        }

        /// <summary>
        /// If we only take the min and max of the input sequence we are ignoring
        /// the constraint that you can only sell after you buy. This test exercises
        /// this requirement.
        /// </summary>
        [TestMethod]
        public void SpreadThenLaterLow()
        {
            var testData = new decimal[] { 10, 9, 8, 9, 10, 11, 12, // small spread of 8 - 12
             10, 8, 6, 4, 2, 1 };   // followed by a big low

            var result = LatitudeStocks.GetTheoreticalMaxProfit(testData);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void InvalidStockPriceData()
        {
            InnerInvalidStockPriceData(new decimal[] { Decimal.MinValue });
            InnerInvalidStockPriceData(new decimal[] { Decimal.MaxValue });
            InnerInvalidStockPriceData(new decimal[] { Decimal.MinusOne }); // Stock prices cannot be negative
            InnerInvalidStockPriceData(new decimal[] { 0, 1, 2 });    // Stock prices cannot be 0
            InnerInvalidStockPriceData(new decimal[] { 9 });    // We need at least two stock prices
            InnerInvalidStockPriceData(new decimal[] { });
            InnerInvalidStockPriceData(null);
        }

        private void InnerInvalidStockPriceData(IList<decimal> testData)
        {
            Assert.ThrowsException<ArgumentException>(() =>
                LatitudeStocks.GetTheoreticalMaxProfit(testData));
        }
    }
}
