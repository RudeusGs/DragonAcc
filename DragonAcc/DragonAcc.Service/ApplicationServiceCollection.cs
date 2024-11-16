using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Common.Services;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;

namespace DragonAcc.Service
{
    public static class ApplicationServiceCollection
    {
        /// <summary>
        /// Adds the application services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            #region Common services
            // JWT
            services.AddScoped<IJwtService, JwtService>();

            // User Management Service.
            services.AddScoped<UserManager<User>>();
            services.AddScoped<SignInManager<User>, SignInManager<User>>();
            services.AddScoped<Common.IServices.IUserService, UserService>();

            // Ftp
            services.AddScoped<IFtpDirectoryService, FtpDirectoryService>();
            #endregion 
            services.AddHttpContextAccessor();
            #region Business services
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<ILol_GameService, Lol_GameService>();
            services.AddScoped<ITocChien_GameService, TocChien_GameService>();
            services.AddScoped<INgocRong_GameService, NgocRong_GameService>();
            services.AddScoped<IPubg_GameService, Pubg_GameService>();
            services.AddScoped<IValorant_GameService, Valorant_GameService>();
            services.AddScoped<IAccountWebsiteService, AccountWebsiteService>();
            services.AddScoped<IPurchasedAccountService, PurchasedAccountService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<ILuckyWheelListPrizeService, LuckyWheelListPrizeService>();
            services.AddScoped<ILuckyWheelService, LuckyWheelService>();
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IWithDrawMoneyService, WithDrawMoneyService>();
            services.AddScoped<IStatisticalService, StatisticalService>();
            services.AddScoped<INotificationService, NotificationService>();
            #endregion
            return services;
        }
    }
}
