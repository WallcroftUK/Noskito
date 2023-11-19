using System.IO;
using System.Threading.Tasks;
using Noskito.Common.Extension;
using Noskito.Database.Dto.Accounts;
using Noskito.Database.Repository;
using Noskito.Logging;

namespace Noskito.Toolkit.Parser
{
    public class AccountParser : IParser
    {
        private readonly AccountRepository accountRepository;

        public AccountParser(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task Parse(DirectoryInfo directory)
        {
            await accountRepository.Add(new AccountDTO
            {
                Username = "admin",
                Password = "admin".ToSha512()
            });
            
            await accountRepository.Add(new AccountDTO
            {
                Username = "test",
                Password = "test".ToSha512()
            });
            
            Log.Info("Created admin & test accounts");
        }
    }
}