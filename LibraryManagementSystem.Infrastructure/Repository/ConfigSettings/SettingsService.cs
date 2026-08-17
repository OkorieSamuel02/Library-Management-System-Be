using AutoMapper;
using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Application.ConfigSetting.DataTransferObject.Request;
using LibraryManagementSystem.Application.ConfigSetting.Interface;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Infrastructure.Repository.BookCatalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Repository.ConfigSettings
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SettingsService> _logger;
        private readonly IMapper _mapper;
        public SettingsService(ApplicationDbContext context, ILogger<SettingsService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<string>> CreateSettingAsync(CreateSettingRequestModel createSetting)
        {
            try
            {
                var settingAlreadyExist = await _context.Settings.FirstOrDefaultAsync();
                if(settingAlreadyExist != null)
                {
                    settingAlreadyExist.loanPeriodDays = createSetting.loanPeriodDays;
                    settingAlreadyExist.fineRatePerDay = createSetting.fineRatePerDay;
                    settingAlreadyExist.UnpaidFinethreshold = createSetting.UnpaidFinethreshold;
                    settingAlreadyExist.maxActiveLoans = createSetting.maxActiveLoans;

                    var updated = await _context.SaveChangesAsync();

                    if (updated == 0)
                    {
                        return Result<string>.Failure("No changes were made to the settings.", System.Net.HttpStatusCode.BadRequest);
                    }

                    return Result<string>.Success("Settings updated successfully", settingAlreadyExist.id.ToString(), System.Net.HttpStatusCode.OK);

                }
                else
                {
                    var setting = new Setting
                    {
                        fineRatePerDay = createSetting.fineRatePerDay,
                        loanPeriodDays = createSetting.loanPeriodDays,
                        maxActiveLoans = createSetting.maxActiveLoans,
                        UnpaidFinethreshold = createSetting.UnpaidFinethreshold,
                    };

                    await _context.Settings.AddAsync(setting);
                    var savedAsync = await _context.SaveChangesAsync();
                    if (savedAsync == 0)
                    {
                        _logger.LogError($"An unexpected error occurred while trying to save books");
                        return Result<string>.Failure($"An unexpected error occurred while trying to save books", System.Net.HttpStatusCode.InternalServerError);
                    }

                    return Result<string>.Success($"Setting Created Successfully", setting.id.ToString(), System.Net.HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<string>> UpdateSettingAsync(UpdateSettingRequestModel createSetting)
        {
            try
            {
                var setting = await _context.Settings.FirstOrDefaultAsync();

                if (setting == null)
                {
                    return Result<string>.Failure("Settings have not been configured.", System.Net.HttpStatusCode.NotFound);
                }
                setting.UnpaidFinethreshold = createSetting.UnpaidFinethreshold ?? setting.UnpaidFinethreshold;
                setting.loanPeriodDays = createSetting.loanPeriodDays ?? setting.loanPeriodDays;
                setting.maxActiveLoans = createSetting.maxActiveLoans ?? setting.maxActiveLoans;
                setting.fineRatePerDay = createSetting.fineRatePerDay ?? setting.fineRatePerDay;

               
                var saved = await _context.SaveChangesAsync();

                if (saved == 0)
                {
                    _logger.LogError($"An unexpected error occurred while trying to save books");
                    return Result<string>.Failure($"An unexpected error occurred while trying to save books", System.Net.HttpStatusCode.InternalServerError);
                }

                return Result<string>.Success($"Book Updated Successfully", setting.id.ToString(), System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                return Result<string>.Failure($"An unexpected error occurred", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
