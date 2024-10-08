using System;
using System.Linq;
using Xunit;

namespace LAB3.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_EmptyFile_ShouldThrowException()
        {
            // Simulate an empty file (no lines)
            string[] lines = Array.Empty<string>();

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The input file must contain at least one line.", ex.Message);
        }

        [Fact]
        public void Test_InvalidNumber_ShouldThrowException()
        {
            // Simulate an invalid number in input
            string[] lines = { "abc" };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The first line must contain a valid natural number.", ex.Message);
        }

        [Fact]
        public void Test_NumberOutOfBounds_ShouldThrowException()
        {
            // Simulate a number that is out of the allowed bounds
            string[] lines = { "20" };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The number N must be between 1 and 18.", ex.Message);
        }

        [Fact]
        public void Test_IncorrectNumberOfRows_ShouldThrowException()
        {
            // Simulate an input with wrong number of rows (should be N+1 rows)
            string[] lines = {
                "3",
                "0 1 0",
                "1 0 1"
            };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The number of lines must match N + 1.", ex.Message);
        }

        [Fact]
        public void Test_IncorrectNumberOfElementsInRow_ShouldThrowException()
        {
            // Simulate an input where a row doesn't contain N elements
            string[] lines = {
                "3",
                "0 1 0",
                "1 0 1 0", // Incorrect row size
                "0 1 0"
            };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Line 2 must contain exactly 3 elements.", ex.Message);
        }

        [Fact]
        public void Test_InvalidElement_ShouldThrowException()
        {
            // Simulate an input with invalid element (not 0 or 1)
            string[] lines = {
                "2",
                "0 1",
                "1 2" // Invalid element
            };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("Each element in line 2 must be either 0 or 1.", ex.Message);
        }

        [Fact]
        public void Test_FindMinCages_SingleAnimal_ShouldReturnOne()
        {
            // Single animal, should require 1 cage
            int N = 1;
            int[][] matrix = new int[][] {
                new int[] { 0 }
            };

            int result = Program.FindMinCages(N, matrix);

            Assert.Equal(1, result); // Only 1 animal, 1 cage needed
        }

        [Fact]
        public void Test_FindMinCages_AllCompatible_ShouldReturnOne()
        {
            // All animals are compatible
            int N = 3;
            int[][] matrix = new int[][] {
                new int[] { 0, 0, 0 },
                new int[] { 0, 0, 0 },
                new int[] { 0, 0, 0 }
            };

            int result = Program.FindMinCages(N, matrix);

            Assert.Equal(1, result); // All can be in 1 cage
        }

        [Fact]
        public void Test_FindMinCages_AllIncompatible_ShouldReturnN()
        {
            // All animals are incompatible with each other
            int N = 3;
            int[][] matrix = new int[][] {
                new int[] { 0, 1, 1 },
                new int[] { 1, 0, 1 },
                new int[] { 1, 1, 0 }
            };

            int result = Program.FindMinCages(N, matrix);

            Assert.Equal(3, result); // 3 animals, no compatibility, 3 cages needed
        }

        [Fact]
        public void Test_FindMinCages_MixedCompatibility_ShouldReturnTwo()
        {
            // Some animals are compatible, others not
            int N = 3;
            int[][] matrix = new int[][] {
                new int[] { 0, 1, 0 },
                new int[] { 1, 0, 1 },
                new int[] { 0, 1, 0 }
            };

            int result = Program.FindMinCages(N, matrix);

            Assert.Equal(2, result); // Can fit some in 1 cage, others need separate cage
        }
    }
}