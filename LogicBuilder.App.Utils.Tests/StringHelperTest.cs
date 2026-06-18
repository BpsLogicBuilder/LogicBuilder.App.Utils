namespace LogicBuilder.App.Utils.Tests
{
    public class StringHelperTest
    {
        private readonly StringHelper _stringHelper;

        public StringHelperTest()
        {
            _stringHelper = new StringHelper();
        }

        #region IsValidEmail Tests

        [Fact]
        public void IsValidEmail_WithValidEmail_ReturnsTrue()
        {
            // Arrange
            string email = "test@example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_WithValidEmailContainingNumbers_ReturnsTrue()
        {
            // Arrange
            string email = "user123@example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_WithValidEmailContainingDots_ReturnsTrue()
        {
            // Arrange
            string email = "user.name@example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_WithValidEmailContainingPlus_ReturnsTrue()
        {
            // Arrange
            string email = "user+tag@example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_WithValidEmailContainingSubdomain_ReturnsTrue()
        {
            // Arrange
            string email = "test@mail.example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_WithInvalidEmailNoAtSign_ReturnsFalse()
        {
            // Arrange
            string email = "testexample.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidEmail_WithInvalidEmailMultipleAtSigns_ReturnsFalse()
        {
            // Arrange
            string email = "test@@example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidEmail_WithInvalidEmailMissingDomain_ReturnsFalse()
        {
            // Arrange
            string email = "test@";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidEmail_WithInvalidEmailMissingLocalPart_ReturnsFalse()
        {
            // Arrange
            string email = "@example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidEmail_WithEmptyString_ReturnsFalse()
        {
            // Arrange
            string email = "";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidEmail_WithNull_ReturnsFalse()
        {
            // Arrange
            string email = null!;

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidEmail_WithWhitespace_ReturnsFalse()
        {
            // Arrange
            string email = "   ";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidEmail_WithSpacesInEmail_ReturnsFalse()
        {
            // Arrange
            string email = "test @example.com";

            // Act
            bool result = _stringHelper.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region StringIsNullOrEmpty Tests

        [Fact]
        public void StringIsNullOrEmpty_WithNull_ReturnsTrue()
        {
            // Arrange
            string value = null!;

            // Act
            bool result = _stringHelper.StringIsNullOrEmpty(value);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void StringIsNullOrEmpty_WithEmptyString_ReturnsTrue()
        {
            // Arrange
            string value = "";

            // Act
            bool result = _stringHelper.StringIsNullOrEmpty(value);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void StringIsNullOrEmpty_WithStringEmpty_ReturnsTrue()
        {
            // Arrange
            string value = string.Empty;

            // Act
            bool result = _stringHelper.StringIsNullOrEmpty(value);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void StringIsNullOrEmpty_WithWhitespace_ReturnsFalse()
        {
            // Arrange
            string value = "   ";

            // Act
            bool result = _stringHelper.StringIsNullOrEmpty(value);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StringIsNullOrEmpty_WithValidString_ReturnsFalse()
        {
            // Arrange
            string value = "test";

            // Act
            bool result = _stringHelper.StringIsNullOrEmpty(value);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StringIsNullOrEmpty_WithSingleCharacter_ReturnsFalse()
        {
            // Arrange
            string value = "a";

            // Act
            bool result = _stringHelper.StringIsNullOrEmpty(value);

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
