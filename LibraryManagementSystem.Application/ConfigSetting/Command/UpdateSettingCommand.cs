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
    public class UpdateSettingCommand : UpdateSettingRequestModel, IRequest<Result<string>>
    {

    }

    public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, Result<string>>
    {
        private readonly ISettingsService _settingsService;
        public UpdateSettingCommandHandler(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public async Task<Result<string>> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
        {
            return await _settingsService.UpdateSettingAsync(request);
        }
    }
}
