using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PriceComparisonMVC.Models.Configuration;

namespace PriceComparisonMVC.Infrastructure.DependencyInjection
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
                    OnTokenValidated = context =>
                    {
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
                            if (usernameClaim != null)
                            {
                                identity.AddClaim(new Claim("username", usernameClaim.Value));
                            }
                        }
                        return Task.CompletedTask;
                    }
                };

            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRights", policy =>
                    policy.RequireRole(Role.Admin));
                options.AddPolicy("StandardRights", policy =>
                    policy.RequireRole(Role.Admin, Role.User, Role.Seller));
                options.AddPolicy("SellerRights", policy =>
                    policy.RequireRole(Role.Seller));
            });
        }
    }
}
