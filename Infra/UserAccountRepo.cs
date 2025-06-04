using RecipeMvc.Data.Data;
using RecipeMvc.Domain;
using Microsoft.EntityFrameworkCore;
using RecipeMvc.Data;

namespace RecipeMvc.Infra
{
    public sealed class UserAccountRepo(ApplicationDbContext db)
        : Repo<UserAccount, UserAccountData>(db, d => new(d)), IUserAccountRepo
    {
        protected internal override string selectTextField => nameof(UserAccountData.Username);

        public async Task<UserAccount?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var users = await GetAsync();
            return users.FirstOrDefault(u =>
                u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public async Task<bool> ExistsAsync(string? username, string? email, int? excludeUserId = null)
        {
            var users = await GetAsync();
            return users.Any(u =>
                ((username != null && u.Username == username) ||
                 (email != null && u.Email == email)) &&
                (!excludeUserId.HasValue || u.Id != excludeUserId.Value)
            );
        }

        public async Task UpdateAsync(UserAccount o)
        {
            var d = o?.Data;
            if (d is null) return;

            var tracked = await db.UserAccounts.FindAsync(d.Id);
            if (tracked is not null)
            {
                db.Entry(tracked).CurrentValues.SetValues(d);
            }
            else
            {
                db.UserAccounts.Update(d);
            }
            await db.SaveChangesAsync();
        }
    }
}