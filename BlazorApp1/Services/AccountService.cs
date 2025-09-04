using BlazorApp1.Data;
using BlazorApp1.Models;
using BlazorApp1.Pages;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Security.Claims;

namespace BlazorApp1.Services
{
    public class AccountService : DbContext
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        protected readonly ApplicationDbContext _context;
        public AccountService(ApplicationDbContext context,AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _context = context;
        }
        public async Task<Guid> GetCurrentUserId()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userId);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

         
        public void AddAccount(string code, string description, int orderBy, Guid userId)
        {
            Account account = new Account();
            account.Code = code;
            account.Description = description;
            account.DateCreated = DateTime.Now;
            account.OrderBy = orderBy;
            account.UserId = userId;
            account.Amount = 0;
            account.IsDeleted = false;

            _context.Add(account);
            _context.SaveChanges();

        }

        public void SaveAccount(Guid userId, int accountId, String description, int orderBy, bool isDeleted)
        {
            Account account = GetAccount(userId, accountId);
            account.Description = description;
            account.OrderBy = orderBy;
            account.IsDeleted = isDeleted;
            _context.SaveChanges();

        }

        public List<Account> GetAccounts(Guid userId)
        {

            return _context.Accounts.AsNoTracking()
                .Where(x => x.UserId == userId && x.IsDeleted==false).OrderBy(x => x.OrderBy).ToList();

        }

        public Account GetAccount(Guid userId, int accountId)
        {

            return _context.Accounts
                .Where(x => x.UserId == userId && x.Id == accountId).FirstOrDefault();

        }

        public void Deposit(int id, decimal amount, Guid userId)
        {
            var account = _context.Accounts
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (account != null)
            {
                account.Amount += amount;
            }

            _context.SaveChanges();

        }

        public void Withdraw(int id, decimal amount, Guid userId)
        {
            var account = _context.Accounts
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

            if (account != null)
            {
                account.Amount = account.Amount - amount;
            }

            _context.SaveChanges();

        }

        public void LogAudit(int accountId, decimal amount, Guid userId, string type, string description)
        {
            Audit a = new Audit();
            a.AccountID = accountId;
            a.DateCreated = DateTime.Now;
            a.Type = type;
            a.UserId = userId;
            a.Description = description;

            _context.Add(a);
            _context.SaveChanges();


        }

        public List<Audit> GetLogAudit(Guid userId)
        {
            return _context.Audits.AsNoTracking()
               .Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).ToList();
        }
    }
}

    
