using RecipeMvc.Data;
using RecipeMvc.Domain;
using Microsoft.EntityFrameworkCore;

namespace RecipeMvc.Infra
{
    public sealed class UserAccountRepo(DbContext db)
        : Repo<UserAccount, UserAccountData>(db, d => new(d)), IUserAccountRepo
    {
        protected internal override string selectTextField => nameof(UserAccountData.Username);
    }
}
