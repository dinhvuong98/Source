using AutoMapper;
using Dapper.FastCrud;
using Data;
using Data.Entity.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Util;
using Services.Dto.Shared;
using Services.Dtos.Temp;
using Services.Interfaces.Internal;
using Services.Interfaces.RedisCache;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Common.Dependency;
using Utilities.Configuation;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Extensions;

namespace Services.Implementation.Internal
{
    public class FileService : BaseService, IFileService
    {
        #region Property
        private readonly VStorageConfig _storageConfig = SingletonDependency<IOptions<VStorageConfig>>.Instance.Value;
        private readonly ILogger<FileService> _logger;
        private readonly ICacheProvider _redisCacheProvider;

        #endregion

        #region Constructor

        public FileService(ICacheProvider redisCacheProvider, DatabaseConnectService databaseConnectService, ILogger<FileService> logger) : base(databaseConnectService)
        {
            _redisCacheProvider = redisCacheProvider;
            _logger = logger;
        }


        #endregion

        #region Public Method

        public async Task<TempUploadFileDto> UploadTempFile(IFormFile file)
        {
            _logger.LogInformation("Start method upload temp file");
            ValidateFile(file);

            Guid result = Guid.Empty;
            try
            {
                result = await UploadFile(_storageConfig.FilesContainer, file);
            }catch(Exception e)
            {
                throw new BusinessException(e.Message, ErrorCode.VSTORAGE_ERROR);
            }

            using (IDbTransaction transaction = this.DatabaseConnectService.BeginTransaction())
            {
                try
                {
                    var tempFile = ToTempUploadFile(result, file.FileName, System.IO.Path.GetExtension(file.FileName), _storageConfig.FilesContainer);
                    await this.DatabaseConnectService.Connection.InsertAsync(tempFile, x => x.AttachToTransaction(transaction));

                    var fileManage = ToFileManager(result, file.FileName, System.IO.Path.GetExtension(file.FileName));
                    await this.DatabaseConnectService.Connection.InsertAsync(fileManage, x => x.AttachToTransaction(transaction));

                    transaction.Commit();
                    var resultFile = Mapper.Map<TempUploadFileDto>(tempFile);
                    _logger.LogInformation("End of method upload temp file");
                    return resultFile;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new BusinessException(e.Message, ErrorCode.FAIL);
                }
            }
        }

        public async Task<bool> DeleteFile(string container, string fileName)
        {
            var authToken = await GetAuthToken();

            return await DeleteVStorageFile(authToken, container + "/" + fileName);
        }


        public async Task<FileDescription> DownloadFile(string fileName)
        {
            _logger.LogInformation("Start method download vstorage file");
            using (HttpClient client = new HttpClient())
            {
                var token = await GetAuthToken();
                client.BaseAddress = new Uri(_storageConfig.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);

                HttpResponseMessage response = await client.GetAsync(_storageConfig.BaseUrl + "/" + _storageConfig.FilesContainer + "/" + fileName);
                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException("Download failed", ErrorCode.INTERNAL_SERVER_ERROR);
                }
                var result = await GetFileDescription(Guid.Parse(fileName), EncryptDecryptHelper.DecryptBytes(await response.Content.ReadAsByteArrayAsync()));
                _logger.LogInformation("End of method download vstorage file");
                return result;
            }
        }

        public async Task CleanTempUploadFile()
        {
            _logger.LogInformation("Start method clean temp upload file");
            using (IDbTransaction transaction = this.DatabaseConnectService.BeginTransaction())
            {
                try
                {
                    var tempUploadFiles = await this.DatabaseConnectService.Connection.FindAsync<TempUploadFile>(x => x
                                                                    .AttachToTransaction(transaction)
                                                                    .Where($"created_at < @dateFilter")
                                                                    .WithParameters(new { dateFilter = (DateTime.UtcNow).AddHours(-3) }));
                    if(tempUploadFiles.Count() > 0)
                    {
                        await this.DatabaseConnectService.Connection.BulkDeleteAsync<TempUploadFile>(x => x
                                                                        .AttachToTransaction(transaction)
                                                                        .Where($"created_at < @dateFilter")
                                                                        .WithParameters(new { dateFilter = (DateTime.UtcNow).AddHours(-3) }));

                        await this.DatabaseConnectService.Connection.BulkDeleteAsync<FileManager>(x => x
                                                                        .AttachToTransaction(transaction)
                                                                        .Where($"id in @fileIds")
                                                                        .WithParameters(new { fileIds = tempUploadFiles.Select(x => x.Id) }));

                        foreach (var item in tempUploadFiles)
                        {
                            await DeleteFile(item.Container, item.Id.ToString());
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            _logger.LogInformation("End of method clean temp upload file");
        }

        #endregion

        #region Private Method

        private void ValidateFile(IFormFile file)
        {
            _logger.LogInformation("Start method validate file");
            string[] contentType = new[] { "image/jpg", "image/jpeg", "image/pjpeg", "image/x-png", "image/png" ,
                                        "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv",
                                        "application/pdf"};
            long maxSize = 10 * 1024 * 1024; //maxSize = 10 MB
           
            //Check format file
            if (!contentType.Contains(file.ContentType.ToLower()))
            {
                throw new BusinessException("Invalid format file!", ErrorCode.INVALID_FORMAT_FILE);
            }
            //Check max size
            if (file.Length > maxSize)
            {
                throw new BusinessException("Invalid size file!", ErrorCode.INVALID_SIZE_FILE);
            }
            _logger.LogInformation("End of method validate file");
        }

        private async Task<Guid> UploadFile(string container, IFormFile file)
        {
            var authToken = await GetAuthToken();
            return await UploadFileToVStorage(container, authToken, file);
        }

        private async Task<string> GetAuthToken()
        {
            _logger.LogInformation("Start method get vstorage auth token");
            var token = _redisCacheProvider.GetByKey<string>(RedisCacheKey.KEY_VSTORAGE_TOKEN);
            if (token == null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_storageConfig.AuthUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var body = "{\"auth\":{" +
                            "\"identity\": {" +
                                "\"methods\": [\"password\"]," +
                                "\"password\": {" +
                                    "\"user\": {" +
                                        "\"domain\": {\"name\": \"default\"}," +
                                        "\"name\": \"" + _storageConfig.Username + "\"," +
                                        "\"password\": \"" + _storageConfig.Password + "\"" +
                                    "}" +
                                "}" +
                            "}," +
                            "\"scope\": {" +
                                "\"project\": {" +
                                    "\"domain\": {\"name\": \"default\"}," +
                                    "\"id\": \"" + _storageConfig.ProjectId + "\"" +
                                "}" +
                            "}" +
                        "}" +
                    "}";
                    var content = new StringContent(body, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(_storageConfig.AuthUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new BusinessException("Cannot connect to vStorage", ErrorCode.VSTORAGE_ERROR);
                    }

                    //string data = await response.Content.ReadAsStringAsync();

                    //VStorageAuthResponseDto result = data.FromJsonString<VStorageAuthResponseDto>();
                    //result.SubjectToken = response.Headers.First(x => x.Key == "X-Subject-Token").Value.First();

                    token = response.Headers.First(x => x.Key == "X-Subject-Token").Value.First();
                    _redisCacheProvider.Set(RedisCacheKey.KEY_VSTORAGE_TOKEN, token, TimeSpan.FromMinutes(RedisCacheTime.VStorageExpiredMinutes));
                }
            }
            _logger.LogInformation("End of method get vstorage auth token");
            return token;

        }

        private string GetVStorageUrl(VStorageAuthResponseDto authResponseDto)
        {
            _logger.LogInformation("Start method get vstorage url");
            string result = authResponseDto.Token.Catalogs.SelectMany(x => x.Endpoints).First(x => x.Url.Contains("AUTH") && x.Interface == "public").Url;
            _logger.LogInformation("End of method get vstorage url");
            return result;
        }

        private async Task<Guid> UploadFileToVStorage(string containerName, string token, IFormFile file)
        {
            _logger.LogInformation("Start method upload file to vstorage");
            var fileName = Guid.NewGuid();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_storageConfig.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);

                var bytes = file.OpenReadStream().GetAllBytes();

                var encryptedBytes = EncryptDecryptHelper.EncryptBytes(bytes);

                HttpResponseMessage response = await client.PutAsync(_storageConfig.BaseUrl + "/" + containerName + "/" + fileName, new ByteArrayContent(encryptedBytes));
                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException("Upload failed", ErrorCode.INTERNAL_SERVER_ERROR);
                }
            }
            _logger.LogInformation("Start method upload file to vstorage");
            return fileName;
        }

        private async Task<bool> DeleteVStorageFile(string token, string filePath)
        {
            _logger.LogInformation("Start method delete vstorage file");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_storageConfig.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);

                HttpResponseMessage response = await client.DeleteAsync(_storageConfig.BaseUrl + "/" + filePath);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
            }
            _logger.LogInformation("End of method delete vstorage file");
            return true;
        }

        private TempUploadFile ToTempUploadFile(Guid fileName, string orgFileName, string extension, string container, string url = null)
        {
            return new TempUploadFile
            {
                Id = fileName,
                OrgFileName = orgFileName,
                OrgFileExtension = extension,
                Container = container,
                FileUrl = url,
                CreatedAt = DateTime.UtcNow,
            };
        }

        private FileManager ToFileManager(Guid fileId, string orgFileName, string extension)
        {
            return new FileManager
            {
                Id = fileId,
                OrgFileName = orgFileName,
                OrgFileExtension = extension,
                CreateAt = DateTime.UtcNow
            };
        }

        private async Task<FileDescription> GetFileDescription(Guid fileId, byte[] bytes)
        {
            _logger.LogInformation("Start method get file description");
            try
            {
                var fileManager = (await this.DatabaseConnectService.Connection.FindAsync<FileManager>(x => x
                                                                    .Where($"id = @fileId")
                                                                    .WithParameters(new { fileId = fileId}))).FirstOrDefault();
                var fileResult = new FileDescription
                {
                    ContentType = fileManager == null && fileManager.OrgFileName.IsNullOrWhiteSpace() ? CommonConstants.DefaultImageContentType : GetMimeType(fileManager.OrgFileName),
                    Data = bytes,
                    FileName = (fileManager == null || fileManager.OrgFileName.IsNullOrWhiteSpace() ? fileId + CommonConstants.DefaultImageExtension : fileManager.OrgFileName)
                };
                _logger.LogInformation("End of method get file description");
                return fileResult;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string GetMimeType(string fileName)
        {
            _logger.LogInformation("Start method get mime type");
            string mimeType = CommonConstants.DefaultImageContentType;
            string ext = System.IO.Path.GetExtension(fileName).ToLower();

            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            _logger.LogInformation("End of method get mime type");
            return mimeType;
        }

        #endregion

    }
}
