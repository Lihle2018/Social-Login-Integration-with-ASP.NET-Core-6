using Online_Election_System.Data;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication();
builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {

	   options.ClientId = builder.Configuration["ExternalProviders:Google:ClientId"];
	   options.ClientSecret = builder.Configuration["ExternalProviders:Google:ClientSecret"];
   })
   .AddFacebook(options =>
   {

	   options.ClientId = builder.Configuration["ExternalProviders:Facebook:ClientId"];
	   options.ClientSecret = builder.Configuration["ExternalProviders:Facebook:ClientSecret"];
   })
   .AddTwitter(twitterOptions =>
   {
	   twitterOptions.ConsumerKey = builder.Configuration["ExternalProviders:Twitter:ConsumerKey"];
	   twitterOptions.ConsumerSecret = builder.Configuration["ExternalProviders:Twitter:ConsumerSecret"];
	   twitterOptions.RetrieveUserDetails = true;
   });
//Middleware
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();