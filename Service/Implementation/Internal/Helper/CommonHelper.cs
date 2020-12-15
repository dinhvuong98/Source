using Dapper.FastCrud;
using Data;
using Data.Entity.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Services.Dto.Shared;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Common.Dependency;
using Utilities.Configuation;
using Utilities.Configurations;
using Utilities.Extensions;

namespace Services.Implementation.Internal.Helpers
{
    public static class CommonHelper
    {
        private readonly static VStorageConfig _storageConfig = SingletonDependency<IOptions<VStorageConfig>>.Instance.Value;
        public static string ToFullName(string firstName, string lastName)
        {
            return new[]
            {
                firstName,
                lastName
            }.JoinNotEmpty(" ");
        }

        public static FileDescription ToFileDescription(this IFormFile file)
        {
            return file == null
                ? null
                : new FileDescription
                {
                    FileName = file.FileName,
                    Data = file.OpenReadStream().GetAllBytes(),
                    ContentType = file.ContentType
                };
        }

        public static bool IsImageFile(this FileDescription image)
        {
            if (image == null)
            {
                return false;
            }

            var imageContenType = new[] { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png" };

            return imageContenType.Contains(image.ContentType.ToLower());
        }

        public static async Task DeleteTempFileFromDb(Guid id, DatabaseConnectService databaseConnectService, IDbTransaction dbTransaction)
        {
            await databaseConnectService.Connection.BulkDeleteAsync<TempUploadFile>(x => x
                                                                    .AttachToTransaction(dbTransaction)
                                                                    .Where($"id = @id")
                                                                    .WithParameters(new { id = id }));
        }

        public static async Task DeleteMultiTempFileFromDb(Guid[] ids, DatabaseConnectService databaseConnectService, IDbTransaction dbTransaction)
        {
            await databaseConnectService.Connection.BulkDeleteAsync<TempUploadFile>(x => x
                                                                    .AttachToTransaction(dbTransaction)
                                                                    .Where($"id in @ids")
                                                                    .WithParameters(new { ids = ids }));
        }

        public static TempUploadFile GetTempFile(Guid id, DatabaseConnectService databaseConnectService, IDbTransaction dbTransaction)
        {
            return databaseConnectService.Connection.Find<TempUploadFile>(x => x
                                                                    .AttachToTransaction(dbTransaction)
                                                                    .Where($"id = @id")
                                                                    .WithParameters(new { id = id }))
                                                                    .FirstOrDefault();
        }
        /// <summary>
        /// Add to temp folder again to delete file
        /// </summary>
        public static async Task RollBackToTempUploadFolderToDelete(Guid id, string container, DatabaseConnectService databaseConnectService, IDbTransaction dbTransaction)
        {
            try
            {
                var file = new TempUploadFile
                {
                    Id = id,
                    Container = container,
                    CreatedAt = DateTime.UtcNow.AddHours(-3)

                };
                await databaseConnectService.Connection.InsertAsync(file, x => x.AttachToTransaction(dbTransaction));
            }
            catch (Exception e) { }
        }


        public static bool CompareObject(object obj, object another)
        {
            if (ReferenceEquals(obj, another))
                return true;
            if ((obj == null) || (another == null)) return false;
            //Compare two object's class, return false if they are difference
            if (obj.GetType() != another.GetType()) return false;

            var result = true;
            //Get all properties of obj
            //And compare each other
            foreach (var property in obj.GetType().GetProperties())
            {
                var objValue = property.GetValue(obj);
                var anotherValue = property.GetValue(another);
                if (!objValue.Equals(anotherValue)) result = false;
            }

            return result;
        }
    }
}
