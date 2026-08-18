using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.ConfigSetting.DataTransferObject.Request;
using LibraryManagementSystem.Application.ConfigSetting.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.ConfigSetting.Command
{
    public class CreateSettingCommand : CreateSettingRequestModel, IRequest<Result<string>>
    {

    }

    public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, Result<string>>
    {
        private readonly ISettingsService _settingsService;
        public CreateSettingCommandHandler(ISettingsService settingsService)
        {
            _settingsService = settingsService;   
        }
        public async Task<Result<string>> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
        {
            return await _settingsService.CreateSettingAsync(request);
        }
    }
}
