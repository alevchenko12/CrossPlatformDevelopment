using System;
using Xunit;

namespace LAB2.Tests
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
            Assert.Equal("The input file must contain exactly one line.", ex.Message);
        }

        [Fact]
        public void Test_FileWithMultipleLines_ShouldThrowException()
        {
            // Simulate a file with multiple lines
            string[] lines = { "2", "3" };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The input file must contain exactly one line.", ex.Message);
        }

        [Fact]
        public void Test_NonNumericInput_ShouldThrowException()
        {
            // Line with non-numeric characters
            string[] lines = { "abc" };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The input string must be a valid natural number.", ex.Message);
        }

        [Fact]
        public void Test_InputExceedsLimit_ShouldThrowException()
        {
            // Simulate input larger than 1000
            string[] lines = { "1001" };

            var ex = Record.Exception(() => Program.ValidateInput(lines));
            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The number N must be a natural number between 1 and 1000.", ex.Message);
        }

        [Fact]
        public void Test_ValidInput_MinimumValue_ShouldReturnCorrectResult()
        {
            // Valid input for N = 1 (smallest valid input)
            string[] lines = { "1" };

            Program.ValidateInput(lines);
            int result = Program.SolveProblem(lines[0]);

            // Only one configuration possible for N=1
            Assert.Equal(1, result);
        }

        [Fact]
        public void Test_ValidInput_NEquals2_ShouldReturnCorrectResult()
        {
            // Valid input for N = 2
            string[] lines = { "2" };

            Program.ValidateInput(lines);
            int result = Program.SolveProblem(lines[0]);

            // For N = 2, the correct number of configurations is 4
            Assert.Equal(4, result);
        }

        [Fact]
        public void Test_ValidInput_NEquals3_ShouldReturnCorrectResult()
        {
            // Valid input for N = 3
            string[] lines = { "3" };

            Program.ValidateInput(lines);
            int result = Program.SolveProblem(lines[0]);

            // For N = 3, the correct number of configurations is 9
            Assert.Equal(9, result);
        }
    }
}
