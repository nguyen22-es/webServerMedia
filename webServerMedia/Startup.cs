
using Common.Entities;
using Common.Services;
using Database.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webServerMedia.Services;


namespace webServerMedia
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
            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDefaultIdentity<VODUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<VODContext>();

            #region JWT Token Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var signingKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["Jwt:SigningSecret"]));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero
                };
                options.RequireHttpsMetadata = false;
            });
            #endregion

            services.AddAuthorization(options =>
            {
                options.AddPolicy("VODUser", policy => policy.RequireClaim("VODUser", "true"));
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin", "true"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddAutoMapper(); // Version 6.0.0: AutoMapper.Extensions.Microsoft.DependencyInjection

            // Version 6.1.0: AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper(typeof(Startup), typeof(Topic), typeof(Module), typeof(Media), typeof(Download));
            services.AddSwaggerGen();
            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IDbWriteService, DbWriteService>();
            services.AddScoped<IAdminService, AdminEFService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
