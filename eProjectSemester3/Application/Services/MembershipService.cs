using eProjectSemester3.Application.Context;
using eProjectSemester3.Application.Entities;
using eProjectSemester3.Application.Utils;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eProjectSemester3.Application.Services
{
    public enum LoginAttemptStatus
    {
        LoginSuccessful,
        UserNotFound,
        PasswordIncorrect,
        PasswordAttemptsExceeded,
        UserLockedOut,
        UserNotApproved,
        Banned
    }

    public class MembershipService
    {
        public readonly AppDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipService(AppDbContext context)
        {
            _context = context;
        }
        

        /// <summary>
        /// Get a user by username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="removeTracking"></param>
        /// <returns></returns>
        public MembershipUser GetUser(string username, bool removeTracking = false)
        {
            username = StringUtils.SafePlainText(username);

            //var cacheKey = string.Concat(CacheKeys.Member.StartsWith, "GetUser-", username, "-", removeTracking);
            //return _cacheService.CachePerRequest(cacheKey, () =>
            //{
                MembershipUser member;
            
                if (removeTracking)
                {
                    member = _context.MembershipUser
                        .Include(x => x.Roles)
                        .AsNoTracking()
                        .FirstOrDefault(name => name.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase));
                }
                else
                {
                    member = _context.MembershipUser
                        .Include(x => x.Roles)
                        .FirstOrDefault(name => name.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase));
                }
                
                return member;
            //});
        }

        /// <summary>
        /// Get a roles by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string[] GetRolesForUser(string username)
        {
            //username = StringUtils.SafePlainText(username);
            var roles = new List<string>();
            var user = GetUser(username, true);

            if (user != null)
            {
                roles.AddRange(user.Roles.Select(role => role.RoleName));
            }

            return roles.ToArray();
        }

        /// <summary>
        /// Return last login status
        /// </summary>
        public LoginAttemptStatus LastLoginStatus { get; private set; } = LoginAttemptStatus.LoginSuccessful;
        
        /// <summary>
        /// Validate a user by password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="maxInvalidPasswordAttempts"> </param>
        /// <returns></returns>
        public bool ValidateUser(string userName, string password, int maxInvalidPasswordAttempts)
        {
            userName = StringUtils.SafePlainText(userName);
            password = StringUtils.SafePlainText(password);

            LastLoginStatus = LoginAttemptStatus.LoginSuccessful;

            var user = GetUser(userName);

            if (user == null)
            {
                LastLoginStatus = LoginAttemptStatus.UserNotFound;
                return false;
            }

            if (user.IsBanned)
            {
                LastLoginStatus = LoginAttemptStatus.Banned;
                return false;
            }

            if (user.IsLockedOut)
            {
                LastLoginStatus = LoginAttemptStatus.UserLockedOut;
                return false;
            }

            if (!user.IsApproved)
            {
                LastLoginStatus = LoginAttemptStatus.UserNotApproved;
                return false;
            }

            var allowedPasswordAttempts = maxInvalidPasswordAttempts;
            if (user.FailedPasswordAttemptCount >= allowedPasswordAttempts)
            {
                LastLoginStatus = LoginAttemptStatus.PasswordAttemptsExceeded;
                return false;
            }

            var salt = user.PasswordSalt;
            var hash = StringUtils.GenerateSaltedHash(password, salt);
            var passwordMatches = hash == user.Password;

            user.FailedPasswordAttemptCount = passwordMatches ? 0 : user.FailedPasswordAttemptCount + 1;

            if (user.FailedPasswordAttemptCount >= allowedPasswordAttempts)
            {
                user.IsLockedOut = true;
                user.LastLockoutDate = DateTime.UtcNow;
            }

            if (!passwordMatches)
            {
                LastLoginStatus = LoginAttemptStatus.PasswordIncorrect;
                return false;
            }

            return LastLoginStatus == LoginAttemptStatus.LoginSuccessful;
        }
    }
}