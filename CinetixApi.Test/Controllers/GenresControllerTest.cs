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
    public class GenresControllerTest
    {
        private DbContextOptions<GenreContext> options;
        private GenreContext genreContext;
        private GenresController genresController;

        public GenresControllerTest()
        {
            options = new DbContextOptionsBuilder<GenreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            genreContext = new GenreContext(options);
            genreContext.Database.EnsureCreated();
            genresController = new GenresController(genreContext);
        }

        [TestMethod]
        public void GetGenres_Should_Return_Genres()
        {
            // Arrange
            var genre1 = new Genre("Action");
            var genre2 = new Genre("Romance");
            var genre3 = new Genre("Comedy");
            // Act
            genreContext.Genres.Add(genre1);
            genreContext.Genres.Add(genre2);
            genreContext.Genres.Add(genre3);
            genreContext.SaveChanges();
            var response = genresController.GetGenres();
            // Assert
            Assert.AreEqual(3, response.Result.Value.Count());
        }

        [TestMethod]
        public void GetGenre_Should_Return_Genre()
        {
            // Arrange
            var genre1 = new Genre("Action");
            // Act
            genreContext.Genres.Add(genre1);
            genreContext.SaveChanges();
            var response = genresController.GetGenre(1);
            // Assert
            Assert.AreEqual(genre1, response.Result.Value);
        }

        [TestMethod]
        public void GetGenre_Should_Return_NotFound()
        {
            // Arrange
            var genre1 = new Genre("Action");
            // Act
            genreContext.Genres.Add(genre1);
            genreContext.SaveChanges();
            var response = genresController.GetGenre(2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutGenre_Should_Return_Genre()
        {
            // Arrange
            var genre1 = new Genre("Action");
            genreContext.Genres.Add(genre1);
            genreContext.SaveChanges();
            genre1.Name = "Comedy";
            // Act
            var putResponse = genresController.PutGenre(1, genre1);
            var getResponse = genresController.GetGenre(1);
            // Assert
            Assert.AreEqual(genre1, putResponse.Result.Value);
            Assert.AreEqual(genre1, getResponse.Result.Value);
        }
        [TestMethod]
        public void PutGenre_Should_Return_BadRequest()
        {
            // Arrange
            var genre1 = new Genre("Action");
            genreContext.Genres.Add(genre1);
            genreContext.SaveChanges();
            genre1.Name = "Science";
            // Act
            var response = genresController.PutGenre(2, genre1);
            // Assert
            BadRequestResult actual = response.Result.Result as BadRequestResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutGenre_Should_Return_NotFound()
        {
            // Arrange
            var genre1 = new Genre(1, "Action");
            var genre2 = new Genre(2, "Comedy");
            genreContext.Genres.Add(genre1);
            genreContext.SaveChanges();            
            // Act
            var response = genresController.PutGenre(2, genre2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PostGenre_Should_Return_Genre()
        {
            // Arrange
            var genre1 = new Genre("Action");
            // Act
            var postResponse = genresController.PostGenre(genre1);
            var getResponse = genresController.GetGenre(1);
            // Assert
            CreatedAtActionResult actual = postResponse.Result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.Created, actual.StatusCode);
            Assert.AreEqual(genre1, actual.Value);
            Assert.AreEqual(genre1, getResponse.Result.Value);
        }

        [TestMethod]
        public void DeleteGenre_Should_Return_NoContent()
        {
            // Arrange
            var genre1 = new Genre("Action");
            genreContext.Genres.Add(genre1);
            genreContext.SaveChanges();
            // Act
            var deleteResponse = genresController.DeleteGenre(1);
            var getReponse = genresController.GetGenre(1);
            // Assert
            NoContentResult actual = deleteResponse.Result as NoContentResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actual.StatusCode);

            NotFoundResult actualUser1 = getReponse.Result.Result as NotFoundResult;
            Assert.IsNotNull(actualUser1);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actualUser1.StatusCode);
            Assert.AreEqual(null, getReponse.Result.Value);
        }

        [TestMethod]
        public void DeleteGenre_Should_Return_NotFound()
        {
            // Arrange
            var genre1 = new Genre("Action");
            genreContext.Genres.Add(genre1);
            genreContext.SaveChanges();
            // Act
            var deleteResponse = genresController.DeleteGenre(2);
            var getResponse = genresController.GetGenre(1);
            // Assert
            NotFoundResult actual = deleteResponse.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);

            Assert.AreEqual(genre1, getResponse.Result.Value);
        }
    }
}