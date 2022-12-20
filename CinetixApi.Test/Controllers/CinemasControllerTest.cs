using Cinetix_Api.Context;
using Cinetix_Api.Controllers;
using Cinetix_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cinetix_Api.Test.Controllers
{
    [TestClass]
    public class CinemasControllerTest
    {
        private DbContextOptions<CinemaContext> cinemaOptions;
        private DbContextOptions<SeatContext> seatOptions;
        private CinemaContext cinemaContext;
        private SeatContext seatContext;
        private CinemasController cinemasController;

        // Dummy Data
        private Cinema cinema1;
        private Cinema cinema2;
        private Seat seat1;
        private Seat seat2;
        private Seat seat3;
        private Seat seat4;
        public CinemasControllerTest()
        {
            cinemaOptions = new DbContextOptionsBuilder<CinemaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            seatOptions = new DbContextOptionsBuilder<SeatContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            cinemaContext = new CinemaContext(cinemaOptions);
            seatContext = new SeatContext(seatOptions);
            cinemaContext.Database.EnsureCreated();
            seatContext.Database.EnsureCreated();
            cinemasController = new CinemasController(cinemaContext, seatContext);

            // Initialize Dummy
            seat1 = new Seat("Seat-1", false, DateTime.Now);
            seat2 = new Seat("Seat-2", false, DateTime.Now);
            seat3 = new Seat("Seat-3", false, DateTime.Now);
            seat4 = new Seat("Seat-4", false, DateTime.Now);
            cinema1 = new Cinema("Cinema1");
            cinema2 = new Cinema("Cinema2");
        }

        [TestMethod]
        public void GetCinemas_Should_Return_Cinemas()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            seat3.CinemaId = 2;
            seat4.CinemaId = 2;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            cinema2.Seats = new List<Seat>(){
                seat3,
                seat4
            };
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            seatContext.Add(seat3);
            seatContext.Add(seat4);
            cinemaContext.Add(cinema1);
            cinemaContext.Add(cinema2);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            // Act
            var response = cinemasController.GetCinemas().Result.Value;
            // Assert
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual(2, response.ElementAt(0).Seats.Count());
            Assert.AreEqual(2, response.ElementAt(1).Seats.Count());
        }

        [TestMethod]
        public void GetCinema_Should_Return_Cinema()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            cinemaContext.Add(cinema1);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            // Act
            var response = cinemasController.GetCinema(1).Result.Value;
            // Assert
            Assert.AreEqual(cinema1, response);
            Assert.AreEqual(2, response.Seats.Count());
        }

        [TestMethod]
        public void GetCinema_Should_Return_NotFound()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            cinemaContext.Add(cinema1);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            // Act
            var response = cinemasController.GetCinema(2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutCinema_Should_Return_Cinema()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            cinemaContext.Add(cinema1);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            cinema1.Name = "Cinema21";
            cinema1.Seats.ElementAt(0).SeatNumber = "Seat-11";
            cinema1.Seats.ElementAt(1).SeatNumber = "Seat-22";
            // Act
            var putResponse = cinemasController.PutCinema(1, cinema1).Result.Value;
            var getResponse = cinemasController.GetCinema(1).Result.Value;
            // Assert
            Assert.AreEqual(cinema1, putResponse);
            Assert.AreEqual(cinema1, getResponse);
            Assert.AreEqual(true, getResponse.Seats.Contains(cinema1.Seats.ElementAt(0)));
            Assert.AreEqual(true, getResponse.Seats.Contains(cinema1.Seats.ElementAt(1)));
        }
        [TestMethod]
        public void PutCinema_Should_Return_BadRequest()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            cinemaContext.Add(cinema1);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            cinema1.Name = "Cinema21";
            cinema1.Seats.ElementAt(0).SeatNumber = "Seat-11";
            cinema1.Seats.ElementAt(1).SeatNumber = "Seat-22";
            // Act
            var response = cinemasController.PutCinema(2, cinema1);
            // Assert
            BadRequestResult actual = response.Result.Result as BadRequestResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PutCinema_Should_Return_NotFound()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            seat3.CinemaId = 2;
            seat4.CinemaId = 2;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            cinema2.Seats = new List<Seat>(){
                seat3,
                seat4
            };
            cinema2.Id = 2;
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            cinemaContext.Add(cinema1);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            // Act
            var response = cinemasController.PutCinema(2, cinema2);
            // Assert
            NotFoundResult actual = response.Result.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);
            Assert.AreEqual(null, response.Result.Value);
        }

        [TestMethod]
        public void PostCinema_Should_Return_Cinema()
        {
            // Arrange
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            // Act
            var postResponse = cinemasController.PostCinema(cinema1);
            var getResponse = cinemasController.GetCinema(1).Result.Value;
            // Assert
            CreatedAtActionResult actual = postResponse.Result.Result as CreatedAtActionResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.Created, actual.StatusCode);
            Assert.AreEqual(cinema1, actual.Value);
            Assert.AreEqual(cinema1, getResponse);
            Assert.AreEqual(2, getResponse.Seats.Count());
            Assert.AreEqual(true, getResponse.Seats.Contains(cinema1.Seats.ElementAt(0)));
            Assert.AreEqual(true, getResponse.Seats.Contains(cinema1.Seats.ElementAt(1)));
        }

        [TestMethod]
        public void DeleteGenre_Should_Return_NoContent()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            cinemaContext.Add(cinema1);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            // Act
            var deleteResponse = cinemasController.DeleteCinema(1);
            var getReponse = cinemasController.GetCinema(1);
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
        public void DeleteCinema_Should_Return_NotFound()
        {
            // Arrange
            seat1.CinemaId = 1;
            seat2.CinemaId = 1;
            cinema1.Seats = new List<Seat>
            {
                seat1,
                seat2
            };
            seatContext.Add(seat1);
            seatContext.Add(seat2);
            cinemaContext.Add(cinema1);
            seatContext.SaveChanges();
            cinemaContext.SaveChanges();
            // Act
            var deleteResponse = cinemasController.DeleteCinema(2);
            var getResponse = cinemasController.GetCinema(1);
            // Assert
            NotFoundResult actual = deleteResponse.Result as NotFoundResult;
            Assert.IsNotNull(actual);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actual.StatusCode);

            Assert.AreEqual(cinema1, getResponse.Result.Value);
        }
    }
}
