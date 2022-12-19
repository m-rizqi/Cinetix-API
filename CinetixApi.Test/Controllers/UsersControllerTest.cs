using Cinetix_Api.Context;
using Cinetix_Api.Controllers;
using Cinetix_Api.Models;
using Cinetix_Api.Request;
using Cinetix_Api.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Cinetix_Api.Test.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        private DbContextOptions<UserContext> options;
        private UserContext userContext;
        private UsersController usersController;

        public UsersControllerTest()
        {
            options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            userContext = new UserContext(options);
            userContext.Database.EnsureCreated();
            usersController = new UsersController(userContext);
        }

        [TestMethod]
        public void GetUsers_Should_Return_Users()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            var user2 = new User(2, "B", "b@email.com", "password");
            var user3 = new User(3, "C", "c@email.com", "password");
            // Act
            userContext.Users.Add(user1);
            userContext.Users.Add(user2);
            userContext.Users.Add(user3);
            userContext.SaveChanges();
            var response = usersController.GetUsers();
            // Assert
            Assert.AreEqual(3, response.Result.Value.Count());
        }

        [TestMethod]
        public void GetUser_Should_Return_User()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            // Act
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var response = usersController.GetUser(1);
            // Assert
            Assert.AreEqual(user1, response.Result.Value);
        }

        [TestMethod]
        public void GetUser_Should_Return_NotFound()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            // Act
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var response = usersController.GetUser(2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutUser_Should_Return_User()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            user1.Fullname = "B";
            user1.Fullname = "b@email.com";
            user1.Fullname = "b_Password";
            // Act
            var response = usersController.PutUser(1, user1);
            // Assert
            Assert.AreEqual(user1, response.Result.Value);
        }
        [TestMethod]
        public void PutUser_Should_Return_BadRequest()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            user1.Fullname = "B";
            user1.Fullname = "b@email.com";
            user1.Fullname = "b_Password";
            // Act
            var response = usersController.PutUser(2, user1);
            // Assert
            BadRequestResult actual = response.Result.Result as BadRequestResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutUser_Should_Return_NotFound()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            var user2 = new User(2, "B", "b@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();            
            // Act
            var response = usersController.PutUser(2, user2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PostUser_Should_Return_User()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            // Act
            var response = usersController.PostUser(user1);
            // Assert
            CreatedAtActionResult actual = response.Result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.Created, actual.StatusCode);
            Assert.AreEqual(user1, actual.Value);
        }

        [TestMethod]
        public void Delete_Should_Return_NoContent()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            // Act
            var response = usersController.DeleteUser(1);
            var getUser1FromDb = usersController.GetUser(1);
            // Assert
            NoContentResult actual = response.Result as NoContentResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actual.StatusCode);

            NotFoundResult actualUser1 = getUser1FromDb.Result.Result as NotFoundResult;
            Assert.IsNotNull(actualUser1);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actualUser1.StatusCode);
            Assert.AreEqual(null, getUser1FromDb.Result.Value);
        }

        [TestMethod]
        public void Delete_Should_Return_NotFound()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            // Act
            var response = usersController.DeleteUser(2);
            var getUser1FromDb = usersController.GetUser(1);
            // Assert
            NotFoundResult actual = response.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);

            Assert.AreEqual(user1, getUser1FromDb.Result.Value);
        }

        [TestMethod]
        public void Login_Should_Return_ErrorResponse_Email_Not_valid()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("email", "Password123.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Email not valid", actual.message);
        }

        [TestMethod]
        public void Login_Should_Return_ErrorResponse_Password_Not_Contain_Number()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("a@email.com", "Password.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Password not valid. It must contain at least a number, an upper case letter, and 8 characters long", actual.message);
        }

        [TestMethod]
        public void Login_Should_Return_ErrorResponse_Password_Not_Contain_Capital()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("a@email.com", "password123.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Password not valid. It must contain at least a number, an upper case letter, and 8 characters long", actual.message);
        }

        [TestMethod]
        public void Login_Should_Return_ErrorResponse_Password_Length_Less_Than_8()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("a@email.com", "p1.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Password not valid. It must contain at least a number, an upper case letter, and 8 characters long", actual.message);
        }

        [TestMethod]
        public void Login_Should_Return_NotFound_Email()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "Password123.");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("b@email.com", "Password123.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void Login_Should_Return_NotFound_Password()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "Password123.");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("a@email.com", "Password1234.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void Login_Should_Return_NotFound_Email_Password()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "Password123.");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("b@email.com", "Password1234.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void Login_Should_Return_User()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "Password123.");
            userContext.Users.Add(user1);
            userContext.SaveChanges();
            var loginRequest = new LoginRequest("a@email.com", "Password123.");
            // Act
            var response = usersController.Login(loginRequest);
            // Assert
            Assert.AreEqual(user1, response.Result.Value);
        }

        [TestMethod]
        public void Register_Should_Return_ErrorResponse_Email_Not_valid()
        {
            // Arrange
            var user1 = new User("A", "email", "password123.");
            // Act
            var response = usersController.Register(user1);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Email not valid", actual.message);
        }

        [TestMethod]
        public void Register_Should_Return_ErrorResponse_Password_Not_Contain_Number()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "Password");
            // Act
            var response = usersController.Register(user1);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Password not valid. It must contain at least a number, an upper case letter, and 8 characters long", actual.message);
        }

        [TestMethod]
        public void Register_Should_Return_ErrorResponse_Password_Not_Contain_Capital()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "password123");
            // Act
            var response = usersController.Register(user1);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Password not valid. It must contain at least a number, an upper case letter, and 8 characters long", actual.message);
        }

        [TestMethod]
        public void Register_Should_Return_ErrorResponse_Password_Length_Less_Than_8()
        {
            // Arrange
            var user1 = new User(1, "A", "a@email.com", "Pa12");
            // Act
            var response = usersController.Register(user1);
            // Assert
            ErrorResponse<User> actual = response.Result.Result as ErrorResponse<User>;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (int)actual.statusCode);
            Assert.AreEqual("Password not valid. It must contain at least a number, an upper case letter, and 8 characters long", actual.message);
        }

        [TestMethod]
        public void Register_Should_Return_User()
        {
            // Arrange
            var user1 = new User("A", "a@email.com", "Password123");
            // Act
            var response = usersController.Register(user1);
            // Assert
            CreatedAtActionResult actual = response.Result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.Created, actual.StatusCode);
            Assert.AreEqual(user1, actual.Value);
        }
    }
}