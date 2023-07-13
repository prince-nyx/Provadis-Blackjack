using BlackJack.Hubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;

namespace BlackJack
{
    public class Program
    {
        public static Program program;
        private Dictionary<string, Game> games;
        private Dictionary<string, Player> players;
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public Program(string[] args)
        {
            games = new Dictionary<string, Game>();
            players = new Dictionary<string, Player>();
            start(args);
        }

        public String loginPlayer(String username, String currentGameId, int wallet)
        {
            Player player = new Player(GenerateRandomString(8),username, wallet, currentGameId);
            players.Add(player.id, player);
            return player.id;
        }

        public string GenerateRandomString(int length)
        {
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, Characters.Length);
                char randomChar = Characters[randomIndex];
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }

        public void start(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddControllersWithViews();
           // builder.Services.AddCookieManager();

            // or
            /*
            // Add CookieManager with options
            builder.Services.AddCookieManager(options =>
            {
                // Allow cookie data to encrypt by default it allow encryption
                options.AllowEncryption = false;
                // Throw if not all chunks of a cookie are available on a request for re-assembly.
                options.ThrowForPartialCookies = true;
                // Set null if not allow to devide in chunks
                options.ChunkSize = null;
                // Default Cookie expire time if expire time set to null of cookie
                // Default time is 1 day to expire cookie 
                options.DefaultExpireTimeInDays = 10;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                });
            */
            builder.Services.AddRazorPages();




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
            app.UseAuthorization();
            app.UseRouting();

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