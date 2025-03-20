using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PriceComparisonMVCAdmin.Models.Configuration;

namespace PriceComparisonMVCAdmin.Infrastructure.DependencyInjection
{
    public static class AddAuthExtension
    {
        public static void AddAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }
            ).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),

                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    
                    {
                        // Get token from the cookies if it wasn't sent in header
                        var token = context.Request.Cookies["token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.Redirect("/Home/NoAccess");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.Response.Redirect("/Account/Login");
                        context.HandleResponse(); // обов'язково, щоб зупинити стандартну обробку
                        return Task.CompletedTask;
                    },
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRights", policy =>
                    policy.RequireRole(Role.Admin));
                options.AddPolicy("StandardRights", policy =>
                    policy.RequireRole(Role.User, Role.Admin, Role.Seller));
                options.AddPolicy("SellerRights", policy =>
                    policy.RequireRole(Role.Seller, Role.Admin));
            });
        }
    }
}
