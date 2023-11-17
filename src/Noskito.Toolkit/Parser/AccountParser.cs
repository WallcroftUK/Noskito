using System.IO;
using System.Threading.Tasks;
using Noskito.Common.Extension;
using Noskito.Common.Logging;
using Noskito.Database.Dto;
using Noskito.Database.Repository;

namespace Noskito.Toolkit.Parser
{
    public class AccountParser : IParser
    {
        private readonly ILogger logger;
        private readonly AccountRepository accountRepository;

        public AccountParser(ILogger logger, AccountRepository accountRepository)
        {
            this.logger = logger;
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
            
            logger.Information("Created admin & test accounts");
        }
    }
}