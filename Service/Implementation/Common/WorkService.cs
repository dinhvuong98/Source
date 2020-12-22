using Dapper.FastCrud;
using Dapper;
using Data;
using Data.Entity.Common;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Temp;
using Services.Implementation.Common.Helpers;
using Services.Interfaces.Common;
using Services.Interfaces.Internal;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Helpers;

namespace Services.Implementation.Common
{
    public class WorkService : BaseService, IWorkService
    {
        private readonly ISessionService _sessionService;

        public WorkService(DatabaseConnectService databaseConnectService, ISessionService sessionService) : base(databaseConnectService)
        {
            _sessionService = sessionService;
            DatabaseConnectService = databaseConnectService;
        }

        #region Public methods
        

        public async Task<PageResultDto<WorkDto>> FilterWork(PageDto pageDto, FilterParamWorkDto filterParamWorkDto)
        {
            if (filterParamWorkDto.FromDate > filterParamWorkDto.ToDate)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var query = new StringBuilder();
            query.Append("SELECT a.id as Id, a.name as Name, a.execution_time as ExecutionTime, a.value as Value, a.modified_at as ModifiedAt, ");
            query.Append("      b.id as FarmingLocationId, b.name as FarmingLocationName, c.code as FarmingLocationCode, ");
            query.Append("      c.id as ShrimpBreedId, c.name as ShrimpBreedName, c.code as ShrimpBreedCode, c.description as ShrimpBreedDescription, c.attachment as ShrimpBreedAttachment, ");
            query.Append("      d.id as UserId, d.fullname as FullName, d.phone as Phone, d.email as Email, d.address as Address, ");
            query.Append("      f.id as ShrimpCropId, f.code as ShrimpCropCode, f.name as ShrimpCropName, f.from_date as ShrimpCropFromDate, f.to_date as ShrimpCropToDate, ");
            query.Append("      g.id as ManagementFactorId, g.name as ManagementFactorName, g.code as ManagementFactorCode, g.sample_value as SampleValue, g.description as Description, ");
            query.Append("      h.id as MeasureUnitId, h.name as MeasureUnitName, h.description as MeasureUnitDescription, ");
            query.Append("      CAST(STUFF((SELECT ', '+ convert(nvarchar(50), file_id) FROM bys_main.bys_work_picture WHERE work_id = a.id FOR XML PATH ('')), 1, 2, '') AS nvarchar(150)) AS Pictures ");
            
            query.Append("FROM bys_main.bys_work a ");
            query.Append("      LEFT JOIN bys_main.bys_farming_location b on b.id = a.farming_location_id ");
            query.Append("      LEFT JOIN bys_main.bys_shrimp_breed c on c.id = a.shrimp_breed_id ");
            query.Append("      LEFT JOIN bys_sc_account.bys_user d on d.id = a.curator ");
            query.Append("      LEFT JOIN bys_main.bys_shrimp_crop_management_factor e on e.id = a.shrimp_crop_management_factor_id ");
            query.Append("      LEFT JOIN bys_main.bys_shrimp_crop f on f.id = e.shrimp_crop_id ");
            query.Append("      LEFT JOIN bys_main.bys_management_factor g on g.id = e.management_factor_id ");
            query.Append("      LEFT JOIN bys_sc_common.bys_measure_unit h on h.id = g.unit_id ");

            query.Append("WHERE (@FromDate is null OR (a.execution_time >= @FromDate)) ");
            query.Append("      AND (@ToDate is null OR (a.execution_time <= @Todate)) ");
            query.Append("      AND (@FarmingLocationId is null OR (a.farming_location_id = @FarmingLocationId AND (@ShrimpCropId is null OR (f.id = @ShrimpCropId)))) ");
            query.Append("      AND (@FactorGroup is null OR (g.factor_group = @Factorgroup)) ");
            query.Append("      AND (@Curator is null OR (a.curator = @Curator)) ");
            query.Append("      AND a.status != @StatusWork ");

            if (filterParamWorkDto.Status == WorkStatus.LateDeadline.ToString())
            {
                query.Append("AND (a.execution_time < @Now AND a.value is null) ");
            }

            if (filterParamWorkDto.Status == WorkStatus.Completed.ToString())
            {
                query.Append("AND a.value is not null ");
            }

            query.Append("ORDER BY a.execution_time ");

            var param = new
            {
                Page = pageDto.Page,
                pageSize = pageDto.PageSize,
                FromDate = filterParamWorkDto.FromDate.FromUnixTimeStamp(),
                ToDate = filterParamWorkDto.ToDate.FromUnixTimeStamp(),
                FarmingLocationId = filterParamWorkDto.FarmingLocationId,
                ShrimpCropId = filterParamWorkDto.ShrimpCropId,
                FactorGroup = filterParamWorkDto.FactorGroup,
                Curator = filterParamWorkDto.Curator,
                StatusWork = StatusWork.Delete.ToString(),
                Now = DateTime.UtcNow
            };

            var count = (await this.DatabaseConnectService.SelectAsync<WorkResultDto>(query.ToString(), param)).Count;

            query.Append("OFFSET @Page * @PageSize ROWS FETCH NEXT @pageSize ROWS ONLY; ");

            var items = (await this.DatabaseConnectService.SelectAsync<WorkResultDto>(query.ToString(), param))
                        .Select(x => x.ToWorkDto()).ToArray();

            var result = new PageResultDto<WorkDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

            return result;
        }

        public async Task<bool> StopWork(Guid shrimpCropManagementFactorId)
        {
            var works = await GetWorks(shrimpCropManagementFactorId);

            using (IDbTransaction transaction = this.DatabaseConnectService.Connection.BeginTransaction())
            {
                try
                {
                    var shrimpCropManagementFactor = await GetShrimpCropManagementFactorById(shrimpCropManagementFactorId, transaction);

                    shrimpCropManagementFactor.Status = CropFactorStatus.StopWork.ToString();

                    await this.DatabaseConnectService.Connection.UpdateAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor, x => x.AttachToTransaction(transaction));

                    if (works != null)
                    {
                        var query = @"UPDATE bys_main.bys_work SET status = @Status WHERE id IN @Ids ";

                        var param = new
                        {
                            Status = StatusWork.Delete.ToString(),
                            Ids = works.Select(x => x.Id).ToArray()
                        };

                        this.DatabaseConnectService.Connection.Execute(query.ToString(), param, transaction);

                        transaction.Commit();
                    }

                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new BusinessException(e.Message, ErrorCode.INTERNAL_SERVER_ERROR);
                }
            }
        }

        public async Task<RecordResultDto> RecordWord(RecordDto dto)
        {
            ValidateRecordWork(dto);

            var work = await GetWorkById(dto.WorkId);

            work.Value = dto.Value;

            work.ModifiedAt = dto.ModifiedAt.FromUnixTimeStamp();

            work.ModifiedBy = _sessionService.UserId;

            await this.DatabaseConnectService.Connection.UpdateAsync<Work>(work);

            return new RecordResultDto
            {
                result = true,
                ModifiedAt = dto.ModifiedAt
            };
        }

        public async Task<bool> UpdatePicture(UpdateWorkPictureDto dto)
        {
            var count = await this.DatabaseConnectService.Connection.CountAsync<WorkPicture>(x => x
                        .Where($"bys_work_picture.work_id = @WorkId")
                        .WithParameters(new { WorkId = dto.WorkId }));

            if (dto.Pictures.Length + count > 3 || dto.WorkId == null)
            {
                throw new BusinessException("", ErrorCode.INVALID_PARAMETER);
            }

            using (IDbTransaction transaction = this.DatabaseConnectService.BeginTransaction())
            {
                try
                {
                    foreach (TempUploadFileDto temp in dto.Pictures)
                    {
                        WorkPicture workPicture = temp.ToWorkPicture();
                        workPicture.WorkId = dto.WorkId;

                        await this.DatabaseConnectService.Connection.InsertAsync<WorkPicture>(workPicture, x => x.AttachToTransaction(transaction));
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new BusinessException(e.Message, ErrorCode.INVALID_PARAMETER);
                }
            }
        }

        #endregion

        #region Private methods
        private async Task<Work[]> GetWorks(Guid shrimpCropManagementFactorId)
        {
            var query = new StringBuilder();

            query.Append("bys_work.shrimp_crop_management_factor_id = @Id AND bys_work.execution_time >= @Now ");
            query.Append("AND bys_work.value is null AND bys_work.status = @StatusWork");

            var param = new
            {
                Id = shrimpCropManagementFactorId,
                Now = DateTime.UtcNow,
                StatusWork = StatusWork.Alive.ToString()
            };

            var result = (await this.DatabaseConnectService.Connection.FindAsync<Work>(x => x
                        .Where($"{query}")
                        .WithParameters(param))).ToArray();

            return result;
        }

        private async Task<Work> GetWorkById(Guid id)
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<Work>(x => x
                            .Where($"bys_work.id = @Id")
                            .WithParameters(new { Id = id }))).FirstOrDefault();

            return result == null ? throw new BusinessException("Invalid Object!", ErrorCode.NOT_EXIST) : result;
        }

        private async Task<ShrimpCropManagementFactor> GetShrimpCropManagementFactorById(Guid id, IDbTransaction transaction)
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCropManagementFactor>(x => x.AttachToTransaction(transaction)
                        .Where($"bys_shrimp_crop_management_factor.id = @Id")
                        .WithParameters(new { Id = id }))).FirstOrDefault();

            return result == null ? throw new BusinessException("invalid object", ErrorCode.NOT_EXIST) : result;
        }

        private void ValidateRecordWork(RecordDto dto)
        {
            int parse;
            var check = Int32.TryParse(dto.Value, out parse);

            if (!check || dto.WorkId == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }
        }
        #endregion
    }
}
