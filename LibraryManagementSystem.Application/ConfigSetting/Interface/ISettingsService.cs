using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.ConfigSetting.DataTransferObject.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.ConfigSetting.Interface
{
    public interface ISettingsService 
    {
        Task<Result<string>> CreateSettingAsync(CreateSettingRequestModel createSetting);
        Task<Result<string>> UpdateSettingAsync(UpdateSettingRequestModel createSetting);
    }
}
