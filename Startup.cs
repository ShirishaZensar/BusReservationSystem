using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusReservation.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bus.DataLayer;
using Bus.RepositoryContract;
using Bus.RepositoryLayer;

namespace BusReservation
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddDbContext<BusDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Conn")));

			services.AddScoped<IBusRepository,BusRepository>();
			services.AddScoped<IRouteRepository, RouteRepository>();
			services.AddScoped<IScheduleRepository, ScheduleRepository>();

			services.AddAuthentication().AddFacebook(options =>
			{
				options.AppId = "187093706156409";
				options.AppSecret = "049b2b92cd077ccb8dc95421d4fb24ee";
			});

			services.AddAuthentication().AddGoogle(options =>
			{
				options.ClientId = "792508201999-djp45kmhobsoodim4olihk03kdnnk6jl.apps.googleusercontent.com";
				options.ClientSecret = "TFOz-WCfPHtX57URrhwByZy4";
			});

			//services.AddAuthentication().AddLinkedin(options =>
			//{
			//	options.ClientId = "86jolpqxp63ws2";
			//	options.ClientSecret = "qEQHHiOxBJPDKOfJ";
			//});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
