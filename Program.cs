using BlackJack.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace BlackJack
{
    public class Program
    {
        public static Program program;
        private ProgramController controller;
        private Dictionary<string, Game> games;
        private List<Player> players;

        public Program(string[] args)
        {
            games = new Dictionary<string, Game>();
            players = new List<Player>();
            controller = new ProgramController();
            start(args);
        }

        public int loginPlayer(String username, String currentGameId, int wallet)
        {
            Player player = new Player(username, "", 0);
            players.Add(player);
            return players.Count();
        }

        public void start(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               options.LoginPath = "/Account/Login"; // Pfad zum Login-Endpunkt
               options.LogoutPath = "/Account/Logout"; // Pfad zum Logout-Endpunkt
           });
            




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();

            app.UseRouting();
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<GameHub>("/GameHub");

            app.Run();
        }


        public static void Main(string[] args)
        {
            program = new Program(args);
        }
    }
}



/*
using BlackJack.Gaming;
using BlackJack.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<ChatHub>("/chatHub");
app.MapHub<GameHub>("/GameHub");

GameController controller = new GameController();


app.Run();
*/