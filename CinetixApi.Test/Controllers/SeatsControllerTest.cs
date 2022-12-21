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
    public class SeatsControllerTest
    {
        private DbContextOptions<SeatContext> options;
        private SeatContext seatContext;
        private SeatsController seatsController;
        private Seat seat1;
        private Seat seat2;

        public SeatsControllerTest()
        {
            options = new DbContextOptionsBuilder<SeatContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            seatContext = new SeatContext(options);
            seatContext.Database.EnsureCreated();
            seatsController = new SeatsController(seatContext);
            seat1 = new Seat("Seat-1", false, DateTime.Now);
            seat2 = new Seat("Seat-2", false, DateTime.Now);
        }

        [TestMethod]
        public void GetSeats_Should_Return_Seats()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.Seats.Add(seat2);
            seatContext.SaveChanges();
            // Act
            var response = seatsController.GetSeats().Result.Value;
            // Assert
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual(true, response.Contains(seat1));
            Assert.AreEqual(true, response.Contains(seat2));
        }

        [TestMethod]
        public void GetSeat_Should_Return_Seat()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.SaveChanges();
            // Act
            var response = seatsController.GetSeat(1).Result.Value;
            // Assert
            Assert.AreEqual(seat1, response);
        }

        [TestMethod]
        public void GetSeat_Should_Return_NotFound()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.SaveChanges();
            // Act
            var response = seatsController.GetSeat(2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutSeat_Should_Return_Seat()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.SaveChanges();
            seat1.SeatNumber = "Seat 101";
            seat1.IsBooked = true;
            // Act
            var putResponse = seatsController.PutSeat(1, seat1);
            var getResponse = seatsController.GetSeat(1);
            // Assert
            Assert.AreEqual(seat1, putResponse.Result.Value);
            Assert.AreEqual(seat1, getResponse.Result.Value);
        }

        [TestMethod]
        public void PutSeat_Should_Return_BadRequest()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.SaveChanges();
            seat1.SeatNumber = "Seat 101";
            seat1.IsBooked = true;
            // Act
            var putResponse = seatsController.PutSeat(2, seat1);
            // Assert
            BadRequestResult actual = putResponse.Result.Result as BadRequestResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual(null, putResponse.Result.Value);
        }

        [TestMethod]
        public void PutSeat_Should_Return_NotFound()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.SaveChanges();
            seat1.SeatNumber = "Seat 101";
            seat1.IsBooked = true;
            seat2.Id = 2;
            // Act
            var response = seatsController.PutSeat(2, seat2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PostSeat_Should_Return_Seat()
        {
            // Arrange
            // Act
            var postResponse = seatsController.PostSeat(seat1);
            var getResponse = seatsController.GetSeat(1);
            // Assert
            CreatedAtActionResult actual = postResponse.Result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.Created, actual.StatusCode);
            Assert.AreEqual(seat1, actual.Value);
            Assert.AreEqual(seat1, getResponse.Result.Value);
        }

        [TestMethod]
        public void DeleteSeat_Should_Return_NoContent()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.SaveChanges();
            // Act
            var deleteResponse = seatsController.DeleteSeat(1);
            var getReponse = seatsController.GetSeat(1);
            // Assert
            NoContentResult actual = deleteResponse.Result as NoContentResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NoContent, actual.StatusCode);

            NotFoundResult actualSeat1 = getReponse.Result.Result as NotFoundResult;
            Assert.IsNotNull(actualSeat1);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actualSeat1.StatusCode);
            Assert.AreEqual(null, getReponse.Result.Value);
        }

        [TestMethod]
        public void DeleteSeat_Should_Return_NotFound()
        {
            // Arrange
            seatContext.Seats.Add(seat1);
            seatContext.SaveChanges();
            // Act
            var deleteResponse = seatsController.DeleteSeat(2);
            var getResponse = seatsController.GetSeat(1);
            // Assert
            NotFoundResult actual = deleteResponse.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);

            Assert.AreEqual(seat1, getResponse.Result.Value);
        }
    }
}