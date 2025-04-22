using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace OrderingSystem.Tests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void CalculateTotal_WithFoodAndDrink_Applies10PercentDiscount()
        {
            // Arrange
            var order = new Order();
            order.Items.Add((new MenuItem { Name = "Pizza", FoodType = "Food", Price = 10.00m }, 1));
            order.Items.Add((new MenuItem { Name = "Tea", FoodType = "Drink", Price = 5.00m }, 1));

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(13.50m, total); // 10% discount on 15.00
            Assert.AreEqual(1.50m, discount);
        }

        [TestMethod]
        public void CalculateTotal_WithFoodAndWater_NoDiscountApplied()
        {
            // Arrange
            var order = new Order();
            order.Items.Add((new MenuItem { Name = "Pizza", FoodType = "Food", Price = 10.00m }, 1));
            order.Items.Add((new MenuItem { Name = "Water", FoodType = "Drink", Price = 0.00m }, 1));

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(10.00m, total);
            Assert.AreEqual(0.00m, discount);
        }

        [TestMethod]
        public void CalculateTotal_WithTotalOver20_Applies20PercentDiscount()
        {
            // Arrange
            var order = new Order();
            order.Items.Add((new MenuItem { Name = "Pizza", FoodType = "Food", Price = 15.00m }, 1));
            order.Items.Add((new MenuItem { Name = "Tea", FoodType = "Drink", Price = 10.00m }, 1));

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(20.00m, total); // 20% discount on 25.00
            Assert.AreEqual(5.00m, discount);
        }

        [TestMethod]
        public void CalculateTotal_WithDiscountExceeding6_CapsDiscountAt6()
        {
            // Arrange
            var order = new Order();
            order.Items.Add((new MenuItem { Name = "Pizza", FoodType = "Food", Price = 30.00m }, 1));
            order.Items.Add((new MenuItem { Name = "Tea", FoodType = "Drink", Price = 10.00m }, 1));

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(34.00m, total); // Discount capped at 6.00
            Assert.AreEqual(6.00m, discount);
        }

        [TestMethod]
        public void CalculateTotal_WithNoItems_ReturnsZeroTotalAndNoDiscount()
        {
            // Arrange
            var order = new Order();

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(0.00m, total);
            Assert.AreEqual(0.00m, discount);
        }

        [TestMethod]
        public void CalculateTotal_WithOnlyFood_NoDiscountApplied()
        {
            // Arrange
            var order = new Order();
            order.Items.Add((new MenuItem { Name = "Pizza", FoodType = "Food", Price = 10.00m }, 1));

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(10.00m, total);
            Assert.AreEqual(0.00m, discount);
        }

        [TestMethod]
        public void CalculateTotal_WithOnlyDrink_NoDiscountApplied()
        {
            // Arrange
            var order = new Order();
            order.Items.Add((new MenuItem { Name = "Tea", FoodType = "Drink", Price = 5.00m }, 1));

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(5.00m, total);
            Assert.AreEqual(0.00m, discount);
        }

        [TestMethod]
        public void CalculateTotal_WithMultipleItems_AppliesCorrectDiscount()
        {
            // Arrange
            var order = new Order();
            order.Items.Add((new MenuItem { Name = "Pizza", FoodType = "Food", Price = 10.00m }, 2)); // 20.00
            order.Items.Add((new MenuItem { Name = "Tea", FoodType = "Drink", Price = 5.00m }, 1));    // 5.00

            // Act
            decimal total = order.CalculateTotal(out decimal discount);

            // Assert
            Assert.AreEqual(20.00m, total); // 20% discount on 25.00
            Assert.AreEqual(5.00m, discount);
        }
    }
}
