using DataRepository.DataSource;
using DataRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public interface IMailConfigurationRespository
    {
        List<MailConfiguration> GetMailConfigurationsByUserId(string userId);

        MailConfiguration GetMailConfigurationsById(int Id);

        StatusResponse ModifyMailConfiguration(MailConfiguration mailConfiguration);

    }
}
