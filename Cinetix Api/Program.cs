using Cinetix_Api.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Inject Db Context
builder.Services.AddDbContext<CinemaContext>(opt =>
    opt.UseInMemoryDatabase("Cinemas"));
builder.Services.AddDbContext<GenreContext>(opt =>
    opt.UseInMemoryDatabase("Genres"));
builder.Services.AddDbContext<MovieContext>(opt =>
    opt.UseInMemoryDatabase("Movies"));
builder.Services.AddDbContext<ReviewContext>(opt =>
    opt.UseInMemoryDatabase("Reviews"));
builder.Services.AddDbContext<SeatContext>(opt =>
    opt.UseInMemoryDatabase("Seats"));
builder.Services.AddDbContext<TicketContext>(opt =>
    opt.UseInMemoryDatabase("Tickets"));
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseInMemoryDatabase("Users"));
builder.Services.AddDbContext<MovieWithCinemasContext>(opt =>
    opt.UseInMemoryDatabase("MovieWithCinemasContext"));
builder.Services.AddDbContext<MovieWithGenresContext>(opt =>
    opt.UseInMemoryDatabase("MovieWithGenresContext"));
builder.Services.AddDbContext<MovieWithReviewsContext>(opt =>
    opt.UseInMemoryDatabase("MovieWithReviewsContext"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
