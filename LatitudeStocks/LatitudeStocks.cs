using System;
using System.Collections.Generic;
using System.Linq;

namespace LatitudeStocks
{
    public static class LatitudeStocks
    {
        /// <summary>
        /// Determines the theoretical maximum profit from a single buy and sell of a stock.
        /// 
        /// The buy happens before the sell (i.e., short selling is not considered).
        /// Brokerage fees are also not considered - this method only calculates the raw profit
        /// from the increase in value of the stock over time.
        /// 
        /// In the event that profit is not possible (i.e., if stock prices only decrease for the day)
        /// the smallest possible loss is returned.
        /// </summary>
        /// <param name="dayPricesByMinute">A dense list of stock prices, given in dollars. 
        /// The index of each element corresponds to the number of minutes that have elasped since trading 
        /// began for the day.</param>
        /// <returns>The theoretical maximum profit (or minimum loss) from one buy and one sell for the day.</returns>
        public static decimal GetTheoreticalMaxProfit(IList<decimal> dayPricesByMinute)
        {
            if (dayPricesByMinute == null || 
                dayPricesByMinute.Count() <= 1) // we need at least 2 prices to be able to buy and sell
            {
                throw new ArgumentException($"{nameof(dayPricesByMinute)} must contain at least two positive nonzero values");
            }

            // Model the trading day as a series of "spreads" where a spread starts
            // at a low price and ends when a lower price is encountered, or the day ends.
            // When these conditions are met the theoretical max profit for the 
            // spread is calculated and a new spread starts. 
            // The max profit for the day is the max profit of all spreads.
            var firstPrice = dayPricesByMinute[0];
            ValidateStockPrice(firstPrice);
            decimal currentSpreadBuyPrice = firstPrice;
            decimal currentSpreadSellPrice = decimal.MinValue;
            decimal maxProfit = decimal.MinValue;

            for (int i = 1; i<dayPricesByMinute.Count; ++i)
            {
                var price = dayPricesByMinute[i];
                
                ValidateStockPrice(price);

                if (price < currentSpreadBuyPrice || // if we're starting a new spread
                    i == dayPricesByMinute.Count - 1)   // or it's the end of the day
                {
                    currentSpreadSellPrice = Math.Max(currentSpreadSellPrice, price);

                    // calculate profit
                    maxProfit = Math.Max(maxProfit, currentSpreadSellPrice - currentSpreadBuyPrice);

                    // and start a new spread
                    currentSpreadBuyPrice = price;
                    currentSpreadSellPrice = decimal.MinValue;
                }
                else
                {
                    currentSpreadBuyPrice = Math.Min(price, currentSpreadBuyPrice);
                    currentSpreadSellPrice = Math.Max(price, currentSpreadSellPrice);
                }
            }

            return maxProfit;
        }

        private static void ValidateStockPrice(decimal price)
        {
            // If we ever get to ten million dollars per share 
            // that would be awesome
            const decimal MaxValidStockPrice = 10000000m;

            if (price <= 0 || price >= MaxValidStockPrice)
            {
                throw new ArgumentException($"Stock prices must be greater than zero and less than {MaxValidStockPrice}");
            }
        }
    }
}
