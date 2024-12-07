﻿using System;
using Azure.Core;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MarketPlace.Data.Services
{
    
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;
        protected UserManager<User> _userManager;
        protected SignInManager<User> _signInManager;
        private IEmailService _emailService;
        
        public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService,
        IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;

            var actionContext = new ActionContext(
        _httpContextAccessor.HttpContext,
        _httpContextAccessor.HttpContext.GetRouteData(),
        new ActionDescriptor());

    _urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
        }
        public Task<bool> ChangeAccountStatus(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool Success, List<string> message)> CreateUserAsync(User Userdto ,   string password , IFormFile profilePicture = null)
        {
            List<string> message = new List<string>();  
            if(Userdto == null)
            {
                message.Add("User cannot be null");
                return (false, message);
            }
            var result = await _userManager.CreateAsync(Userdto , password);
            if(result.Succeeded)
            {
                await  _userManager.AddToRoleAsync(Userdto, "Buyer");
                message.Add("User created successfully");
                return (true, message);
            }
            else if(profilePicture != null && result.Succeeded)
            {
                await UpdateProfilePictureAsync(Userdto.Id, profilePicture);
                message.Add("User created successfully");
                return (true, message);
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    message.Add(error.Description);
                }
                return (false, message);
            }
        }
        public async Task<(bool Success, List<string> message)> SignInAsync(string Username, string password)
        {
            List<string> message = new List<string>();
            var user = await _userManager.FindByNameAsync(Username);
            if(user == null)
            {
                message.Add("No user found with the provided email address");
                return (false, message);
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if(result.Succeeded)
            {
                message.Add("User signed in successfully");
                return (true,message);
            }
            else
            {
                message.Add( "Incorrect email and password combination");
                return (false,message);
            }
        }

        public Task<User> GetUserWithAddressAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestAccountDeletionAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> UpdateProfilePictureAsync(string userId, IFormFile profilePicture)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return (false, new List<string> { "User Id cannot be null" });
            }
            using(var memoryStream = new MemoryStream())
            {
                await profilePicture.CopyToAsync(memoryStream);
                user.ProfilePicture = memoryStream.ToArray();
            }

            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return (true, null);
            }
            else
            {
                return (false, result.Errors.Select(e => e.Description));
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public Task<bool> UpdateUserDetailsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SignOutAsync(string Username)
        {
            if(Username == null)
            {
                return  false;
            }
            await _signInManager.SignOutAsync();
            return true;
            
        }

        public async Task<(bool Success, List<string> error)> SendResetTokenAsync(string email)
        {
            List<string> errors = new List<string>();
            var user = await _userManager.FindByEmailAsync(email);

            if(user == null){
                return (false, new List<string> { "User not found" });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = _urlHelper.Action("ResetPassword","Account", new {token }, protocol: "https");
            var subject = "Password Reset Request";
            var message = $"<p>Click the link below to reset your password:</p> <a href='{resetLink}'>Reset Password</a>";

            var result = await _emailService.SendEmailAsync(user.Email, subject, message);

            if(result){
                return (true,null);
            }
            else
            {
                errors.Add("Failed to send email");
                return (false, errors);
            }

        }
        public async Task<bool> ResetPasswordAsync(string email,string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return false;
            }
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

    }
}
