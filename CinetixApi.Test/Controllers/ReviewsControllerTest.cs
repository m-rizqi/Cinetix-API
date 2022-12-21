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
    public class ReviewsControllerTest
    {
        private DbContextOptions<ReviewContext> options;
        private ReviewContext reviewContext;
        private ReviewsController reviewsController;
        private Review review1;
        private Review review2;

        public ReviewsControllerTest()
        {
            options = new DbContextOptionsBuilder<ReviewContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            reviewContext = new ReviewContext(options);
            reviewContext.Database.EnsureCreated();
            reviewsController = new ReviewsController(reviewContext);
            review1 = new Review("Author 1", "Lorem Ipsum 1");
            review2 = new Review("Author 2", "Lorem Ipsum 2");
        }

        [TestMethod]
        public void GetReviews_Should_Return_Reviews()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.Reviews.Add(review2);
            reviewContext.SaveChanges();
            // Act
            var response = reviewsController.GetReviews().Result.Value;
            // Assert
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual(true, response.Contains(review1));
            Assert.AreEqual(true, response.Contains(review2));
        }

        [TestMethod]
        public void GetReview_Should_Return_Review()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.SaveChanges();
            // Act
            var response = reviewsController.GetReview(1).Result.Value;
            // Assert
            Assert.AreEqual(review1, response);
        }

        [TestMethod]
        public void GetReview_Should_Return_NotFound()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.SaveChanges();
            // Act
            var response = reviewsController.GetReview(2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutReview_Should_Return_Review()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.SaveChanges();
            review1.Author = "Author 101";
            review1.Resume = "Lorem Ipsum 101";
            // Act
            var putResponse = reviewsController.PutReview(1, review1);
            var getResponse = reviewsController.GetReview(1);
            // Assert
            Assert.AreEqual(review1, putResponse.Result.Value);
            Assert.AreEqual(review1, getResponse.Result.Value);
        }

        [TestMethod]
        public void PutReview_Should_Return_BadRequest()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.SaveChanges();
            review1.Author = "Author 101";
            review1.Resume = "Lorem Ipsum 101";
            // Act
            var putResponse = reviewsController.PutReview(2, review1);
            // Assert
            BadRequestResult actual = putResponse.Result.Result as BadRequestResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual(null, putResponse.Result.Value);
        }

        [TestMethod]
        public void PutReview_Should_Return_NotFound()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.SaveChanges();
            review1.Author = "Author 101";
            review1.Resume = "Lorem Ipsum 101";
            review2.Id = 2;
            // Act
            var response = reviewsController.PutReview(2, review2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PostReview_Should_Return_Review()
        {
            // Arrange
            // Act
            var postResponse = reviewsController.PostReview(review1);
            var getResponse = reviewsController.GetReview(1);
            // Assert
            CreatedAtActionResult actual = postResponse.Result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.Created, actual.StatusCode);
            Assert.AreEqual(review1, actual.Value);
            Assert.AreEqual(review1, getResponse.Result.Value);
        }

        [TestMethod]
        public void DeleteReview_Should_Return_NoContent()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.SaveChanges();
            // Act
            var deleteResponse = reviewsController.DeleteReview(1);
            var getReponse = reviewsController.GetReview(1);
            // Assert
            NoContentResult actual = deleteResponse.Result as NoContentResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actual.StatusCode);

            NotFoundResult actualReview1 = getReponse.Result.Result as NotFoundResult;
            Assert.IsNotNull(actualReview1);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actualReview1.StatusCode);
            Assert.AreEqual(null, getReponse.Result.Value);
        }

        [TestMethod]
        public void DeleteReview_Should_Return_NotFound()
        {
            // Arrange
            reviewContext.Reviews.Add(review1);
            reviewContext.SaveChanges();
            // Act
            var deleteResponse = reviewsController.DeleteReview(2);
            var getResponse = reviewsController.GetReview(1);
            // Assert
            NotFoundResult actual = deleteResponse.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);

            Assert.AreEqual(review1, getResponse.Result.Value);
        }
    }
}